using Rolemancer.Abilities.Effects;
using Rolemancer.Abilities.Targets;

namespace Rolemancer.Abilities.DataMapping
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
        
        public static void DiscardEffect(this EffectComplexKey key, DataMap map)
        {
            map.Effects.DiscardEffect(key);
        }
        
        public static void DestroyEffect(this EffectComplexKey effectKey, DataMap map)
        {
            map.Effects.RemoveEffect(effectKey);
            map.EffectAsTarget.Remove(effectKey);
        }
    }
}