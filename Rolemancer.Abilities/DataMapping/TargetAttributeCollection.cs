using System;
using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Targets;
using Unity.Burst;
using Unity.Collections;
using Attribute = Rolemancer.Abilities.Attributes.Attribute;

namespace Rolemancer.Abilities.DataMapping
{
    [BurstCompile]
    [BurstCompatible]
    public struct TargetAttributeCollection : IDisposable
    {
        private NativeHashMap<AttributeComplexKey, Attributes.Attribute> _attributes;
        private NativeMultiHashMap<TargetId, AttributeComplexKey> _targetToAttributes;

        public TargetAttributeCollection(AllocatorManager.AllocatorHandle handle)
        {
            _attributes = new NativeHashMap<AttributeComplexKey, Attributes.Attribute>(10, handle);
            _targetToAttributes = new NativeMultiHashMap<TargetId, AttributeComplexKey>(10, handle);
        }

        public AttributeCollectionByDBKey GetTargetAttributes(
            TargetId targetId,
            AllocatorManager.AllocatorHandle handle)
        {
            var result = new AttributeCollectionByDBKey(handle);

            var valuesForKey = _targetToAttributes.GetValuesForKey(targetId);
            while (valuesForKey.MoveNext())
            {
                var complexKey = valuesForKey.Current;
                result.Set(_attributes[complexKey]);
            }
            valuesForKey.Dispose();

            return result;
        }

        public bool TryGet(AttributeComplexKey id, out Attributes.Attribute attribute)
        {
            return _attributes.TryGetValue(id, out attribute);
        }
        
        public bool TryGet(TargetId targetId, AttributeDBKey key, out Attributes.Attribute attribute)
        {
            var complexKey = new AttributeComplexKey(targetId, key);
            return TryGet(complexKey, out attribute);
        }

        public bool HasAttributes(TargetId id)
        {
            return _targetToAttributes.ContainsKey(id);
        }

        public bool Has(AttributeComplexKey id)
        {
            return _attributes.ContainsKey(id);
        }

        public bool Has(TargetId targetId, AttributeDBKey key)
        {
            var complexKey = new AttributeComplexKey(targetId, key);
            return Has(complexKey);
        }
        
        public bool Has(TargetId targetId, Attributes.Attribute attribute)
        {
            return Has(targetId, attribute.DbKey);
        }

        public void Set(TargetId targetId, AttributeCollectionByDBKey attributes)
        {
            foreach (var kvp in attributes)
            {
                Set(targetId, kvp.Value);
            }
        }
        
        public void Set(AttributeComplexKey key, Attributes.Attribute attribute)
        {
            _targetToAttributes.Add(key.Target, key);
            _attributes[key] = attribute;
        }

        public void Set(TargetId targetId, Attributes.Attribute attribute)
        {
            var complexKey = new AttributeComplexKey(targetId, attribute.DbKey);
            Set(complexKey, attribute);
        }

        public bool Remove(AttributeComplexKey key)
        {
            _targetToAttributes.Remove(key.Target, key);
            if (_targetToAttributes.CountValuesForKey(key.Target) == 0) 
                _targetToAttributes.Remove(key.Target);
            return _attributes.Remove(key);
        }

        public bool Remove(TargetId targetId, Attributes.Attribute attribute)
        {
            return Remove(targetId, attribute.DbKey);
        }

        public bool Remove(TargetId targetId, AttributeDBKey attributeKey)
        {
            var complexKey = new AttributeComplexKey(targetId, attributeKey);
            return Remove(complexKey);
        }

        public void Remove(TargetId targetId)
        {
            var valuesForKey = _targetToAttributes.GetValuesForKey(targetId);
            while (valuesForKey.MoveNext())
            {
                Remove(valuesForKey.Current);
            }
            valuesForKey.Dispose();
            _targetToAttributes.Remove(targetId);
        }
        
        public NativeArray<Attributes.Attribute> GetAllAttributes(AllocatorManager.AllocatorHandle handle)
        {
            return _attributes.GetValueArray(handle);
        }

        public void ReplaceTargetAttributes(TargetId targetId, AttributeCollectionByDBKey attributes)
        {
            Remove(targetId);
            var temp = attributes.GetAllAttributes(Allocator.Temp);
            for (int i = 0; i < temp.Length; i++)
            {
                var attribute = temp[i];
                Set(targetId, attribute);
            }
            temp.Dispose();
        }

        public void ReplaceExistingAttributes(TargetAttributeCollection from)
        {
            // Don't change to `foreach`!
            // There is an error https://docs.unity3d.com/Packages/com.unity.collections@0.15/manual/index.html
            // (bottom of the page)
            var keys = _attributes.GetKeyArray(Allocator.Temp);
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                Set(key, from._attributes[key]);
            }

            keys.Dispose();
        }

        public void Dispose()
        {
            _attributes.Dispose();
            _targetToAttributes.Dispose();
        }
    }
}