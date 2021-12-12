using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Rolemancer.AbilityTools.Targets;

namespace Rolemancer.AbilityTools.DataMapping
{
    public static class EffectsExtensions
    {
        public static void AttachEffect(this Effect effect, TargetId targetId)
        {
            var map = Mapping.Map;
            AttachEffect(effect, targetId, map);
        }
        
        public static void AttachEffect(this Effect effect, TargetId targetId, DataMap map)
        {
            map.Effects.AddEffect(targetId, effect);
        }
        
        public static void DiscardEffect(this ComplexKey<EffectDBKey> key, DataMap map)
        {
            map.Effects.DiscardEffect(key);
        }
        
        public static void DestroyEffect(this ComplexKey<EffectDBKey> effectKey, DataMap map)
        {
            map.Effects.RemoveEffect(effectKey);
            map.EffectAsTarget.Remove(effectKey);
        }
    }
}