// 
// (C) 2023 Takap.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Takap.Utility
{
    internal static class InternalListExtensions
    {
        /// <summary>
        /// List をハックして Span を取り出す
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Span<T> AsSpan<T>(this List<T> self)
        {
#if NET5_0_OR_GREATER
            return System.Runtime.InteropServices.CollectionsMarshal.AsSpan(self);
#else
            // Hacked!!!!
            return Unsafe.As<ListDummy<T>>(self).Items.AsSpan(0, self.Count);
#endif
        }

        private class ListDummy<T> { internal T[] Items; }
    }
}
