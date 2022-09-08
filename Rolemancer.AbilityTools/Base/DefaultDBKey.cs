using System;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Base
{
    [BurstCompatible]
    [BurstCompile]
    public readonly struct DefaultDBKey : IEquatable<DefaultDBKey>
    {
        public readonly int Key;

        public DefaultDBKey(int key)
        {
            Key = key;
        }

        public bool Equals(DefaultDBKey other)
        {
            return Key == other.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is DefaultDBKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(DefaultDBKey left, DefaultDBKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DefaultDBKey left, DefaultDBKey right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"DB key {Key}";
        }
    }
}