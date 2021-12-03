using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.DataMapping;
using Rolemancer.Abilities.ModifierProcessors;
using Rolemancer.Abilities.Modifiers;
using Rolemancer.Abilities.Targets;
using Rolemancer.Abilities.Tests.ConcreteModifierProcessors;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rolemancer.Abilities.Effects
{
    public static class EffectProcessing
    {
        public static void Process(double elapsedTime)
        {
            var map = Mapping.Map;

            // Pending
            var pending = new PendingJob { Map = map, ElapsedTime = elapsedTime };
            var pendingHandle = pending.Schedule();
            // Update Life Time
            var checkLifeTime = new CheckLifeTime { Map = map, ElapsedTime = elapsedTime };
            var checkLifeTimeHandle = checkLifeTime.Schedule(pendingHandle);
            // Prepare Arguments
            var updateAttributes = new UpdateAttributes { Map = map, ElapsedTime = elapsedTime, };
            var updateAttributesHandle = updateAttributes.Schedule(checkLifeTimeHandle);
            // Remove Disposed
            var removeDiscarded = new RemoveJob() { Map = map };
            var removeDiscardedHandle = removeDiscarded.Schedule(updateAttributesHandle);

            pendingHandle.Complete();
            checkLifeTimeHandle.Complete();
            updateAttributesHandle.Complete();
            removeDiscardedHandle.Complete();
        }

        private struct PendingJob : IJob
        {
            public DataMap Map;
            public double ElapsedTime;

            public void Execute()
            {
                var effectsCollection = Map.Effects.GetAllEffects();
                var keys = effectsCollection.GetKeyArray(Allocator.Temp);

                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var effect = effectsCollection[key];
                    if (effect.Status != EffectStatus.Creating)
                        continue;
                    effect.Status = EffectStatus.Pending;
                    if (effect.ApplyMode == EffectApplyMode.LongLife)
                    {
                        effect.DiscardTime = ElapsedTime + effect.LifeTime;
                    }
                    effectsCollection[key] = effect;
                }

                keys.Dispose();
            }
        }

        private struct CheckLifeTime : IJob
        {
            public DataMap Map;
            public double ElapsedTime;

            public void Execute()
            {
                var effectsCollection = Map.Effects.GetAllEffects();
                var keys = effectsCollection.GetKeyArray(Allocator.Temp);
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var effect = effectsCollection[key];

                    if (effect.ApplyMode == EffectApplyMode.Immediate)
                        continue;

                    if (effect.Status == EffectStatus.Discarded)
                        continue;

                    var finishTime = effect.DiscardTime;

                    if (ElapsedTime >= finishTime)
                    {
                        key.DiscardEffect(Map);
                    }
                }

                keys.Dispose();
            }
        }

        private struct UpdateAttributes : IJob
        {
            public DataMap Map;
            public double ElapsedTime;
            
            public void Execute()
            {
                PrepareArguments();
            }

            private void PrepareArguments()
            {
                var effects = Map.Effects.GetAllEffects();
                var affectedTargets = Map.Effects.GetAffectedTargets(Allocator.Temp);

                for (var i = 0; i < affectedTargets.Length; i++)
                {
                    var targetId = affectedTargets[i];

                    var prev = Map.Attributes.GetTargetAttributes(targetId, Allocator.Temp);
                    var current = Map.Attributes.GetTargetAttributes(targetId, Allocator.Temp);

                    var targetEffects = Map.Effects.GetTargetEffects(targetId, Allocator.Temp);
                    var targetEffectKeys = targetEffects.GetKeyArray(Allocator.Temp);
                    var diff = new AttributeDBKeysCollection(Allocator.Temp);
                    
                    for (var j = 0; j < targetEffectKeys.Length; j++)
                    {
                        var effectKey = targetEffectKeys[j];
                        var effect = targetEffects[effectKey];

                        var effectAttrs = effectKey.GetAttributes(Map, Allocator.Temp);

                        var isImmediate = effect.ApplyMode == EffectApplyMode.Immediate;

                        if (isImmediate || effect.Status == EffectStatus.Pending)
                        {
                            current.Append(effectAttrs);
                            diff.Append(effectAttrs);
                            
                            effect.Status = EffectStatus.Applied;

                            if (isImmediate)
                                effect.Status = EffectStatus.Discarded;

                            effects[effectKey] = effect;
                        }
                        else if (effect.Status == EffectStatus.Discarded)
                        {
                            current.Truncate(effectAttrs);
                            //Yes, add keys of changed attributes
                            diff.Append(effectAttrs);
                            effects[effectKey] = effect;
                        }
                    }

                    var processorArguments = new ProcessorArguments
                    {
                        Target = targetId,
                        ElapsedTime = ElapsedTime,
                        CurrentAttributes = current,
                        PrevAttributes = prev,
                        Changed = diff
                    };
                    
                    ModifierProcessorsRunner.ProcessAttributes(BattleModifierProcessorLibrary.ProcessorFunctions,
                        ref processorArguments);

                    Map.Attributes.ReplaceTargetAttributes(processorArguments.Target, processorArguments.CurrentAttributes);
                    processorArguments.Dispose();
                    
                    targetEffects.Dispose();
                    targetEffectKeys.Dispose();
                }

                affectedTargets.Dispose();
                Map.Effects.ResetAffectedTargets();
            }
        }
        
        private struct RemoveJob : IJob
        {
            public DataMap Map;

            public void Execute()
            {
                var effectsCollection = Map.Effects.GetAllEffects();
                var keys = effectsCollection.GetKeyArray(Allocator.Temp);

                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var effect = effectsCollection.Get(key);
                    if (effect.Status == EffectStatus.Discarded)
                    {
                        key.DestroyEffect(Map);
                    }
                }

                keys.Dispose();
            }
        }
    }
}