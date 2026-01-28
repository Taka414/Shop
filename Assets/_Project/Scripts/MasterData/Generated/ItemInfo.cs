//
// (C) 2026 Takap.
//

using System;
using UnityEngine;

namespace Takap.Games.Shopping
{
    /// <summary>
    /// マスターデータから読み取ったアイテム情報を表します。
    /// </summary>
    [Serializable]
    public readonly partial struct ItemInfo : IEquatable<ItemInfo>
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このオブジェクトを表すキーを取得します。
        /// </summary>
        public readonly uint Key;

        /// <summary>
        /// Addressablesのアドレスを取得します。
        /// </summary>
        public readonly string Address;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public ItemInfo(uint key, string address)
        {
            Key = key;
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        public static implicit operator ItemInfo((uint key, string address) p) => new(p.key, p.address);
        public static implicit operator uint(ItemInfo info) => info.Key;

        public static bool operator ==(ItemInfo left, ItemInfo right) => left.Equals(right);
        public static bool operator !=(ItemInfo left, ItemInfo right) => !left.Equals(right);

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        // Keyのみで等値判定を行う
        public bool Equals(ItemInfo other) => Key == other.Key;
        public override bool Equals(object obj) => obj is ItemInfo other && Equals(other);
        public override int GetHashCode() => Key.GetHashCode();
    }
}
