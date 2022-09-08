using System.Collections.Generic;
using Rolemancer.AbilityTools.Attributes;
using Unity.Collections;

namespace Rolemancer.AbilityTools.Modifiers
{
    public static class ModifierProcessorsRunner
    {
        private static readonly HashSet<IModifierProcessor> Temp;

        static ModifierProcessorsRunner()
        {
            Temp = new HashSet<IModifierProcessor>();
        }

        public static void ProcessAttributes(
            Dictionary<AttributeDbKey, IModifierProcessor> processors,
            ref ProcessorArguments arguments)
        {
            var allAttributes = arguments.CurrentAttributes.GetAllAttributes(Allocator.Temp);
            // Don't change to `foreach`!
            // There is an error https://docs.unity3d.com/Packages/com.unity.collections@0.15/manual/index.html
            // (bottom of the page)
            for (var i = 0; i < allAttributes.Length; i++)
                if (processors.TryGetValue(allAttributes[i].DbKey, out var processor))
                    Temp.Add(processor);

            foreach (var processor in Temp)
                processor.Processor(ref arguments);

            Temp.Clear();
            allAttributes.Dispose();
        }
    }
}