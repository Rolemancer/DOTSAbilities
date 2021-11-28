using System;
using Rolemancer.Abilities.Targets;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.Attributes
{
    [BurstCompatible, BurstCompile]
    public readonly struct AttributeComplexKey : IEquatable<AttributeComplexKey>
    {
        public readonly TargetId Target;
        public readonly AttributeDBKey DBKey;

        public AttributeComplexKey(TargetId target, AttributeDBKey dbKey)
        {
            Target = target;
            DBKey = dbKey;
        }

        public bool Equals(AttributeComplexKey other)
        {
            return Target.Equals(other.Target) && DBKey.Equals(other.DBKey);
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeComplexKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Target.GetHashCode() * 397) ^ DBKey.GetHashCode();
            }
        }

        public static bool operator ==(AttributeComplexKey left, AttributeComplexKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AttributeComplexKey left, AttributeComplexKey right)
        {
            return !left.Equals(right);
        }
    }
}