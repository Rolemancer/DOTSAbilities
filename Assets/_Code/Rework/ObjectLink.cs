using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;

namespace Rolemancer.Rework
{
    [BurstCompatible]
    [BurstCompile]
    public struct ObjectLink
    {
        //public IntPtr Pointer;

        //public ulong Handle; //The alternative way
        private GCHandle _handle;

        private ObjectLink(GCHandle handle)
        {
            _handle = handle;
        }

        public static ObjectLink Create<T>(T obj)
        {
            //var pointer = (IntPtr)UnsafeUtility.PinGCObjectAndGetAddress(value, out var handle); //The alternative way
            var gcHandle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var link = new ObjectLink(gcHandle);
            return link;
        }

        public T Get<T>()
        {
            //UnsafeUtility.AsRef<T>(pointer); //The alternative way
            var value = (T)_handle.Target;
            return value;
        }

        public void Free()
        {
            _handle.Free();

            //UnsafeUtility.ReleaseGCObject(keyValue.Value.Handle); //The alternative way
        }
    }

    [BurstCompatible]
    [BurstCompile]
    public struct ObjectLink<T>
        where T: class
    {
        public ObjectLink Link;

        private ObjectLink(ObjectLink link)
        {
            Link = link;
        }
        
        public static ObjectLink<T> Create(T obj)
        {
            var link = ObjectLink.Create(obj);
            var result = new ObjectLink<T>(link);
            return result;
        }

        public T Get()
        {
            var value = Link.Get<T>();
            return value;
        }

        public static implicit operator ObjectLink(ObjectLink<T> d) => d.Link;

        public void Free()
        {
            Link.Free();
        }
    }
}