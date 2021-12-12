using System;
using Rolemancer.AbilityTools.Targets;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.AbilityTools.DataMapping
{
    public static class Mapping
    {
        public static DataMap Map;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Map = new DataMap(Allocator.Persistent);
        }
    }

    [BurstCompile]
    [BurstCompatible]
    public struct DataMap : IDisposable
    {
        public TargetAttributeCollection Attributes;
        public TargetEffectsCollection Effects;
        public ValueAsTargetCollection EffectAsTarget;

        private static int _targetIdCounter;

        public DataMap(AllocatorManager.AllocatorHandle handle)
        {
            Attributes = new TargetAttributeCollection(handle);
            Effects = new TargetEffectsCollection(handle);
            EffectAsTarget = new ValueAsTargetCollection(handle);
            _targetIdCounter = 0;
        }

        public TargetId GetNextTargetId()
        {
            var targetId = new TargetId(_targetIdCounter);
            _targetIdCounter++;
            return targetId;
        }

        public void Dispose()
        {
            Attributes.Dispose();
            Effects.Dispose();
            EffectAsTarget.Dispose();
        }
    }
}