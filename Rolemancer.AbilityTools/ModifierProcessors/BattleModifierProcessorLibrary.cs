using System.Collections.Generic;
using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.Modifiers;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.AbilityTools.ModifierProcessors
{
    public static class BattleModifierProcessorLibrary
    {
        private static  HashSet<IModifierProcessor> _processors;
        public static  Dictionary<AttributeDbKey, IModifierProcessor> ProcessorFunctions;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _processors = new HashSet<IModifierProcessor>();
            ProcessorFunctions = new Dictionary<AttributeDbKey, IModifierProcessor>();
            FulfillProcessors();
            ExtractProcessorFunctions();
        }

        public static void AddProcessor(IModifierProcessor processor)
        {
            _processors.Add(processor);
            ExtractProcessorFunction(processor);
        }
        
        private static void FulfillProcessors()
        {
           // _processors.Add(new DamageProcessor(new AttributeDBKey(3), new AttributeDBKey(1)));
            //_processors.Add(new MultiplicationProcessor(10005, 10002));
        }

        private static void ExtractProcessorFunctions()
        {
            foreach (var processor in _processors)
            {
                ExtractProcessorFunction(processor);
            }
        }

        private static void ExtractProcessorFunction(IModifierProcessor processor)
        {
            var dbKeysCollection = processor.TriggerVariables;
            var keysArray = dbKeysCollection.GetAll(Allocator.TempJob);
            for (int i = 0; i < keysArray.Length; i++)
            {
                var dbKey = keysArray[i];
                ProcessorFunctions[dbKey] = processor;
            }

            keysArray.Dispose();
        }
    }
}