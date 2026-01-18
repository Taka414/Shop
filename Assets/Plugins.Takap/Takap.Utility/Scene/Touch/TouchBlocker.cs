//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// 画面のタッチ防止機能を表します。
    /// </summary>
    public class TouchBlocker : MonoBehaviour, ISimpleTouchBlocker
    {
        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // 標準シーンブロックが有効かどうかのフラグ
        // true: 有効 / false: 無効
        //  → 無効に設定しているとユーザー指定のブロックを無視 & シーン遷移中の自動ブロックも発生しない
        [SerializeField] bool _isEnabled;
        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 操作ブロックを開始します。
        /// </summary>
        [Button]
        public void EnableBlock()
        {
            if (!_isEnabled)
            {
                Log.Trace("[TouchBlocker] 機能は無効化されているためスキップしました。");
                return;
            }

            Log.Trace("[有効化] TouchBlocker");
            if (gameObject.activeSelf)
            {
                Log.Warn("[拒否] 既に表示中です");
                return;
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 操作ブロックを終了します。
        /// </summary>
        [Button]
        public void DisableBlock()
        {
            if (!_isEnabled)
            {
                Log.Trace("[TouchBlocker] 機能は無効化されてるためスキップしました。");
                return;
            }

            Log.Trace("[無効化] TouchBlocker");
            if (!gameObject.activeSelf)
            {
                Log.Warn("[拒否] 既に非表示中です");
                return;
            }
            gameObject.SetActive(false);
        }
    }
}