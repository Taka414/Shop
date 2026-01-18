// 
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 重みづけ抽選を行うときに使用するコンテナの基底クラスを表します。
    /// </summary>
    public abstract class FrequencyContainerBase<T>
    {
        // オブジェクトの元ネタ
        [SerializeField, HorizontalGroup("a"), HideLabel] T _source;
        // 出現率(%)
        [SerializeField, HorizontalGroup("a"), HideLabel] float _rate;

        public float Rate => _rate;
        public T Source => _source;
    }

    /// <summary>
    /// <see cref="FrequencyContainerBase{T}"/> の機能を拡張します。
    /// </summary>
    public static class FrequencyContainerExtension
    {
        /// <summary>
        /// <see cref="AliasSampler"/> で使用する条件リストを取得します。
        /// </summary>
        public static IEnumerable<float> GetRateList<T>(this IEnumerable<FrequencyContainerBase<T>> self)
        {
            foreach (FrequencyContainerBase<T> item in self)
            {
                yield return item.Rate;
            }
        }

        /// <summary>
        /// <see cref="FrequencyContainerBase{T}"/> を使用して抽選機を初期化します。
        /// </summary>
        public static void AddWeights<T>(this AliasSampler self, IEnumerable<FrequencyContainerBase<T>> conditions)
        {
            if (self == null)
            {
                return;
            }
            self.AddWeights(conditions.GetRateList());
        }

        /// <summary>
        /// 重みづけ抽選して配列から結果のオブジェクトを取得します。
        /// </summary>
        public static T GetSource<T>(this AliasSampler self, FrequencyContainerBase<T>[] array)
        {
            int index = self.Sample();
            return array[index].Source;
        }
    }
}
