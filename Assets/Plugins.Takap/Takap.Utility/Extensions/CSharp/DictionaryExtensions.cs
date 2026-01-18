//
// (C) 2022 Takap.
//

using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Dictionary{TKey, TValue}"/> を拡張します。
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 指定したキーがテーブルに含まれている場合値を取得しそれ以外は TValue の既定値を返します。
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> table, TKey key)
        {
            if (table.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return default;
        }

        /// <summary>
        /// 指定した複数のキーを先頭から評価しキーの何れかがテーブルに含まれている場合その値を取得し、それ以外は TValue の既定値を返します。
        /// </summary>
        public static TValue GetOrDefaultAll<TKey, TValue>(this Dictionary<TKey, TValue> table, params TKey[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                TKey _key = key[i];
                if (table.TryGetValue(_key, out TValue value))
                {
                    return value;
                }
            }
            return default;
        }
    }
}