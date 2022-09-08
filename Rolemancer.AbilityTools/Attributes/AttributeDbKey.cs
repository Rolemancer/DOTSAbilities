using System;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Attributes
{
    [BurstCompatible, BurstCompile]
    public readonly struct AttributeDbKey: IEquatable<AttributeDbKey>
    {
        public readonly int Key;

        public AttributeDbKey(int key)
        {
            Key = key;
        }

        public bool Equals(AttributeDbKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeDbKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(AttributeDbKey left, AttributeDbKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AttributeDbKey left, AttributeDbKey right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Attribute DB key {Key}";
        }
    }
}