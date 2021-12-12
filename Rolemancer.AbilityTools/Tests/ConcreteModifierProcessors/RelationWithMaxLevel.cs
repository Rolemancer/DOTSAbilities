using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Modifiers;

namespace Rolemancer.AbilityTools.Tests.ConcreteModifierProcessors
{
    public class RelationWithMaxLevel : ModifierProcessorBase
    {
        private readonly AttributeDbKey _maxLevelForTarget;
        private readonly AttributeDbKey _targetKey;

        public RelationWithMaxLevel(AttributeDbKey maxLevelForTarget, AttributeDbKey targetKey)
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
                arguments.CurrentAttributes.Set(new Attribute(_targetKey) { Value = current.Value });
            }
        }
    }
}