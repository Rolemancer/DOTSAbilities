using System;

namespace Rolemancer.Abilities.Effects
{
    public struct Effect
    {
        public readonly EffectDBKey DBKey;
        public EffectStatus Status;
        public EffectApplyMode ApplyMode;
        public double DiscardTime;
        public float LifeTime;

        private Effect(EffectDBKey dbKey, EffectApplyMode mode)
        {
            DBKey = dbKey;
            Status = EffectStatus.Creating;
            ApplyMode = mode;
            DiscardTime = 0;
            LifeTime = 0;
        }

        public static Effect ImmediateEffect(EffectDBKey dbKey)
        {
            var effect = new Effect(dbKey, EffectApplyMode.Immediate);
            return effect;
        }

        public static Effect LongLifeEffect(EffectDBKey dbKey, float lifeTime)
        {
            var effect = new Effect(dbKey, EffectApplyMode.LongLife)
            {
                LifeTime = lifeTime
            };
            return effect;
        }
        
        public static Effect InfinityEffect(EffectDBKey dbKey)
        {
            return LongLifeEffect(dbKey, Single.MaxValue);
        }
    }
}

public enum EffectApplyMode
{
    Immediate,
    LongLife
}


public enum EffectStatus
{
    Creating,
    Pending,
    Applied,
    Discarded
}