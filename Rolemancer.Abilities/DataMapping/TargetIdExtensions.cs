using Rolemancer.Abilities.Targets;

namespace Rolemancer.Abilities.DataMapping
{
    // TODO: for the future
    public static class TargetIdExtensions
    {
        public static TargetId MakeTarget<T>(this T effect)
        {
            var map = GetMap();
            return map.GetNextTargetId();
        }
        
        private static DataMap GetMap()
        {
            return Mapping.Map;
        }
    }
}