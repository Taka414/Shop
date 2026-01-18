// 
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="IList{T}"/> 関係する汎用操作を定義します。
    /// </summary>
    public static class ListUtil
    {
        /// <summary>
        /// リストから指定したN個分の要素を重複せずに取り出します。
        /// </summary>
        public static T[] GetRandom<T>(IList<T> list, int count)
        {
            if (count > list.Count)
            {
                return new T[0];
            }

            // 要素分のインデックスリストを作成
            var indexList = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                indexList.Add(i);
            }

            // 戻り値を入れるための配列
            var array = new T[count];

            // インデックスをランダムに取り出して消費していくことで
            // 元のリストを破壊しないようにする
            for (int i = 0; i < count; i++)
            {
                int index = UniRandom.Range(0, indexList.Count);
                int value = indexList[index];
                indexList.RemoveAt(index);
                array[i] = list[value];
            }
            return array;
        }

        /// <summary>
        /// リストの中から適当に要素を一つ取り出します。
        /// </summary>
        public static T PickupOne<T>(IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("リストが空です。");
            }
            return list[UniRandom.Range(0, list.Count)];
        }
    }
}
