//
// (C) 2022 Takap.
//

using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="ScrollRect"/> の機能を拡張します。
    /// </summary>
    public static class ScrollRectExtensions
    {
        /// <summary>
        /// リストを一番上に移動します。
        /// </summary>
        public static void JumpTop(this ScrollRect self) => self.verticalNormalizedPosition = 1.0f;
    }
}