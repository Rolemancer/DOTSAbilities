using System.Collections;
using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.DataMapping;
using Rolemancer.Abilities.Effects;
using Rolemancer.Abilities.ModifierProcessors;
using Rolemancer.Abilities.Targets;
using Rolemancer.Abilities.Tests.ConcreteModifierProcessors;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.Abilities.Tests
{
    public class CreateEffectTest : MonoBehaviour
    {
        private static AttributeDBKey _maxHpKey = new AttributeDBKey(0);
        private static AttributeDBKey _hpKey = new AttributeDBKey(1);
        private static AttributeDBKey _damageKey = new AttributeDBKey(2);
        
        private static EffectDBKey _baseMaxHpEffectKey = new EffectDBKey(0);
        private static EffectDBKey _infinityBuffMaxHpEffectKey = new EffectDBKey(1);
        private static EffectDBKey _temporaryBuffMaxHpEffectKey = new EffectDBKey(2);
        private static EffectDBKey _damageEffectKey = new EffectDBKey(3);
        
        private void Start()
        {
            StartCoroutine(TestEffects());
        }

        private IEnumerator TestEffects()
        {
            var dataMap = Mapping.Map;
            var targetId = dataMap.GetNextTargetId();
            
            BattleModifierProcessorLibrary.AddProcessor(new DamageProcessor(_damageKey, _hpKey));
            BattleModifierProcessorLibrary.AddProcessor(new RelationWithMaxLevel(_maxHpKey, _hpKey));
            
            
            CreateBaseMaxHpEffect(targetId);
            CreateMaxHpBuffEffect_Infinity(targetId);
            CreateMaxHpBuffEffect_Temporary(targetId);

            EffectProcessing.Process(Time.realtimeSinceStartup);
            
            PrintAttributes(targetId);
            yield return new WaitForSeconds(4);
            EffectProcessing.Process(Time.realtimeSinceStartup);
            PrintAttributes(targetId);
            
            CreateDamageEffect(targetId);
            EffectProcessing.Process(Time.realtimeSinceStartup);
            PrintAttributes(targetId);
        }

        private void CreateBaseMaxHpEffect(TargetId target)
        {
            var effect = Effect.InfinityEffect(_baseMaxHpEffectKey);
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = _maxHpKey, Value = 7 };
            complexKey.AttachAttributes(buffHp);
        }

        private void CreateMaxHpBuffEffect_Infinity(TargetId target)
        {
            var effect = Effect.InfinityEffect(_infinityBuffMaxHpEffectKey);
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = _maxHpKey, Value = 13 };
            complexKey.AttachAttributes(buffHp);
        }

        private void CreateMaxHpBuffEffect_Temporary(TargetId target)
        {
            var effect = Effect.LongLifeEffect(_temporaryBuffMaxHpEffectKey, 3);
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = _maxHpKey, Value = 9 };
            complexKey.AttachAttributes(buffHp);
        }
        
        private void CreateDamageEffect(TargetId target)
        {
            var effect = Effect.LongLifeEffect(_damageEffectKey, 3);
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = _damageKey, Value = 3 };
            complexKey.AttachAttributes(buffHp);
        }
        
        private void PrintAttributes(TargetId targetId)
        {
            var attributes = targetId.GetAttributes(Allocator.Temp);
            var allAttributes = attributes.GetAllAttributes(Allocator.Temp);
            for (var i = 0; i < allAttributes.Length; i++)
            {
                var attribute = allAttributes[i];
                Debug.Log($"Attr collection: Key = {attribute.DbKey}, value = {attribute.Value}");
            }

            allAttributes.Dispose();
            attributes.Dispose();
        }
    }


    //     protected override void OnCreate()
    //     {
    //         _ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    //
    //         var defaultAttributes = new AttributeCollection();
    //         defaultAttributes.Set(new Attribute(){Id = 10004, Value = 7}); // MaxHp
    //         defaultAttributes.Set(new Attribute(){Id = 10003, Value = 7}); // Hp
    //
    //         var target = EntityManager.CreateEntity();
    //         EntityManager.SetName(target, "Target");
    //         EntityManager.AddComponentData(target, new BaseAttributes
    //         {
    //             Value = defaultAttributes
    //         });
    //         
    //         CreateMaxHpEffect(target);
    //         CreateHpEffect(target);
    //         CreateIncomingDamageEffect(target);
    //         CreateBuffTemporaryEffect(target);
    //
    //         _ecbSystem.AddJobHandleForProducer(Dependency);
    //     }
    //
    //     private void CreateMaxHpEffect(Entity target)
    //     {
    //         var maxHp = new Modifier(10004, 10);
    //
    //         var immediateMaxHpEffect = EntityManager.CreateEntity();
    //         EntityManager.SetName(immediateMaxHpEffect, "Immediate Max Hp");
    //         EntityManager.AddComponentData(immediateMaxHpEffect, new CreateImmediateEffectComponent());
    //         EntityManager.AddComponentData(immediateMaxHpEffect, new ModifierComponent { Value = maxHp });
    //         EntityManager.AddComponentData(immediateMaxHpEffect, new EffectTarget { Value = target });
    //     }
    //
    //     private void CreateHpEffect(Entity target)
    //     {
    //         var hp = new Modifier(10003, 10);
    //
    //         var immediateHpEffect = EntityManager.CreateEntity();
    //         EntityManager.SetName(immediateHpEffect, "Immediate Hp");
    //         EntityManager.AddComponentData(immediateHpEffect, new CreateImmediateEffectComponent());
    //         EntityManager.AddComponentData(immediateHpEffect, new ModifierComponent { Value = hp });
    //         EntityManager.AddComponentData(immediateHpEffect, new EffectTarget { Value = target });
    //     }
    //
    //     private void CreateIncomingDamageEffect(Entity target)
    //     {
    //         var incomingDamage = new Modifier(10006, 3);
    //
    //         var immediateHpEffect = EntityManager.CreateEntity();
    //         EntityManager.SetName(immediateHpEffect, "Immediate Incoming Damage");
    //         EntityManager.AddComponentData(immediateHpEffect, new CreateImmediateEffectComponent());
    //         EntityManager.AddComponentData(immediateHpEffect, new ModifierComponent { Value = incomingDamage });
    //         EntityManager.AddComponentData(immediateHpEffect, new EffectTarget { Value = target });
    //     }
    //
    //     private void CreateBuffTemporaryEffect(Entity target)
    //     {
    //         var damageBuff = new Modifier(10005, 8);
    //
    //         var damageBuffEffect = EntityManager.CreateEntity();
    //         EntityManager.SetName(damageBuffEffect, "LongLife Damage Buff");
    //         EntityManager.AddComponentData(damageBuffEffect, new CreateLongLifeEffectComponent());
    //         EntityManager.AddComponentData(damageBuffEffect, new ModifierComponent { Value = damageBuff });
    //         EntityManager.AddComponentData(damageBuffEffect, new EffectTarget { Value = target });
    //         EntityManager.AddComponentData(damageBuffEffect, new EffectLifetime { Value = 3 });
    //     }
    //
    //     protected override void OnUpdate()
    //     {
    //     }
}