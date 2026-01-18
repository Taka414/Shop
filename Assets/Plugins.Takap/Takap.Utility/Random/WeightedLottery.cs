//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 重み付きのランダム抽選を行うクラス
    /// </summary>
    [System.Serializable]
    public class WeightedLottery<T>
    {
        // 使い方
        #region...
        // 
        // public enum Rarity { Legendary, Epic, Rare, Uncommon, Common }
        // 
        // private void Test()
        // {
        //     抽選確率を設定する
        //     WeightedLottery<Rarity> selector = new();
        //     selector.Add(Rarity.Legendary, 0.5f);
        //     selector.Add(Rarity.Epic, 2.5f);
        //     selector.Add(Rarity.Rare, 12f);
        //     selector.Add(Rarity.Uncommon, 25f);
        //     selector.Add(Rarity.Common, 60f);
        //     
        //     // 抽選する
        //     var result = selector.RandomElement();
        // }
        #endregion

        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] List<WeightedLotteryItem<T>> _list = new();

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        Random.State _state;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 抽選条件のリストを設定または取得します。
        /// </summary>
        public List<WeightedLotteryItem<T>> Items => _list;

        /// <summary>
        /// このオブジェクトの現在の <see cref="Random.State"/> を取得します。
        /// </summary>
        public Random.State State => _state;

        /// <summary>
        /// このオブジェクトの Seed 値を取得します・
        /// </summary>
        public int Seed { get; private set; }

        /// <summary>
        /// 抽選回数を取得します。
        /// </summary>
        public int TryCount { get; private set; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public WeightedLottery()
        {
            Seed = (int)System.DateTime.Now.Ticks;
            _state = UniRandom.GetState(Seed);
        }
        public WeightedLottery(int seed)
        {
            Seed = seed;
            _state = UniRandom.GetState(Seed);
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した条件で抽選条件を追加します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WeightedLotteryItem<T> Add(T item, float weight = 1.0f)
        {
            WeightedLotteryItem<T> condition = new(item, weight);
            _list.Add(condition);
            return condition;
        }

        /// <summary>
        /// 現在のオブジェクトの内容で重み付き抽選を行います。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SelectOne()
        {
            try
            {
                return _list.SelectOne(ref _state);
            }
            finally
            {
                TryCount++;
            }
        }
    }
}