using Rolemancer.Abilities.Attributes;
using Unity.Collections;

namespace Rolemancer.Abilities.Modifiers
{
    public abstract class ModifierProcessorBase : IModifierProcessor
    {
        public AttributeDBKeysCollection TriggerVariables { get; }
        public AttributeDBKeysCollection AllVariables { get; }

        public ProcessAttributes Processor => ChangeAttributes;

        public ModifierProcessorBase()
        {
            TriggerVariables = new AttributeDBKeysCollection(Allocator.Persistent);
            AllVariables = new AttributeDBKeysCollection(Allocator.Persistent);
        }
        
        protected abstract void ChangeAttributes(ref ProcessorArguments arguments);
    }
}