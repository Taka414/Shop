//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> を拡張します。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// foreach を実行します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> f)
        {
            foreach (T item in self)
            {
                f(item);
            }
        }

        /// <summary>
        /// List.ConvertAll を IEnumerable でも使えるようにする。
        /// </summary>
        public static IEnumerable<TOutput> ConvertAll<T, TOutput>(this IEnumerable<T> self, Converter<T, TOutput> converter)
        {
            foreach (T item in self)
            {
                yield return converter(item);
            }
        }

        /// <summary>
        /// 変換しながらListにする。
        /// </summary>
        public static List<TOutput> ToList<T, TOutput>(this IEnumerable<T> self, Converter<T, TOutput> converter)
        {
            int _size = self.Count(); // selfの実体が本当にIEnumerableだと実行速度がヤバい！

            var list = new List<TOutput>(_size);
            foreach (T item in self)
            {
                list.Add(converter(item));
            }

            return list;
        }

        /// <summary>
        /// 変換しながら配列にする。
        /// </summary>
        public static TOutput[] ToArray<T, TOutput>(this IEnumerable<T> self, Converter<T, TOutput> converter)
        {
            int _size = self.Count();
            var array = new TOutput[_size];

            int i = 0;
            foreach (T item in self)
            {
                array[i++] = converter(item);
            }

            return array;
        }

        /// <summary>
        /// 合計を計算します。
        /// </summary>
        public static Vector2 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector2> selector)
        {
            Vector2 total = Vector2.zero;
            foreach (var item in source)
            {
                total += selector(item);
            }
            return total;
        }

        /// <summary>
        /// 合計を計算します。
        /// </summary>
        public static Vector3 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector3> selector)
        {
            Vector3 total = Vector3.zero;
            foreach (var item in source)
            {
                total += selector(item);
            }
            return total;
        }
    }
}