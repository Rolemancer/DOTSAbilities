using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Rolemancer.AbilityTools.Base
{
    [BurstCompile]
    [BurstCompatible]
    public struct DataByComplexKeyCollection<TDbKey, TTypeWithKey> : 
        IDisposable,
        IEnumerable<KeyValue<ComplexKey<TDbKey>, TTypeWithKey>>
            where TDbKey : unmanaged, IEquatable<TDbKey>
            where TTypeWithKey : unmanaged, ITypeWithDbKey<TDbKey>
    {
        private NativeParallelHashMap<ComplexKey<TDbKey>, TTypeWithKey> _items;

        public DataByComplexKeyCollection(AllocatorManager.AllocatorHandle handle)
        {
            _items = new NativeParallelHashMap<ComplexKey<TDbKey>, TTypeWithKey>(10, handle);
        }

        public bool TryGet(ComplexKey<TDbKey> key, out TTypeWithKey typeWithKey)
        {
            return _items.TryGetValue(key, out typeWithKey);
        }

        public TTypeWithKey Get(ComplexKey<TDbKey> key)
        {
            return _items[key];
        }

        public bool Has(ComplexKey<TDbKey> key)
        {
            return _items.ContainsKey(key);
        }

        public void Set(ComplexKey<TDbKey> key, TTypeWithKey typeWithKey)
        {
            _items[key] = typeWithKey;
        }

        public bool Remove(ComplexKey<TDbKey> key)
        {
            return _items.Remove(key);
        }

        public NativeArray<TTypeWithKey> GetAllAttributes(AllocatorManager.AllocatorHandle handle)
        {
            return _items.GetValueArray(handle);
        }

        public void Dispose()
        {
            _items.Dispose();
        }

        public NativeArray<ComplexKey<TDbKey>> GetKeyArray(AllocatorManager.AllocatorHandle handle)
        {
            return _items.GetKeyArray(handle);
        }

        public TTypeWithKey this[ComplexKey<TDbKey> key]
        {
            get => _items[key];

            set => _items[key] = value;
        }

        public NativeParallelHashMap<ComplexKey<TDbKey>, TTypeWithKey>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<ComplexKey<TDbKey>, TTypeWithKey>> IEnumerable<KeyValue<ComplexKey<TDbKey>, TTypeWithKey>>.
            GetEnumerator()
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