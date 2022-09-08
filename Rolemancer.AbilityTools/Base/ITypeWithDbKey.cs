using System;

namespace Rolemancer.AbilityTools.Base
{
    public interface ITypeWithDbKey<out TDbKey>
        where TDbKey: unmanaged, IEquatable<TDbKey>
    {
        TDbKey DbKey { get; }
    }
}