//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// Z位置の更新方法を表します。
    /// </summary>
    public enum ZIndexMode
    {
        /// <summary>1回更新したら終了します。</summary>
        Once = 0,
        /// <summary>常に更新し続けます。</summary>
        Always,
    }
}