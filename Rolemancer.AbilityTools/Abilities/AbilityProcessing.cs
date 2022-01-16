using Rolemancer.AbilityTools.DataMapping;
using Rolemancer.AbilityTools.Effects;
using Unity.Collections;
using Unity.Jobs;

namespace Rolemancer.AbilityTools.Abilities
{
    public static class AbilityProcessing
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

            pendingHandle.Complete();
            checkLifeTimeHandle.Complete();
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

                    if (ElapsedTime >= finishTime) key.DiscardEffect(Map);
                }

                keys.Dispose();
            }
        }
    }
}