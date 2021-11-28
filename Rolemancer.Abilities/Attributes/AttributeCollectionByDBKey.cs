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
    public struct AttributeCollectionByDBKey : IDisposable, IEnumerable<KeyValue<AttributeDBKey, Attribute>>
    {
        private NativeHashMap<AttributeDBKey, Attribute> _attributes;

        public AttributeCollectionByDBKey(AllocatorManager.AllocatorHandle handle)
        {
            _attributes = new NativeHashMap<AttributeDBKey, Attribute>(10, handle);
        }

        public Attribute this[AttributeDBKey key]
        {
            get => _attributes[key];

            set => _attributes[key] = value;
        }

        public NativeHashMap<AttributeDBKey, Attribute>.Enumerator GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        /// <summary>
        ///     This method is not implemented. Use <see cref="GetEnumerator" /> instead.
        /// </summary>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException">Method is not implemented.</exception>
        IEnumerator<KeyValue<AttributeDBKey, Attribute>> IEnumerable<KeyValue<AttributeDBKey, Attribute>>.
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

        public bool TryGet(AttributeDBKey id, out Attribute attribute)
        {
            return _attributes.TryGetValue(id, out attribute);
        }

        public Attribute Get(AttributeDBKey id)
        {
            return _attributes[id];
        }

        public bool Has(AttributeDBKey id)
        {
            return _attributes.ContainsKey(id);
        }

        public void Set(Attribute attribute)
        {
            _attributes[attribute.DbKey] = attribute;
        }

        public void Append(Attribute attribute)
        {
            var dbKey = attribute.DbKey;
            if (Has(dbKey))
            {
                var attr = Get(dbKey);
                attr.Value += attribute.Value;
                Set(attr);
            }
            else
            {
                Set(attribute);
            }
        }

        public void Truncate(Attribute attribute)
        {
            var dbKey = attribute.DbKey;
            if (Has(dbKey))
            {
                var attr = Get(dbKey);
                attr.Value -= attribute.Value;
                Set(attr);
            }
            else
            {
                Set(new Attribute{DbKey = dbKey, Value = -attribute.Value});
            }
        }
        
        public bool Remove(AttributeDBKey key)
        {
            return _attributes.Remove(key);
        }

        public NativeArray<Attribute> GetAllAttributes(AllocatorManager.AllocatorHandle handle)
        {
            return _attributes.GetValueArray(handle);
        }

        public void Append(AttributeCollectionByDBKey from)
        {
            var allAttributes = from.GetAllAttributes(Allocator.Temp);
            for (int i = 0; i < allAttributes.Length; i++)
            {
                var attribute = allAttributes[i];
                Append(attribute);
            }

            allAttributes.Dispose();
        }
        
        public void Truncate(AttributeCollectionByDBKey from)
        {
            var allAttributes = from.GetAllAttributes(Allocator.Temp);
            for (int i = 0; i < allAttributes.Length; i++)
            {
                var attribute = allAttributes[i];
                Truncate(attribute);
            }
            allAttributes.Dispose();
        }

        public void Dispose()
        {
            _attributes.Dispose();
        }
    }
}