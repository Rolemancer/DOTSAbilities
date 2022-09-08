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
    public struct DataByDbKeyCollection<TDbKey, TTypeWithKey> : IDisposable,
        IEnumerable<KeyValue<TDbKey, TTypeWithKey>>
        where TDbKey : unmanaged, IEquatable<TDbKey>
        where TTypeWithKey : unmanaged, ITypeWithDbKey<TDbKey>
    {
        private NativeHashMap<TDbKey, TTypeWithKey> _items;

        public DataByDbKeyCollection(AllocatorManager.AllocatorHandle handle)
        {
            _items = new NativeHashMap<TDbKey, TTypeWithKey>(10, handle);
        }

        public TTypeWithKey this[TDbKey key]
        {
            get => _items[key];

            set => _items[key] = value;
        }

        public NativeHashMap<TDbKey, TTypeWithKey>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<TDbKey, TTypeWithKey>> IEnumerable<KeyValue<TDbKey, TTypeWithKey>>.
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

        public bool TryGet(TDbKey id, out TTypeWithKey TTypeWithKey)
        {
            return _items.TryGetValue(id, out TTypeWithKey);
        }

        public TTypeWithKey Get(TDbKey id)
        {
            return _items[id];
        }

        public bool Has(TDbKey id)
        {
            return _items.ContainsKey(id);
        }

        public void Set(TTypeWithKey typeWithKey)
        {
            _items[typeWithKey.DbKey] = typeWithKey;
        }

        public bool Remove(TDbKey key)
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
    }
}