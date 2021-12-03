using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.Attributes
{
    [BurstCompatible]
    [BurstCompile]
    public struct AttributeDBKeysCollection : IDisposable, IEnumerable<AttributeDBKey>
    {
        private NativeHashSet<AttributeDBKey> _keys;

        public AttributeDBKeysCollection(AllocatorManager.AllocatorHandle handle)
        {
            _keys = new NativeHashSet<AttributeDBKey>(4, handle);
        }

        public bool Has(AttributeDBKey id)
        {
            return _keys.Contains(id);
        }

        public void Set(AttributeDBKey attributeDBKey)
        {
            _keys.Add(attributeDBKey);
        }

        public void Set(Attribute attribute)
        {
            var id = attribute.DbKey;
            Set(id);
        }

        public void Append(AttributeCollectionByDBKey attributes)
        {
            var temp = attributes.GetAllAttributes(Allocator.Temp);
            for (int i = 0; i < temp.Length; i++)
            {
                Set(temp[i]);
            }
            temp.Dispose();
        }
        
        public bool Remove(AttributeDBKey id)
        {
            return _keys.Remove(id);
        }

        public NativeArray<AttributeDBKey> GetAll(AllocatorManager.AllocatorHandle handle)
        {
            return _keys.ToNativeArray(handle);
        }

        public void Dispose()
        {
            _keys.Dispose();
        }

        public NativeHashSet<AttributeDBKey>.Enumerator GetEnumerator()
        {
            return _keys.GetEnumerator();
        }


        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<AttributeDBKey> IEnumerable<AttributeDBKey>.GetEnumerator()
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