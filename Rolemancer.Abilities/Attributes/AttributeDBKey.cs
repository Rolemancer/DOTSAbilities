using System;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Abilities.Attributes
{
    [BurstCompatible, BurstCompile]
    public readonly struct AttributeDBKey: IEquatable<AttributeDBKey>
    {
        public readonly int Key;

        public AttributeDBKey(int key)
        {
            Key = key;
        }

        public bool Equals(AttributeDBKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeDBKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(AttributeDBKey left, AttributeDBKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AttributeDBKey left, AttributeDBKey right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Attribute DB key {Key}";
        }
    }
}