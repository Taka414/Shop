//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 画面のタッチ防止機能を表します。
    /// </summary>
    public interface ISimpleTouchBlocker
    {
        /// <summary>
        /// 操作ブロックを開始します。
        /// </summary>
        void EnableBlock();

        /// <summary>
        /// 操作ブロックを終了します。
        /// </summary>
        void DisableBlock();
    }
}