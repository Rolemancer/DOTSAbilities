using System;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Rolemancer.AbilityTools.Targets;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.DataMapping
{
    [BurstCompile]
    [BurstCompatible]
    public struct TargetEffectsCollection : IDisposable
    {
        private DataByComplexKeyCollection<EffectDBKey, Effect> _effects;
        private NativeParallelMultiHashMap<TargetId, ComplexKey<EffectDBKey>> _targetEffects;
        private NativeParallelHashMap<ComplexKey<EffectDBKey>, TargetId> _effectToTarget;
        private NativeParallelHashSet<TargetId> _affectedTargets;
        
        public TargetEffectsCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effects = new DataByComplexKeyCollection<EffectDBKey, Effect>(handle);
            _targetEffects = new NativeParallelMultiHashMap<TargetId, ComplexKey<EffectDBKey>>(10, handle);
            _effectToTarget = new NativeParallelHashMap<ComplexKey<EffectDBKey>, TargetId>(10, handle);
            _affectedTargets = new NativeParallelHashSet<TargetId>(10, handle);
        }

        public void AddEffect(TargetId targetId, Effect effect)
        {
            var key = new ComplexKey<EffectDBKey>(targetId, effect.DbKey);
            _targetEffects.Add(key.Target, key);
            _effectToTarget[key] = key.Target;
            _effects[key] = effect;
            _affectedTargets.Add(targetId);
        }

        public void DiscardEffect(ComplexKey<EffectDBKey> key)
        {
            var effect = _effects.Get(key);
            effect.Status = EffectStatus.Discarded;
            _effects[key] = effect;
            _affectedTargets.Add(key.Target);
        }
        
        public bool HasEffects(TargetId targetId)
        {
            return _targetEffects.ContainsKey(targetId);
        }

        public bool HasEffect(TargetId targetId, Effect effect)
        {
            return HasEffect(targetId, effect.DbKey);
        }
        

        public bool HasEffect(ComplexKey<EffectDBKey> effectComplexKey)
        {
            return _effectToTarget.ContainsKey(effectComplexKey);
        }

        public bool HasEffect(TargetId targetId, EffectDBKey effectDBKey)
        {
            var complexKey = new ComplexKey<EffectDBKey>(targetId, effectDBKey);
            return HasEffect(complexKey);
        }

        public bool TryGetEffect(TargetId targetId, EffectDBKey effectDBKey, out Effect effect)
        {
            var complexKey = new ComplexKey<EffectDBKey>(targetId, effectDBKey);
            return _effects.TryGet(complexKey, out effect);
        }

        public NativeArray<TargetId> GetAllEffectTargets(AllocatorManager.AllocatorHandle handle)
        {
            return _targetEffects.GetKeyArray(handle);
        }

        public NativeParallelHashMap<ComplexKey<EffectDBKey>,Effect> GetTargetEffects(TargetId targetId, AllocatorManager.AllocatorHandle handle)
        {
            var count = _targetEffects.CountValuesForKey(targetId);
            var result = new NativeParallelHashMap<ComplexKey<EffectDBKey>,Effect>(count, handle);

            
            var valuesForKey = _targetEffects.GetValuesForKey(targetId);
            while (valuesForKey.MoveNext())
            {
                var effectId = valuesForKey.Current;
                result[effectId] = _effects[effectId];
            }
            valuesForKey.Dispose();

            return result;
        }

        public DataByComplexKeyCollection<EffectDBKey, Effect> GetAllEffects()
        {
            return _effects;
        }

        public void RemoveEffect(TargetId targetId, Effect effect)
        {
            var key = new ComplexKey<EffectDBKey>(targetId, effect.DbKey);
            RemoveEffect(key);
        }

        public void RemoveEffect(TargetId targetId, EffectDBKey effectDBKey)
        {
            var complexKey = new ComplexKey<EffectDBKey>(targetId, effectDBKey);
            RemoveEffect(complexKey);
        }

        public void RemoveEffect(ComplexKey<EffectDBKey> effectComplexKey)
        {
            _effectToTarget.Remove(effectComplexKey);
            _effects.Remove(effectComplexKey);

            var targetId = effectComplexKey.Target;
            _targetEffects.Remove(targetId, effectComplexKey);
            if (_targetEffects.CountValuesForKey(targetId) == 0)
                _targetEffects.Remove(targetId);

            _affectedTargets.Add(targetId);
        }

        public int GetAffectedTargetCount()
        {
            return _affectedTargets.Count();
        }
        
        public NativeArray<TargetId> GetAffectedTargets(AllocatorManager.AllocatorHandle handle)
        {
            return _affectedTargets.ToNativeArray(handle);
        }
        
        public void ResetAffectedTargets()
        {
            _affectedTargets.Clear();
        }

        public void Dispose()
        {
            _effects.Dispose();
            _targetEffects.Dispose();
            _effectToTarget.Dispose();
            _affectedTargets.Dispose();
        }
    }
}