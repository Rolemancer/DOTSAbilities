using System;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Rolemancer.AbilityTools.Targets;
using Unity.Collections;

namespace Rolemancer.AbilityTools.DataMapping
{
    public struct ValueAsTargetCollection : IDisposable
    {
        private NativeHashMap<ComplexKey<EffectDBKey>, TargetId> _effectAsTarget;
        
        public ValueAsTargetCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effectAsTarget = new NativeHashMap<ComplexKey<EffectDBKey>, TargetId>(10, handle);
        }

        public void AddEffect(ComplexKey<EffectDBKey> effectComplexKey, TargetId targetId)
        {
            _effectAsTarget[effectComplexKey] = targetId;
        }

        public bool TryGet(ComplexKey<EffectDBKey> effectComplexKey, out TargetId targetId)
        {
            return _effectAsTarget.TryGetValue(effectComplexKey, out targetId);
        }
        
        public TargetId GetEffectAsTarget(ComplexKey<EffectDBKey> effectComplexKey)
        {
            return _effectAsTarget[effectComplexKey];
        }

        public void Remove(ComplexKey<EffectDBKey> effectComplexKey)
        {
            _effectAsTarget.Remove(effectComplexKey);
        }
        
        public void Dispose()
        {
            _effectAsTarget.Dispose();
        }
    }
}