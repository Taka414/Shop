//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 編集中と実行中でカメラ動作を変更するためのスクリプト
    /// </summary>
    [ExecuteAlways]
    public class DynamicCanvasModeManager : MonoBehaviour
    {
        // この機能が有効か無効かを表すフラグ
        // true: 有効 / false: 無効
        [SerializeField] bool _enabled;

        // RenderMode.ScreenSpaceCamera の時に使用するカメラ
        // 未設定の時はメインカメラを使用する
        [SerializeField, EnableIf(nameof(_enabled))] Camera _renderCamera;
        // モードを変更する対象のCanvas
        [SerializeField, EnableIf(nameof(_enabled))] Canvas[] _targetCanvasList;

        // 編集中のモード
        [SerializeField, EnableIf(nameof(_enabled))] RenderMode _editingRenderMode = RenderMode.ScreenSpaceCamera;
        // 実行中のモード
        [SerializeField, EnableIf(nameof(_enabled))] RenderMode _playingRenderMode = RenderMode.ScreenSpaceOverlay;

        public void Awake()
        {
            if (!_enabled)
            {
                return;
            }

            if (_renderCamera == null)
            {
                _renderCamera = Camera.main;
            }

            if (_targetCanvasList == null || _targetCanvasList.Length == 0)
            {
                return;
            }

#if UNITY_EDITOR
            foreach (Canvas canvas in _targetCanvasList)
            {
                canvas.renderMode =
                    UnityEditor.EditorApplication.isPlaying ?
                        _playingRenderMode : _editingRenderMode;
            }
#else
            foreach (var canvas in _targetCanvasList)
            {
                // 実機動作時は強制的にオーバーレイ表示にする
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
#endif
            foreach (Canvas canvas in _targetCanvasList)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    canvas.worldCamera = _renderCamera;
                }
            }
        }
    }
}
