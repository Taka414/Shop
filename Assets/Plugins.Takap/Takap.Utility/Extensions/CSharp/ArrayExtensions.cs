//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// 配列に対する拡張機能を提供します。
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// <see cref="ArrayUtil.Clone{T}(T[,])"/> と同じ機能を持つ拡張メソッド
        /// </summary>
        public static T[,] Clone<T>(this T[,] src) => ArrayUtil.Clone(src);

        /// <summary>
        /// <see cref="ArrayUtil.Cut{T}(T[,], int, int, int, int)"/> と同じ機能を持つ拡張メソッド
        /// </summary>
        public static T[,] Cut<T>(this T[,] src, int x, int y, int w, int h) => ArrayUtil.Cut(src, x, y, w, h);

        /// <summary>
        /// <see cref="ArrayUtil.CutByCenter{T}(T[,], int, int, int, int)"/> と同じ機能を持つ拡張メソッド
        /// </summary>
        public static (T[,] map, Rect2Di rect) CutByCenter<T>(this T[,] src, int cx, int cy, int rw, int rh) => ArrayUtil.CutByCenter(src, cx, cy, rw, rh);

        /// <summary>
        /// <see cref="ArrayUtil.TostringByDebug{T}(T[,])"/> と同じ機能を持つ拡張メソッド
        /// </summary>
        public static string ToStringByDebug<T>(this T[,] src) => ArrayUtil.TostringByDebug(src);

        /// <summary>
        /// 指定した条件を満たす要素の数をカウントします。
        /// </summary>
        public static int Count<T>(this T[] self, Func<T, bool> f)
        {
            int i = 0;
            foreach (T item in self)
            {
                if (f(item))
                {
                    i++;
                }
            }
            return i;
        }

        /// <summary>
        /// 配列の中から適当に要素を一つ取り出します。
        /// </summary>
        public static T PickupOne<T>(this T[] self) => ArrayUtil.PickupOne(self);
    }
}
