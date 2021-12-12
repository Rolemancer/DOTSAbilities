using System;
using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Targets;
using Attribute = Rolemancer.AbilityTools.Attributes.Attribute;

namespace Rolemancer.AbilityTools.Modifiers
{
    public struct ProcessorArguments : IDisposable
    {
        public TargetId Target;
        public double ElapsedTime;

        public DataByDbKeyCollection<AttributeDbKey, Attribute> CurrentAttributes;
        public DataByDbKeyCollection<AttributeDbKey, Attribute> PrevAttributes;
        public DbKeysCollection<AttributeDbKey> Changed;

        public void Dispose()
        {
            CurrentAttributes.Dispose();
            PrevAttributes.Dispose();
        }
    }
}