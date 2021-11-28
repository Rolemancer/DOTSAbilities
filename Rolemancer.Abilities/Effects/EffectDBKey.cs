using System;

namespace Rolemancer.Abilities.Effects
{
    public readonly struct EffectDBKey: IEquatable<EffectDBKey>
    {
        public readonly int Id;

        public EffectDBKey(int id)
        {
            Id = id;
        }

        public bool Equals(EffectDBKey other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is EffectDBKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(EffectDBKey left, EffectDBKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EffectDBKey left, EffectDBKey right)
        {
            return !left.Equals(right);
        }
    }
}