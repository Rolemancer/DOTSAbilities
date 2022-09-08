using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Unity.Collections;

namespace Rolemancer.AbilityTools.DataMapping
{
    public static class EffectExtensions
    {
        public static void AttachAttributes(this ComplexKey<EffectDBKey> effectComplexKey, Attribute attribute)
        {
            var map = Mapping.Map;
            AttachAttributes(effectComplexKey, map, attribute);
        }

        public static void AttachAttributes(this ComplexKey<EffectDBKey> effectComplexKey, DataMap map, Attribute attribute)
        {
            if (!map.EffectAsTarget.TryGet(effectComplexKey, out var effectTargetId))
            {
                effectTargetId = map.GetNextTargetId();
                map.EffectAsTarget.AddEffect(effectComplexKey, effectTargetId);
            }

            map.Attributes.Set(effectTargetId, attribute);
        }

        public static DataByDbKeyCollection<AttributeDbKey, Attribute> GetAttributes(this ComplexKey<EffectDBKey> effectComplexKey,
            AllocatorManager.AllocatorHandle handle)
        {
            var map = Mapping.Map;
            return GetAttributes(effectComplexKey, map, handle);
        }

        public static DataByDbKeyCollection<AttributeDbKey, Attribute> GetAttributes(this ComplexKey<EffectDBKey> effectComplexKey,
            DataMap map,
            AllocatorManager.AllocatorHandle handle)
        {
            if (map.EffectAsTarget.TryGet(effectComplexKey, out var effectTargetId))
                return map.Attributes.GetTargetAttributes(effectTargetId, handle);

            return default;
        }
    }
}