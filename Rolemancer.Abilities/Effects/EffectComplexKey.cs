using System;
using Rolemancer.Abilities.Targets;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.Effects
{
    [BurstCompatible]
    [BurstCompile]
    public readonly struct EffectComplexKey : IEquatable<EffectComplexKey>
    {
        public readonly TargetId Target;
        public readonly EffectDBKey DBKey;

        public EffectComplexKey(TargetId target, EffectDBKey dbKey)
        {
            Target = target;
            DBKey = dbKey;
        }

        public bool Equals(EffectComplexKey other)
        {
            return Target.Equals(other.Target) && DBKey.Equals(other.DBKey);
        }

        public override bool Equals(object obj)
        {
            return obj is EffectComplexKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Target.GetHashCode() * 397) ^ DBKey.GetHashCode();
            }
        }

        public static bool operator ==(EffectComplexKey left, EffectComplexKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EffectComplexKey left, EffectComplexKey right)
        {
            return !left.Equals(right);
        }
    }
}