﻿using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Targets;
using Unity.Collections;

namespace Rolemancer.AbilityTools.DataMapping
{
    public static class TargetIdAttributesExtensions
    {
        public static void AddAttribute(this TargetId targetId, Attribute attribute)
        {
            var map = GetMap();
            AddAttribute(targetId, attribute, map);
        }

        public static void AddAttribute(this TargetId targetId, Attribute attribute, DataMap map)
        {
            map.Attributes.Set(targetId, attribute);
        }

        public static bool HasAttribute(this TargetId targetId, Attribute attribute)
        {
            var map = GetMap();
            return HasAttribute(targetId, attribute, map);
        }

        public static bool HasAttribute(this TargetId targetId, Attribute attribute, DataMap map)
        {
            return map.Attributes.Has(targetId, attribute);
        }

        public static bool HasAttribute(this TargetId targetId, AttributeDbKey attributeDBKey)
        {
            var map = GetMap();
            return HasAttribute(targetId, attributeDBKey, map);
        }

        public static bool HasAttribute(this TargetId targetId, AttributeDbKey attributeDBKey, DataMap map)
        {
            return map.Attributes.Has(targetId, attributeDBKey);
        }

        public static DataByDbKeyCollection<AttributeDbKey, Attribute> GetAttributes(this TargetId targetId,
            AllocatorManager.AllocatorHandle handle)
        {
            var map = GetMap();
            return GetAttributes(targetId, map, handle);
        }

        public static DataByDbKeyCollection<AttributeDbKey, Attribute> GetAttributes(this TargetId targetId, DataMap map,
            AllocatorManager.AllocatorHandle handle)
        {
            return map.Attributes.GetTargetAttributes(targetId, handle);
        }

        public static bool TryGetAttribute(this TargetId targetId,
            AttributeDbKey attributeDBKey,
            out Attribute attribute)
        {
            var map = GetMap();
            return TryGetAttribute(targetId, attributeDBKey, map, out attribute);
        }

        public static bool TryGetAttribute(this TargetId targetId,
            AttributeDbKey attributeDBKey,
            DataMap map, out Attribute attribute)
        {
            return map.Attributes.TryGet(targetId, attributeDBKey, out attribute);
        }

        public static void RemoveAttribute(this TargetId targetId, Attribute attribute)
        {
            var map = GetMap();
            RemoveAttribute(targetId, attribute, map);
        }

        public static void RemoveAttribute(this TargetId targetId, Attribute attribute, DataMap map)
        {
            map.Attributes.Remove(targetId, attribute);
        }

        public static void RemoveAttribute(this TargetId targetId, AttributeDbKey attributeDBKey)
        {
            var map = GetMap();
            RemoveAttribute(targetId, attributeDBKey, map);
        }

        public static void RemoveAttribute(this TargetId targetId, AttributeDbKey attributeDBKey, DataMap map)
        {
            map.Attributes.Remove(targetId, attributeDBKey);
        }

        private static DataMap GetMap()
        {
            return Mapping.Map;
        }
    }
}