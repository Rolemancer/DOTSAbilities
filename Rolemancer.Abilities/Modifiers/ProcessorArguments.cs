using System;
using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Targets;

namespace Rolemancer.Abilities.Modifiers
{
    public struct ProcessorArguments : IDisposable
    {
        public TargetId Target;
        public double ElapsedTime;

        public AttributeCollectionByDBKey CurrentAttributes;
        public AttributeCollectionByDBKey PrevAttributes;
        public AttributeDBKeysCollection Changed;

        public void Dispose()
        {
            CurrentAttributes.Dispose();
            PrevAttributes.Dispose();
        }
    }
}