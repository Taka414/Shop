//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 任意の型 {T} と画像のペアを表します。
    /// </summary>
    [Serializable]
    public abstract class KeySpritePair<TKey> : KeyValuePair<TKey, Sprite>
    {
        /// <summary>
        /// キーに対応する <see cref="Sprite"/> を取得します。
        /// </summary>
        public Sprite Sprite => Value;
    }
}