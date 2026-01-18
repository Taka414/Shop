//
// (C) 2022 Takap.
//

using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Queue{T}"/> の拡張機能を定義します。
    /// </summary>
    public static class QueueExtension
    {
        public static void EnqueueRange<T>(this Queue<T> self, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                self.Enqueue(item);
            }
        }
    }
}