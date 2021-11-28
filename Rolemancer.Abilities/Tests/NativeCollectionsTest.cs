using System;
using Unity.Collections;
using UnityEngine;

namespace Rolemancer.Abilities.Tests
{
    public class NativeCollectionsTest : MonoBehaviour
    {
        private First _f;

        private void Start()
        {
            _f = new First();
            Debug.Log("1. Count = " + _f.Data.Length);
            _f.Add();
            Debug.Log("2. Count = " + _f.Data.Length);
            var x = _f.Data;
            x.Add("Bless");
            Debug.Log("3. Count = " + _f.Data.Length);
            Debug.Log("3. Count New = " + x.Length);
        }
    }
    
    public class First
    {
        public NativeList<FixedString64Bytes> Data;

        public First()
        {
            Data = new NativeList<FixedString64Bytes>(1, Allocator.Persistent);
        }

        public void Add()
        {
            Data.Add("Hello");
        }
    }
}