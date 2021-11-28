using Rolemancer.Abilities.Attributes;

namespace Rolemancer.Abilities.Modifiers
{
    public delegate void ProcessAttributes(ref ProcessorArguments arguments);

    public interface IModifierProcessor
    {
        AttributeDBKeysCollection TriggerVariables { get; }
        AttributeDBKeysCollection AllVariables { get; }
        ProcessAttributes Processor { get; }
    }
}