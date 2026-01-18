//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="KeyValuePair{TKey, TValue}"/> を管理するクラス
    /// </summary>
    public class KeyValuePairTable<TKey, TValue>
    {
        private Dictionary<TKey, KeyValuePair<TKey, TValue>> table;

        /// <summary>
        /// オブジェクトが初期化済みかどうかを取得します。
        /// true : 初期化済み / false : また
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// 指定した列挙子を利用してオブジェクトを初期化します
        /// </summary>
        public void Initialize(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            table = new Dictionary<TKey, KeyValuePair<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                table[item.Key] = item;
            }
        }

        /// <summary>
        /// 指定したキーに関係する画像を取得します。
        /// </summary>
        public TValue GetValue(TKey key)
        {
            if (table.ContainsKey(key))
            {
                return table[key].Value;
            }
            throw new NotSupportedException($"key is not suported. key='{key}'");
        }
    }
}