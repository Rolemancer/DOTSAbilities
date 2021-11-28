using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Rolemancer.Abilities.Effects
{
    [BurstCompile]
    [BurstCompatible]
    public struct EffectsDBKeysCollection : IDisposable, IEnumerable<KeyValue<EffectDBKey, Effect>>
    {
        private NativeHashMap<EffectDBKey, Effect> _effects;

        public EffectsDBKeysCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effects = new NativeHashMap<EffectDBKey, Effect>(10, handle);
        }

        public bool TryGet(EffectDBKey key, out Effect effect)
        {
            return _effects.TryGetValue(key, out effect);
        }

        public Effect Get(EffectDBKey key)
        {
            return _effects[key];
        }

        public bool Has(EffectDBKey key)
        {
            return _effects.ContainsKey(key);
        }

        public void Set(Effect effect)
        {
            _effects[effect.DBKey] = effect;
        }

        public bool Remove(Effect effect)
        {
            return Remove(effect.DBKey);
        }

        public bool Remove(EffectDBKey dbKey)
        {
            return _effects.Remove(dbKey);
        }

        public NativeArray<Effect> GetAllEffects(AllocatorManager.AllocatorHandle handle)
        {
            return _effects.GetValueArray(handle);
        }

        public void Dispose()
        {
            _effects.Dispose();
        }

        public NativeHashMap<EffectDBKey, Effect>.Enumerator GetEnumerator()
        {
            return _effects.GetEnumerator();
        }


        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<EffectDBKey, Effect>> IEnumerable<KeyValue<EffectDBKey, Effect>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}