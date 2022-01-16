using Rolemancer.AbilityTools.Base;
using Rolemancer.AbilityTools.Effects;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Abilities
{
    [BurstCompatible]
    [BurstCompile]
    public struct Ability : ITypeWithDbKey<AbilityDBKey>
    {
        public bool Enabled;
        public float Cooldown;
        public float CooldownFinishTime;
        public EffectDBKey EffectToApply;
        
        public AbilityDBKey DbKey { get; }

        public Ability(AbilityDBKey key, EffectDBKey effect)
        {
            DbKey = key;
            Enabled = true;
            Cooldown = default;
            CooldownFinishTime = default;
            EffectToApply = effect;
        }

        public void EnableCooldown(float currentTime)
        {
            CooldownFinishTime = currentTime + Cooldown;
            if (currentTime < CooldownFinishTime)
                Enabled = false;
        }
        
        public void EnableCustomCooldown(float currentTime, float finishTime)
        {
            CooldownFinishTime = finishTime;
            
        }
    }

    [BurstCompatible]
    [BurstCompile]
    public struct AbilityInstance
    {
    }
}