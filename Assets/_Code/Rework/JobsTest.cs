using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rolemancer.Rework
{
    public class TestMapHolder
    {
        private static TestMapHolder _instance;
        public TestMap Map;

        private TestMapHolder()
        {
            Map = new TestMap(Allocator.Persistent);
        }

        public static TestMapHolder Get
        {
            get
            {
                if (_instance == null)
                    _instance = new TestMapHolder();
                return _instance;
            }
        }
    }

    [BurstCompile]
    [BurstCompatible]
    public struct TestMap
    {
        public NativeParallelMultiHashMap<int, float> TargetToAttributes;
        public NativeList<int> Ids;

        public TestMap(AllocatorManager.AllocatorHandle handle)
        {
            Ids = new NativeList<int>(10, handle);
            TargetToAttributes = new NativeParallelMultiHashMap<int, float>(10, handle);
        }
    }

    public class JobsTest : MonoBehaviour
    {

        public void Start()
        {
            var map = TestMapHolder.Get.Map;
            map.Ids.Add(1);
            map.Ids.Add(2);

            map.TargetToAttributes.Add(1, 10);
            map.TargetToAttributes.Add(1, 11);
            map.TargetToAttributes.Add(2, 20);

            var job = new PendingJob { Map = map };
            var handle = job.Schedule();
            handle.Complete();
        }

        private struct PendingJob : IJob
        {
            public TestMap Map;

            public void Execute()
            {
                for (var i = 0; i < Map.Ids.Length; i++)
                {
                    var id = Map.Ids[i];

                    var x = new NativeList<float>(10, Allocator.Temp);

                    var temp = Map.TargetToAttributes.GetValuesForKey(id);
                    while (temp.MoveNext())
                    {
                        var value = temp.Current;
                        x.Add(value);
                        Debug.Log("Key = " + id + "; Value = " + value);
                    }

                    temp.Dispose();
                }
            }
        }
    }
}