using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Abilities
{
    [BurstCompatible]
    [BurstCompile]
    public struct Ability
    {
        public AbilityDBKey DbKey;
    }
}