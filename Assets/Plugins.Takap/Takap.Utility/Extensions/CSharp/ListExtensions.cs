// 
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="IList{T}"/> の機能を拡張します。
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// リストから指定したN個分の要素を重複せずに取り出します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRandom<T>(this IList<T> list, int count)
        {
            return ListUtil.GetRandom(list, count);
        }

        /// <summary>
        /// リストの中身を順不同に全て取り出します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRandom<T>(this IList<T> list)
        {
            return ListUtil.GetRandom(list, list.Count);
        }

        /// <summary>
        /// リストの中から適当に要素を一つ取り出します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickupOne<T>(this IList<T> src) => ListUtil.PickupOne(src);

        /// <summary>
        /// 追加した要素を戻り値として受け取ります。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Append<T>(this IList<T> src, T item)
        {
            src.Add(item);
            return item;
        }

        /// <summary>
        /// foreach を内部的に forで実行して少しだけ高速に実行します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEachFast<T>(this IList<T> self, Action<T> f)
        {
            int max = self.Count;
            for (int i = 0; i < max; i++)
            {
                f(self[i]);
            }
        }

        /// <summary>
        /// Find の Try ～ Parse パターン版
        /// </summary>
        public static bool TryFind<T>(this IList<T> self, Predicate<T> match, out T result)
        {
            int max = self.Count;
            for (int i = 0; i < max; i++)
            {
                T item = self[i];
                if (match(item))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }

        /// <summary>
        /// リストに存在しない場合だけ追加します。
        /// </summary>
        public static bool AddUnique<T>(this IList<T> self, T item)
        {
            if (self.Contains(item))
            {
                return false;
            }
            self.Add(item);
            return true;
        }
    }
}
