using Rolemancer.AbilityTools.Targets;

namespace Rolemancer.AbilityTools.DataMapping
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