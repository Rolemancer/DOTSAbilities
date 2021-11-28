using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.Attributes
{
    [BurstCompatible]
    [BurstCompile]
    public struct Attribute
    {
        public AttributeDBKey DbKey;
        public float Value;
        public override string ToString()
        {
            return string.Format("Id={0}; Value={1}", DbKey, Value);
        }
    }
}