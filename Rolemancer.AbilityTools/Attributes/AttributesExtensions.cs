using Rolemancer.AbilityTools.Base;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Attributes
{
    public static class AttributesExtensions
    {
        public static void Append(this DataByDbKeyCollection<AttributeDbKey, Attribute> collection,
            Attribute attribute)
        {
            var dbKey = attribute.DbKey;
            if (collection.Has(dbKey))
            {
                var attr = collection.Get(dbKey);
                attr.Value += attribute.Value;
                collection.Set(attr);
            }
            else
            {
                collection.Set(attribute);
            }
        }

        public static void Truncate(this DataByDbKeyCollection<AttributeDbKey, Attribute> collection,
            Attribute attribute)
        {
            var dbKey = attribute.DbKey;
            if (collection.Has(dbKey))
            {
                var attr = collection.Get(dbKey);
                attr.Value -= attribute.Value;
                collection.Set(attr);
            }
            else
            {
                collection.Set(new Attribute(dbKey) { Value = -attribute.Value });
            }
        }


        public static void Append(this DataByDbKeyCollection<AttributeDbKey, Attribute> target,
            DataByDbKeyCollection<AttributeDbKey, Attribute> collectionFrom)
        {
            var allAttributes = collectionFrom.GetAllAttributes(Allocator.Temp);
            for (var i = 0; i < allAttributes.Length; i++)
            {
                var attribute = allAttributes[i];
                target.Append(attribute);
            }

            allAttributes.Dispose();
        }

        public static void Truncate(this DataByDbKeyCollection<AttributeDbKey, Attribute> target,
            DataByDbKeyCollection<AttributeDbKey, Attribute> collectionFrom)
        {
            var allAttributes = collectionFrom.GetAllAttributes(Allocator.Temp);
            for (var i = 0; i < allAttributes.Length; i++)
            {
                var attribute = allAttributes[i];
                target.Truncate(attribute);
            }

            allAttributes.Dispose();
        }

        public static void Set(this DbKeysCollection<AttributeDbKey> target, Attribute attribute)
        {
            var id = attribute.DbKey;
            target.Set(id);
        }

        public static void Append(this DbKeysCollection<AttributeDbKey> target,
            DataByDbKeyCollection<AttributeDbKey, Attribute> attributes)
        {
            var temp = attributes.GetAllAttributes(Allocator.Temp);
            for (var i = 0; i < temp.Length; i++) target.Set(temp[i]);
            temp.Dispose();
        }
    }
}