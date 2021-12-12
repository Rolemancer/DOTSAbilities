using Rolemancer.AbilityTools.Attributes;

namespace Rolemancer.AbilityTools.Modifiers
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