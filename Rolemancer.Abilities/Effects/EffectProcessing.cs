﻿using Rolemancer.Abilities.DataMapping;
using Rolemancer.Abilities.Modifiers;
using Rolemancer.Abilities.Tests.ConcreteModifierProcessors;
using Unity.Collections;
using Unity.Jobs;

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
            // Process Attributes
            var updateAttributes = new UpdateTargetAttributes { Map = map, ElapsedTime = elapsedTime };
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
                        effect.DiscardTime = ElapsedTime + effect.LifeTime;
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
                        effect.Status = EffectStatus.Discarded;
                        effectsCollection[key] = effect;
                    }
                }

                keys.Dispose();
            }
        }

        private struct UpdateTargetAttributes : IJob
        {
            public DataMap Map;
            public double ElapsedTime;

            public void Execute()
            {
                UpdateAttributes();
            }

            private void UpdateAttributes()
            {
                var effects = Map.Effects.GetAllEffects();
                var affectedTargets = Map.Effects.GetAffectedTargets();

                for (var i = 0; i < affectedTargets.Length; i++)
                {
                    var targetId = affectedTargets[i];

                    var prev = Map.Attributes.GetTargetAttributes(targetId, Allocator.Temp);
                    var current = Map.Attributes.GetTargetAttributes(targetId, Allocator.Temp);

                    var targetEffects = Map.Effects.GetTargetEffects(targetId, Allocator.Temp);
                    var keys = targetEffects.GetKeyArray(Allocator.Temp);
                    for (var j = 0; j < keys.Length; j++)
                    {
                        var effectKey = keys[j];
                        var effect = targetEffects[effectKey];

                        var effectAttrs = effectKey.GetAttributes(Map, Allocator.Temp);

                        var isImmediate = effect.ApplyMode == EffectApplyMode.Immediate;

                        if (isImmediate || effect.Status == EffectStatus.Pending)
                        {
                            current.Append(effectAttrs);
                            effect.Status = EffectStatus.Applied;

                            if (isImmediate)
                                effect.Status = EffectStatus.Discarded;

                            effects[effectKey] = effect;
                        }
                        else if (effect.Status == EffectStatus.Discarded)
                        {
                            current.Truncate(effectAttrs);
                            effects[effectKey] = effect;
                        }
                    }

                    keys.Dispose();

                    var processorArguments = new ProcessorArguments
                    {
                        Target = targetId,
                        CurrentAttributes = current,
                        PrevAttributes = prev
                    };

                    ModifierProcessorsRunner.ProcessAttributes(BattleModifierProcessorLibrary.ProcessorFunctions,
                        ref processorArguments);

                    Map.Attributes.ReplaceTargetAttributes(targetId, current);

                    targetEffects.Dispose();
                    prev.Dispose();
                    current.Dispose();
                }

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
                    key.DestroyEffect(Map);
                }

                keys.Dispose();
            }
        }
    }
}