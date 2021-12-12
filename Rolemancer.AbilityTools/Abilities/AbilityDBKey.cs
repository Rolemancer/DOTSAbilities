using System;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Abilities
{
    [BurstCompatible, BurstCompile]
    public readonly struct AbilityDBKey: IEquatable<AbilityDBKey>
    {
        public readonly int Key;

        public AbilityDBKey(int key)
        {
            Key = key;
        }

        public bool Equals(AbilityDBKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is AbilityDBKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(AbilityDBKey left, AbilityDBKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AbilityDBKey left, AbilityDBKey right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Attribute DB key {Key}";
        }
    }
}