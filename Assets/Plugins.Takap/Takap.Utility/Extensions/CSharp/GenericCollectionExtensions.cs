//
// (C) 2022 Takap.
//

using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// Generic.Collections のコンテナに対する拡張処理を記述します。
    /// </summary>
    public static class GenericCollectionExtensions
    {
        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/> の最初の要素を取得します。存在しない場合TValueのデフォルト値を返します。
        /// </summary>
        public static TValue GetFirstItem<Tkey, TValue>(this Dictionary<Tkey, TValue> table)
        {
            if (table.Count == 0)
            {
                return default;
            }

            Tkey key = new List<Tkey>(table.Keys)[0];
            return table[key];
        }

        /// <summary>
        /// リストの最後の要素を1剣だけ削除します。
        /// </summary>
        public static T RemoveLast<T>(this IList<T> src)
        {
            T last = src.Tail();
            src.RemoveAt(src.Count - 1);
            return last;
        }

        /// <summary>
        /// 指定したリストの最後の要素を取得します。
        /// </summary>
        public static T Tail<T>(this IList<T> src)
        {
            return src[src.Count - 1];
        }
    }
}