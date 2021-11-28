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
    public struct EffectsCollection : IDisposable, IEnumerable<KeyValue<EffectComplexKey, Effect>>
    {
        private NativeHashMap<EffectComplexKey, Effect> _effects;

        public EffectsCollection(AllocatorManager.AllocatorHandle handle)
        {
            _effects = new NativeHashMap<EffectComplexKey, Effect>(10, handle);
        }

        public bool TryGet(EffectComplexKey key, out Effect effect)
        {
            return _effects.TryGetValue(key, out effect);
        }

        public Effect Get(EffectComplexKey key)
        {
            return _effects[key];
        }

        public bool Has(EffectComplexKey key)
        {
            return _effects.ContainsKey(key);
        }

        public void Set(EffectComplexKey key, Effect effect)
        {
            _effects[key] = effect;
        }

        public bool Remove(EffectComplexKey key)
        {
            return _effects.Remove(key);
        }

        public NativeArray<Effect> GetAllEffects(AllocatorManager.AllocatorHandle handle)
        {
            return _effects.GetValueArray(handle);
        }

        public void Dispose()
        {
            _effects.Dispose();
        }

        public NativeArray<EffectComplexKey> GetKeyArray(AllocatorManager.AllocatorHandle handle)
        {
            return _effects.GetKeyArray(handle);
        }
        public Effect this[EffectComplexKey key]
        {
            get => _effects[key];

            set => _effects[key] = value;
        }

        public NativeHashMap<EffectComplexKey, Effect>.Enumerator GetEnumerator()
        {
            return _effects.GetEnumerator();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<EffectComplexKey, Effect>> IEnumerable<KeyValue<EffectComplexKey, Effect>>.GetEnumerator()
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