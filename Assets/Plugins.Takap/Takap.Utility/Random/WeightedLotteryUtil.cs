//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 重み付け抽選機能を表します。
    /// </summary>
    public static class WeightedLotteryUtil
    {
        /// <summary>
        /// 抽選条件に従って要素を1つ選択します。
        /// </summary>
        /// <remarks>
        /// 指定した確率で抽選するけど再現性が無い
        /// </remarks>
        public static T SelectOne<T>(WeightedLotteryItem<T>[] list)
        {
            float total = 0;
            for (int i = 0; i < list.Length; i++)
            {
                total += list[i].Weight;
            }

            float value = Random.Range(0, total);
            for (int i = 0; i < list.Length; i++)
            {
                value -= list[i].Weight;
                if (value <= 0) return list[i].Value;
            }

            return default;
        }

        /// <summary>
        /// 抽選条件に従って要素を1つ選択します。
        /// </summary>
        /// <remarks>
        /// 指定した確率で抽選するけど再現性が無い
        /// </remarks>
        public static T SelectOne<T>(List<WeightedLotteryItem<T>> list)
        {
            var span = list.AsSpan(); // Hacked!!!
            int count = list.Count;

            float total = 0;
            for (int i = 0; i < count; i++)
            {
                total += span[i].Weight;
            }

            float value = Random.Range(0, total);
            for (int i = 0; i < count; i++)
            {
                value -= span[i].Weight;
                if (value <= 0) return span[i].Value;
            }

            return default;
        }

        /// <summary>
        /// 抽選条件に従って要素を1つ選択します。
        /// </summary>
        public static T SelectOne<T>(WeightedLotteryItem<T>[] list, ref Random.State state)
        {
            float total = 0;
            for (int i = 0; i < list.Length; i++)
            {
                total += list[i].Weight;
            }

            float value = UniRandom.Range(0, total, ref state);
            for (int i = 0; i < list.Length; i++)
            {
                value -= list[i].Weight;
                if (value <= 0) return list[i].Value;
            }

            return default;
        }

        /// <summary>
        /// 抽選条件に従って要素を1つ選択します。
        /// </summary>
        public static T SelectOne<T>(List<WeightedLotteryItem<T>> list, ref Random.State state)
        {
            var span = list.AsSpan(); // Hacked!!!
            int count = list.Count;

            float total = 0;
            for (int i = 0; i < count; i++)
            {
                total += span[i].Weight;
            }

            float value = UniRandom.Range(0, total, ref state);
            for (int i = 0; i < count; i++)
            {
                value -= span[i].Weight;
                if (value <= 0) return span[i].Value;
            }

            return default;
        }
    }
}