using System;
using Rolemancer.AbilityTools.Targets;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Base
{
    [BurstCompatible]
    [BurstCompile]
    public readonly struct ComplexKey<TDbKey> : IEquatable<ComplexKey<TDbKey>>
        where TDbKey : struct
    {
        public readonly TargetId Target;
        public readonly TDbKey DBKey;

        public ComplexKey(TargetId target, TDbKey dbKey)
        {
            Target = target;
            DBKey = dbKey;
        }

        public bool Equals(ComplexKey<TDbKey> other)
        {
            return Target.Equals(other.Target) && DBKey.Equals(other.DBKey);
        }

        public override bool Equals(object obj)
        {
            return obj is ComplexKey<TDbKey> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Target.GetHashCode() * 397) ^ DBKey.GetHashCode();
            }
        }

        public static bool operator ==(ComplexKey<TDbKey> left, ComplexKey<TDbKey> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComplexKey<TDbKey> left, ComplexKey<TDbKey> right)
        {
            return !left.Equals(right);
        }
    }
}