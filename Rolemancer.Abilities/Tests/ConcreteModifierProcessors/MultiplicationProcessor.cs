using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Modifiers;

namespace Rolemancer.Abilities.Tests.ConcreteModifierProcessors
{
    /// <summary>
    /// result = [result] * [multiplier]
    /// </summary>
    public class MultiplicationProcessor : ModifierProcessorBase
    {
        private readonly AttributeDBKey _multiplierAttributeDBKey;
        private readonly AttributeDBKey _resultAttributeDBKey;

        public MultiplicationProcessor(AttributeDBKey multiplierAttributeDBKey, AttributeDBKey resultAttributeDBKey)
        {
            _multiplierAttributeDBKey = multiplierAttributeDBKey;
            _resultAttributeDBKey = resultAttributeDBKey;

            TriggerVariables.Set(_multiplierAttributeDBKey);
            TriggerVariables.Set(_resultAttributeDBKey);
            
            AllVariables.Set(_multiplierAttributeDBKey);
            AllVariables.Set(_resultAttributeDBKey);
        }
        
        protected override void ChangeAttributes(ref ProcessorArguments arguments)
        {
            var resultAttribute = arguments.CurrentAttributes.Get(_resultAttributeDBKey);
            var multiplierAttribute = arguments.CurrentAttributes.Get(_multiplierAttributeDBKey);
            resultAttribute.Value *= multiplierAttribute.Value;
            arguments.CurrentAttributes.Set(resultAttribute);
        }
    }
}