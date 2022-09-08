using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Base
{
    [BurstCompatible]
    [BurstCompile]
    public struct DbKeysCollection<TDbKey> : IDisposable, IEnumerable<TDbKey>
        where TDbKey : unmanaged, IEquatable<TDbKey>
    {
        private NativeHashSet<TDbKey> _keys;

        public DbKeysCollection(AllocatorManager.AllocatorHandle handle)
        {
            _keys = new NativeHashSet<TDbKey>(4, handle);
        }

        public bool Has(TDbKey dbKey)
        {
            return _keys.Contains(dbKey);
        }

        public void Set(TDbKey dbKey)
        {
            _keys.Add(dbKey);
        }

        public bool Remove(TDbKey dbKey)
        {
            return _keys.Remove(dbKey);
        }

        public NativeArray<TDbKey> GetAll(AllocatorManager.AllocatorHandle handle)
        {
            return _keys.ToNativeArray(handle);
        }

        public void Dispose()
        {
            _keys.Dispose();
        }

        public NativeHashSet<TDbKey>.Enumerator GetEnumerator()
        {
            return _keys.GetEnumerator();
        }


        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<TDbKey> IEnumerable<TDbKey>.GetEnumerator()
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