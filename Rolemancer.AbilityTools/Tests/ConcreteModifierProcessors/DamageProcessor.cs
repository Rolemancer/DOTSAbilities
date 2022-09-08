using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Modifiers;
using Unity.Mathematics;

namespace Rolemancer.AbilityTools.Tests.ConcreteModifierProcessors
{
    /// <summary>
    ///     health = max(health - [damage], 0)
    /// </summary>
    public class DamageProcessor : ModifierProcessorBase
    {
        private readonly AttributeDbKey _damageAttributeDBKey;
        private readonly AttributeDbKey _healthAttributeDBKey;

        public DamageProcessor(AttributeDbKey damageAttributeDBKey, AttributeDbKey healthAttributeDBKey)
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