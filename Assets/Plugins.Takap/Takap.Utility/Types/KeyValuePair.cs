//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Unity 上でシリアル化可能なキーと値の組み合わせを表します。
    /// </summary>
    [Serializable]
    public abstract class KeyValuePair<TKey, TValue>
    {
        // 任意の管理キー
        [SerializeField] private TKey key;
        // キーに対応する
        [SerializeField] TValue value;

        /// <summary>
        /// 管理キーを「取得します。
        /// </summary>
        public TKey Key => key;

        /// <summary>
        /// 管理キーを「取得します。
        /// </summary>
        public TValue Value => value;
    }
}