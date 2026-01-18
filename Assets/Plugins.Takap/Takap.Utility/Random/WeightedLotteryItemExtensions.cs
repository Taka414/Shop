//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 重み付け抽選機能の拡張メソッドを定義します。
    /// </summary>
    public static class WeightedLotteryItemExtensions
    {
        /// <summary>
        /// 指定した条件リストの確立に従って1つを選択します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SelectOne<T>(this WeightedLotteryItem<T>[] list) => WeightedLotteryUtil.SelectOne(list);

        /// <summary>
        /// 指定した条件リストの確立に従って1つを選択します。
        /// </summary>
        /// <remarks>
        /// state を指定することで再現性がある抽選を実行できます。
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SelectOne<T>(this WeightedLotteryItem<T>[] list, ref Random.State state) => WeightedLotteryUtil.SelectOne(list, ref state);

        /// <summary>
        /// 指定した条件リストの確立に従って1つを選択します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SelectOne<T>(this List<WeightedLotteryItem<T>> list) => WeightedLotteryUtil.SelectOne(list);

        /// <summary>
        /// 指定した条件リストの確立に従って1つを選択します。
        /// </summary>
        /// <remarks>
        /// state を指定することで再現性がある抽選を実行できます。
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SelectOne<T>(this List<WeightedLotteryItem<T>> list, ref Random.State state) => WeightedLotteryUtil.SelectOne(list, ref state);
    }
}