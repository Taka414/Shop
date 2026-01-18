//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Component"/> の機能を拡張します。
    /// </summary>
    public static class SpriteSelectorExtensions
    {
        /// <summary>
        /// コンポーネントを指定した引数に設定します。
        /// Editor 実行時は取得できない場合警告メッセージを出力します。
        /// </summary>
        public static bool SetComponent(this Component self, ref ISpriteSelector target)
        {
            target = SpriteSelector.GetInstance(self);
            return target != null;
        }
    }
}
