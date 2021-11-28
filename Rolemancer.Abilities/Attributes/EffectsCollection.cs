using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Rolemancer.Abilities.Attributes
{
    [BurstCompile]
    [BurstCompatible]
    public struct AttributesCollection : IDisposable, IEnumerable<KeyValue<AttributeComplexKey, Attribute>>
    {
        private NativeHashMap<AttributeComplexKey, Attribute> _attributes;

        public AttributesCollection(AllocatorManager.AllocatorHandle handle)
        {
            _attributes = new NativeHashMap<AttributeComplexKey, Attribute>(10, handle);
        }

        public bool TryGet(AttributeComplexKey key, out Attribute attribute)
        {
            return _attributes.TryGetValue(key, out attribute);
        }

        public Attribute Get(AttributeComplexKey key)
        {
            return _attributes[key];
        }

        public bool Has(AttributeComplexKey key)
        {
            return _attributes.ContainsKey(key);
        }

        public void Set(AttributeComplexKey key, Attribute attribute)
        {
            _attributes[key] = attribute;
        }

        public bool Remove(AttributeComplexKey key)
        {
            return _attributes.Remove(key);
        }

        public NativeArray<Attribute> GetAllAttributes(AllocatorManager.AllocatorHandle handle)
        {
            return _attributes.GetValueArray(handle);
        }

        public void Dispose()
        {
            _attributes.Dispose();
        }

        public NativeArray<AttributeComplexKey> GetKeyArray(AllocatorManager.AllocatorHandle handle)
        {
            return _attributes.GetKeyArray(handle);
        }
        
        public Attribute this[AttributeComplexKey key]
        {
            get => _attributes[key];

            set => _attributes[key] = value;
        }

        public NativeHashMap<AttributeComplexKey, Attribute>.Enumerator GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<AttributeComplexKey, Attribute>> IEnumerable<KeyValue<AttributeComplexKey, Attribute>>.GetEnumerator()
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