using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Modifiers
{
    public abstract class ModifierProcessorBase : IModifierProcessor
    {
        public DbKeysCollection<AttributeDbKey> TriggerVariables { get; }
        public DbKeysCollection<AttributeDbKey> AllVariables { get; }

        public ProcessAttributes Processor => ChangeAttributes;

        public ModifierProcessorBase()
        {
            TriggerVariables = new DbKeysCollection<AttributeDbKey>(Allocator.Persistent);
            AllVariables = new DbKeysCollection<AttributeDbKey>(Allocator.Persistent);
        }
        
        protected abstract void ChangeAttributes(ref ProcessorArguments arguments);
    }
}