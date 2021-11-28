using System.Collections.Generic;
using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.Modifiers;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.Abilities.Tests.ConcreteModifierProcessors
{
    public static class BattleModifierProcessorLibrary
    {
        private static  List<IModifierProcessor> _processors;
        public static  Dictionary<AttributeDBKey, IModifierProcessor> ProcessorFunctions;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _processors = new List<IModifierProcessor>();
            ProcessorFunctions = new Dictionary<AttributeDBKey, IModifierProcessor>();
            FulfillProcessors();
            ExtractProcessorFunctions();
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
}