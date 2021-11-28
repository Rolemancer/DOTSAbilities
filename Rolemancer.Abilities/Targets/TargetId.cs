using System;

namespace Rolemancer.Abilities.Targets
{
    public readonly struct TargetId: IEquatable<TargetId>
    {
        public readonly int Id;

        public TargetId(int id)
        {
            Id = id;
        }

        public bool Equals(TargetId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is TargetId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(TargetId left, TargetId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TargetId left, TargetId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"TargetId = {Id}";
        }
    }
}