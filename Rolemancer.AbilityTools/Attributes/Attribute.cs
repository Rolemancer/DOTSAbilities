using Rolemancer.AbilityTools.Base;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Attributes
{
    [BurstCompatible]
    [BurstCompile]
    public struct Attribute : ITypeWithDbKey<AttributeDbKey>
    {
        public readonly AttributeDbKey DbKey { get; }
        public float Value;

        public Attribute(AttributeDbKey dbKey)
        {
            DbKey = dbKey;
            Value = default;
        }
        
        public override string ToString()
        {
            return string.Format("Id={0}; Value={1}", DbKey, Value);
        }
    }
}