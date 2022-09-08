using Rolemancer.AbilityTools.Attributes;
using Rolemancer.AbilityTools.DataMapping;
using Rolemancer.AbilityTools.Effects;
using Rolemancer.AbilityTools.Targets;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rolemancer.AbilityTools.Tests
{
    public class WorkingCreateAttributesTest : MonoBehaviour
    {
        private void Start()
        {
            TestAttributes();
        }

        private void TestAttributes()
        {
            var dataMap = Mapping.Map;
            var targetId = dataMap.GetNextTargetId();
            var hpKey = new AttributeDbKey(0);
            var maxHpKey = new AttributeDbKey(1);
            var hp = new Attribute(hpKey) { Value = 7 };
            var maxHp = new Attribute(maxHpKey) { Value = 7 };

            targetId.AddAttribute(hp);
            targetId.AddAttribute(maxHp);

            Debug.Log("Has Hp: " + targetId.HasAttribute(hp));
            Debug.Log("Has Max Hp: " + targetId.HasAttribute(maxHp.DbKey));

            targetId.TryGetAttribute(hpKey, out var hpAttribute);
            Debug.Log("Get Hp: " + hpAttribute.Value);

            targetId.AttachEffect(new Effect());
            PrintAttributes(targetId);
        }

        private void PrintAttributes(TargetId targetId)
        {
            var job = new PrintJob { TargetId = targetId };
            var x = job.Schedule();
            x.Complete();
        }

        private struct PrintJob : IJob
        {
            public TargetId TargetId;

            public void Execute()
            {
                var attributes = TargetId.GetAttributes(Allocator.Temp);
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
    }
}