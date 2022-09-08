using System;
using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Targets;
using Unity.Burst;
using Unity.Collections;
using Attribute = Rolemancer.AbilityTools.Attributes.Attribute;

namespace Rolemancer.AbilityTools.DataMapping
{
    [BurstCompile]
    [BurstCompatible]
    public struct TargetAttributeCollection : IDisposable
    {
        private NativeParallelHashMap<ComplexKey<AttributeDbKey>, Attributes.Attribute> _attributes;
        private NativeParallelMultiHashMap<TargetId, ComplexKey<AttributeDbKey>> _targetToAttributes;

        public TargetAttributeCollection(AllocatorManager.AllocatorHandle handle)
        {
            _attributes = new NativeParallelHashMap<ComplexKey<AttributeDbKey>, Attributes.Attribute>(10, handle);
            _targetToAttributes = new NativeParallelMultiHashMap<TargetId, ComplexKey<AttributeDbKey>>(10, handle);
        }

        public DataByDbKeyCollection<AttributeDbKey, Attribute> GetTargetAttributes(
            TargetId targetId,
            AllocatorManager.AllocatorHandle handle)
        {
            var result = new DataByDbKeyCollection<AttributeDbKey, Attribute>(handle);

            var valuesForKey = _targetToAttributes.GetValuesForKey(targetId);
            while (valuesForKey.MoveNext())
            {
                var complexKey = valuesForKey.Current;
                result.Set(_attributes[complexKey]);
            }
            valuesForKey.Dispose();

            return result;
        }

        public bool TryGet(ComplexKey<AttributeDbKey> id, out Attributes.Attribute attribute)
        {
            return _attributes.TryGetValue(id, out attribute);
        }
        
        public bool TryGet(TargetId targetId, AttributeDbKey key, out Attributes.Attribute attribute)
        {
            var complexKey = new ComplexKey<AttributeDbKey>(targetId, key);
            return TryGet(complexKey, out attribute);
        }

        public bool HasAttributes(TargetId id)
        {
            return _targetToAttributes.ContainsKey(id);
        }

        public bool Has(ComplexKey<AttributeDbKey> id)
        {
            return _attributes.ContainsKey(id);
        }

        public bool Has(TargetId targetId, AttributeDbKey key)
        {
            var complexKey = new ComplexKey<AttributeDbKey>(targetId, key);
            return Has(complexKey);
        }
        
        public bool Has(TargetId targetId, Attributes.Attribute attribute)
        {
            return Has(targetId, attribute.DbKey);
        }

        public void Set(TargetId targetId, DataByDbKeyCollection<AttributeDbKey, Attribute> attributes)
        {
            foreach (var kvp in attributes)
            {
                Set(targetId, kvp.Value);
            }
        }
        
        public void Set(ComplexKey<AttributeDbKey> key, Attributes.Attribute attribute)
        {
            _targetToAttributes.Add(key.Target, key);
            _attributes[key] = attribute;
        }

        public void Set(TargetId targetId, Attributes.Attribute attribute)
        {
            var complexKey = new ComplexKey<AttributeDbKey>(targetId, attribute.DbKey);
            Set(complexKey, attribute);
        }

        public bool Remove(ComplexKey<AttributeDbKey> key)
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

        public bool Remove(TargetId targetId, AttributeDbKey attributeKey)
        {
            var complexKey = new ComplexKey<AttributeDbKey>(targetId, attributeKey);
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

        public void ReplaceTargetAttributes(TargetId targetId, DataByDbKeyCollection<AttributeDbKey, Attribute> attributes)
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