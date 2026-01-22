//
// (C) 2026 Takap.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;

namespace Takap.Games.CheckAssets
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
        public readonly string Key;

        /// <summary>
        /// Addressablesのアドレスを取得します。
        /// </summary>
        public readonly string Address;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public ItemInfo(string key, string address)
        {
            Key = key;
            Address = address;
        }

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        public static implicit operator ItemInfo((string key, string address) p) => new(p.key, p.address);

        public static bool operator ==(ItemInfo left, ItemInfo right) => left.Equals(right);
        public static bool operator !=(ItemInfo left, ItemInfo right) => !left.Equals(right);

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        // Keyのみで等値判定を行う
        public bool Equals(ItemInfo other) => string.Equals(Key, other.Key, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is ItemInfo other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Key);
    }

    // 
    public readonly partial struct ItemInfo
    {
        /// <summary></summary>
        public static readonly ItemInfo Leaf = ("{12345678-1234-1234-1234-123456789012}", "assets/icons/leaf.png");

        /// <summary></summary>
        public static readonly ItemInfo Wood = ("{306B735D-5456-44BD-A5DB-DF636941B1F4}", "assets/icons/wood.png");

        /// <summary></summary>
        public static readonly ItemInfo Tree = ("{1A754549-72D7-4248-B02A-3D0A7E39A3E5}", "assets/icons/tree.png");

        static ItemInfo()
        {
            // これも自動生成
            _list = new List<ItemInfo>()
            {
                Leaf,
                Wood,
                Tree,
            };
            Items = new ReadOnlyCollection<ItemInfo>(_list);
        }

        public static ReadOnlyCollection<ItemInfo> Items;
        static readonly IList<ItemInfo> _list;
    }
}
