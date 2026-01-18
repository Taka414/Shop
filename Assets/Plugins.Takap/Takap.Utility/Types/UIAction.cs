//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// UI上で発生する動作を表します。
    /// </summary>
    public enum UIAction
    {
        /// <summary>未設定</summary>
        None = 0,
        /// <summary>押され</summary>
        PointerDown,
        /// <summary>移動した</summary>
        PointerMove,
        /// <summary>離された</summary>
        PointerUp,
    }
}
