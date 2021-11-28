using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Effects;
using Unity.Collections;

namespace Rolemancer.Abilities.DataMapping
{
    public static class EffectExtensions
    {
        public static void AttachAttributes(this EffectComplexKey effectComplexKey, Attribute attribute)
        {
            var map = Mapping.Map;
            AttachAttributes(effectComplexKey, map, attribute);
        }

        public static void AttachAttributes(this EffectComplexKey effectComplexKey, DataMap map, Attribute attribute)
        {
            if (!map.EffectAsTarget.TryGet(effectComplexKey, out var effectTargetId))
            {
                effectTargetId = map.GetNextTargetId();
                map.EffectAsTarget.AddEffect(effectComplexKey, effectTargetId);
            }

            map.Attributes.Set(effectTargetId, attribute);
        }

        public static AttributeCollectionByDBKey GetAttributes(this EffectComplexKey effectComplexKey,
            AllocatorManager.AllocatorHandle handle)
        {
            var map = Mapping.Map;
            return GetAttributes(effectComplexKey, map, handle);
        }

        public static AttributeCollectionByDBKey GetAttributes(this EffectComplexKey effectComplexKey,
            DataMap map,
            AllocatorManager.AllocatorHandle handle)
        {
            if (map.EffectAsTarget.TryGet(effectComplexKey, out var effectTargetId))
                return map.Attributes.GetTargetAttributes(effectTargetId, handle);

            return default;
        }
    }
}