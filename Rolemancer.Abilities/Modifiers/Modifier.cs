using Rolemancer.Abilities.Attributes;

namespace Rolemancer.Abilities.Modifiers
{
    public readonly struct Modifier
    {
        public readonly Attribute Value;

        public Modifier(Attribute attribute)
        {
            Value = attribute;
        }
    }
}