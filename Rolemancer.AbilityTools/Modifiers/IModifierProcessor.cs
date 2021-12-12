using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;

namespace Rolemancer.AbilityTools.Modifiers
{
    public delegate void ProcessAttributes(ref ProcessorArguments arguments);

    public interface IModifierProcessor
    {
        DbKeysCollection<AttributeDbKey> TriggerVariables { get; }
        DbKeysCollection<AttributeDbKey> AllVariables { get; }
        ProcessAttributes Processor { get; }
    }
}