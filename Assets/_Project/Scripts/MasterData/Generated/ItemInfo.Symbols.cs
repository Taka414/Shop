//
// 2026 Takap.
//

// このファイルは 2026/01/27 01:24:25 に自動生成されました。

using System.Collections.Generic;

namespace Takap.Games.Shopping
{
    /// <summary>
    /// アイテムリソースを表します。
    /// </summary>
    public readonly partial struct ItemInfo
    {
        /// <summary>'金色の鍵' を表します。</summary>
        public static readonly ItemInfo KeyGold = (1, "Assets/_Project/Textures/Icons/key_gold.png");

        /// <summary>'悪魔の鍵' を表します。</summary>
        public static readonly ItemInfo KeyDevil = (2, "Assets/_Project/Textures/Icons/key_devil.png");

        /// <summary>'通常の鍵' を表します。</summary>
        public static readonly ItemInfo KeyCommon = (3, "Assets/_Project/Textures/Icons/key_common.png");

        /// <summary>
        /// 全ての要素を取得します。
        /// </summary>
        public static readonly IReadOnlyList<ItemInfo> Items = new[] 
        {
            KeyGold,
            KeyDevil,
            KeyCommon,
        };
    }
}
