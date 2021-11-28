using System;
using Rolemancer.Abilities.Effects;
using Rolemancer.Abilities.Targets;
using Unity.Collections;

namespace Rolemancer.Abilities.DataMapping
{
    public struct ValueAsTargetCollection : IDisposable
    {
        private NativeHashMap<EffectComplexKey, TargetId> _effectAsTarget;
        
        public ValueAsTargetCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effectAsTarget = new NativeHashMap<EffectComplexKey, TargetId>(10, handle);
        }

        public void AddEffect(EffectComplexKey effectComplexKey, TargetId targetId)
        {
            _effectAsTarget[effectComplexKey] = targetId;
        }

        public bool TryGet(EffectComplexKey effectComplexKey, out TargetId targetId)
        {
            return _effectAsTarget.TryGetValue(effectComplexKey, out targetId);
        }
        
        public TargetId GetEffectAsTarget(EffectComplexKey effectComplexKey)
        {
            return _effectAsTarget[effectComplexKey];
        }

        public void Remove(EffectComplexKey effectComplexKey)
        {
            _effectAsTarget.Remove(effectComplexKey);
        }
        
        public void Dispose()
        {
            _effectAsTarget.Dispose();
        }
    }
}