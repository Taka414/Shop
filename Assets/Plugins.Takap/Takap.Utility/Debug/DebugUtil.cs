#if UNITY_EDITOR

//
// (C) 2024 Takap.
//

using System.Diagnostics;

namespace Takap.Utility
{
    /// <summary>
    /// 編集中のデバッグ用の機能を定義します。
    /// </summary>
    public static class DebugUtil
    {
        /// <summary>
        /// ゲームを一時停止します。
        /// </summary>
        /// <remarks>
        /// 定義したまま実機で動かすと止まらない動作になる。
        /// 必要かどうかは不明
        /// </remarks>
        [Conditional("UNITY_EDITOR")]
        public static void PauseGame()
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
    }
}

#endif