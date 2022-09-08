using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Modifiers;

namespace Rolemancer.AbilityTools.Tests.ConcreteModifierProcessors
{
    /// <summary>
    /// result = [result] * [multiplier]
    /// </summary>
    public class MultiplicationProcessor : ModifierProcessorBase
    {
        private readonly AttributeDbKey _multiplierAttributeDBKey;
        private readonly AttributeDbKey _resultAttributeDBKey;

        public MultiplicationProcessor(AttributeDbKey multiplierAttributeDBKey, AttributeDbKey resultAttributeDBKey)
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