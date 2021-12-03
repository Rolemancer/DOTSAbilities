using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Modifiers;

namespace Rolemancer.Abilities.Tests.ConcreteModifierProcessors
{
    public class RelationWithMaxLevel : ModifierProcessorBase
    {
        private readonly AttributeDBKey _maxLevelForTarget;
        private readonly AttributeDBKey _targetKey;

        public RelationWithMaxLevel(AttributeDBKey maxLevelForTarget, AttributeDBKey targetKey)
        {
            _maxLevelForTarget = maxLevelForTarget;
            _targetKey = targetKey;

            TriggerVariables.Set(_maxLevelForTarget);

            AllVariables.Set(_maxLevelForTarget);
            AllVariables.Set(_targetKey);
        }

        protected override void ChangeAttributes(ref ProcessorArguments arguments)
        {
            var current = arguments.CurrentAttributes[_maxLevelForTarget];

            if (arguments.CurrentAttributes.TryGet(_targetKey, out var currentTargetAttribute))
            {
                if (!arguments.PrevAttributes.TryGet(_maxLevelForTarget, out var prev))
                    return;
                var changeModifier = current.Value / prev.Value;
                currentTargetAttribute.Value = currentTargetAttribute.Value * changeModifier;
                arguments.CurrentAttributes.Set(currentTargetAttribute);
            }
            else
            {
                arguments.CurrentAttributes.Set(new Attribute { DbKey = _targetKey, Value = current.Value });
            }
        }
    }
}