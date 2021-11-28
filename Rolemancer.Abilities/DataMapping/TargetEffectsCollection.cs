using System;
using Rolemancer.Abilities.Effects;
using Rolemancer.Abilities.Targets;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.DataMapping
{
    [BurstCompile]
    [BurstCompatible]
    public struct TargetEffectsCollection : IDisposable
    {
        private EffectsCollection _effects;
        private NativeMultiHashMap<TargetId, EffectComplexKey> _targetEffects;
        private NativeHashMap<EffectComplexKey, TargetId> _effectToTarget;
        private NativeList<TargetId> _affectedTargets;
        
        public TargetEffectsCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effects = new EffectsCollection(handle);
            _targetEffects = new NativeMultiHashMap<TargetId, EffectComplexKey>(10, handle);
            _effectToTarget = new NativeHashMap<EffectComplexKey, TargetId>(10, handle);
            _affectedTargets = new NativeList<TargetId>(10, handle);
        }

        public void AddEffect(TargetId targetId, Effect effect)
        {
            var key = new EffectComplexKey(targetId, effect.DBKey);
            _targetEffects.Add(key.Target, key);
            _effectToTarget[key] = key.Target;
            _effects[key] = effect;
            _affectedTargets.Add(targetId);
        }

        public bool HasEffects(TargetId targetId)
        {
            return _targetEffects.ContainsKey(targetId);
        }

        public bool HasEffect(TargetId targetId, Effect effect)
        {
            return HasEffect(targetId, effect.DBKey);
        }
        

        public bool HasEffect(EffectComplexKey effectComplexKey)
        {
            return _effectToTarget.ContainsKey(effectComplexKey);
        }

        public bool HasEffect(TargetId targetId, EffectDBKey effectDBKey)
        {
            var complexKey = new EffectComplexKey(targetId, effectDBKey);
            return HasEffect(complexKey);
        }

        public bool TryGetEffect(TargetId targetId, EffectDBKey effectDBKey, out Effect effect)
        {
            var complexKey = new EffectComplexKey(targetId, effectDBKey);
            return _effects.TryGet(complexKey, out effect);
        }

        public NativeArray<TargetId> GetAllEffectTargets(AllocatorManager.AllocatorHandle handle)
        {
            return _targetEffects.GetKeyArray(handle);
        }

        public NativeHashMap<EffectComplexKey,Effect> GetTargetEffects(TargetId targetId, AllocatorManager.AllocatorHandle handle)
        {
            var count = _targetEffects.CountValuesForKey(targetId);
            var result = new NativeHashMap<EffectComplexKey,Effect>(count, handle);

            
            var valuesForKey = _targetEffects.GetValuesForKey(targetId);
            while (valuesForKey.MoveNext())
            {
                var effectId = valuesForKey.Current;
                result[effectId] = _effects[effectId];
            }
            valuesForKey.Dispose();

            return result;
        }

        public EffectsCollection GetAllEffects()
        {
            return _effects;
        }

        public void RemoveEffect(TargetId targetId, Effect effect)
        {
            var key = new EffectComplexKey(targetId, effect.DBKey);
            RemoveEffect(key);
        }

        public void RemoveEffect(TargetId targetId, EffectDBKey effectDBKey)
        {
            var complexKey = new EffectComplexKey(targetId, effectDBKey);
            RemoveEffect(complexKey);
        }

        public void RemoveEffect(EffectComplexKey effectComplexKey)
        {
            _effectToTarget.Remove(effectComplexKey);
            _effects.Remove(effectComplexKey);

            var targetId = effectComplexKey.Target;
            _targetEffects.Remove(targetId, effectComplexKey);
            if (_targetEffects.CountValuesForKey(targetId) == 0)
                _targetEffects.Remove(targetId);

            _affectedTargets.Add(targetId);
        }

        public NativeList<TargetId> GetAffectedTargets()
        {
            return _affectedTargets;
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