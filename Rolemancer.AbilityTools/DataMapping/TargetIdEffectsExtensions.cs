using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Rolemancer.AbilityTools.Targets;
using Unity.Collections;

namespace Rolemancer.AbilityTools.DataMapping
{
    public static class TargetIdEffectsExtensions
    {
        public static ComplexKey<EffectDBKey> AttachEffect(this TargetId targetId, Effect effect)
        {
            var map = GetMap();
            return AttachEffect(targetId, effect, map);
        }
        public static ComplexKey<EffectDBKey> AttachEffect(this TargetId targetId, Effect effect, DataMap map)
        {
            var key = new ComplexKey<EffectDBKey>(targetId, effect.DbKey);
            map.Effects.AddEffect(targetId, effect);
            return key;
        }

        public static bool HasEffect(this TargetId targetId, Effect effect)
        {
            var map = GetMap();
            return HasEffect(targetId, effect, map);
        }
        
        public static bool HasEffect(this TargetId targetId, Effect effect, DataMap map)
        {
            return map.Effects.HasEffect(targetId, effect);
        }

        public static bool HasEffect(this TargetId targetId, EffectDBKey effectDataBaseKey)
        {
            var map = GetMap();
            return HasEffect(targetId, effectDataBaseKey, map);
        }
        
        public static bool HasEffect(this TargetId targetId, EffectDBKey effectDataBaseKey, DataMap map)
        {
            return map.Effects.HasEffect(targetId, effectDataBaseKey);
        }

        public static NativeHashMap<ComplexKey<EffectDBKey>,Effect> GetEffects(this TargetId targetId,
            AllocatorManager.AllocatorHandle handle)
        {
            var map = GetMap();
            return GetEffects(targetId, map, handle);
        }
        
        public static NativeHashMap<ComplexKey<EffectDBKey>,Effect> GetEffects(this TargetId targetId, DataMap map,
            AllocatorManager.AllocatorHandle handle)
        {
            return map.Effects.GetTargetEffects(targetId, handle);
        }

        public static bool TryGetEffect(this TargetId targetId,
            EffectDBKey effectDataBaseKey,
            out Effect effect)
        {
            var map = GetMap();
            return TryGetEffect(targetId, effectDataBaseKey, map, out effect);
        }
        
        public static bool TryGetEffect(this TargetId targetId,
            EffectDBKey effectDataBaseKey, 
            DataMap map,
            out Effect effect)
        {
            return map.Effects.TryGetEffect(targetId, effectDataBaseKey, out effect);
        }

        public static void RemoveEffect(this TargetId targetId, Effect effect)
        {
            var map = GetMap();
            RemoveEffect(targetId, effect.DbKey, map);
        }
        
        public static void RemoveEffect(this TargetId targetId, Effect effect, DataMap map)
        {
            RemoveEffect(targetId, effect.DbKey, map);
        }

        public static void RemoveEffect(this TargetId targetId, EffectDBKey effectDataBaseKey)
        {
            var map = GetMap();
            RemoveEffect(targetId, effectDataBaseKey, map);
        }
        
        public static void RemoveEffect(this TargetId targetId, EffectDBKey effectDataBaseKey, DataMap map)
        {
            map.Effects.RemoveEffect(targetId, effectDataBaseKey);
        }

        private static DataMap GetMap()
        {
            return Mapping.Map;
        }
    }
}