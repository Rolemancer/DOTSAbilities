using System.Collections;
using Rolemancer.Abilities.Attributes;
using Rolemancer.Abilities.DataMapping;
using Rolemancer.Abilities.Effects;
using Rolemancer.Abilities.Targets;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.Abilities.Tests
{
    public class CreateEffectTest : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(TestEffects());
        }

        private IEnumerator TestEffects()
        {
            var dataMap = Mapping.Map;
            var targetId = dataMap.GetNextTargetId();

            CreateBaseMaxHpEffect(targetId);
            CreateMaxHpBuffEffect_Infinity(targetId);
            CreateMaxHpBuffEffect_Temporary(targetId);

            EffectProcessing.Process(Time.realtimeSinceStartup);
            
            PrintAttributes(targetId);
            yield return new WaitForSeconds(4);
            EffectProcessing.Process(Time.realtimeSinceStartup);
            PrintAttributes(targetId);
        }

        private void CreateBaseMaxHpEffect(TargetId target)
        {
            var effect = Effect.InfinityEffect(new EffectDBKey(0));
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = new AttributeDBKey(0), Value = 7 };
            complexKey.AttachAttributes(buffHp);
        }

        private void CreateMaxHpBuffEffect_Infinity(TargetId target)
        {
            var effect = Effect.InfinityEffect(new EffectDBKey(1));
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = new AttributeDBKey(0), Value = 13 };
            complexKey.AttachAttributes(buffHp);
        }

        private void CreateMaxHpBuffEffect_Temporary(TargetId target)
        {
            var effect = Effect.LongLifeEffect(new EffectDBKey(2), 3);
            var complexKey = target.AttachEffect(effect);

            var buffHp = new Attribute { DbKey = new AttributeDBKey(0), Value = 9 };
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