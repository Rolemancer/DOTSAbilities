using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Modifiers;
using Unity.Mathematics;

namespace Rolemancer.Abilities.Tests.ConcreteModifierProcessors
{
    /// <summary>
    ///     health = max(health - [damage], 0)
    /// </summary>
    public class DamageProcessor : ModifierProcessorBase
    {
        private readonly AttributeDBKey _damageAttributeDBKey;
        private readonly AttributeDBKey _healthAttributeDBKey;

        public DamageProcessor(AttributeDBKey damageAttributeDBKey, AttributeDBKey healthAttributeDBKey)
        {
            _damageAttributeDBKey = damageAttributeDBKey;
            _healthAttributeDBKey = healthAttributeDBKey;

            TriggerVariables.Set(_damageAttributeDBKey);

            AllVariables.Set(_damageAttributeDBKey);
            AllVariables.Set(_healthAttributeDBKey);
        }

        protected override void ChangeAttributes(ref ProcessorArguments arguments)
        {
            var damageAttribute = arguments.CurrentAttributes.Get(_damageAttributeDBKey);
            var healthAttribute = arguments.CurrentAttributes.Get(_healthAttributeDBKey);
            healthAttribute.Value -= damageAttribute.Value;
            healthAttribute.Value = math.max(healthAttribute.Value, 0);
            arguments.CurrentAttributes.Set(healthAttribute);
            arguments.CurrentAttributes.Remove(_damageAttributeDBKey);
        }
    }
}