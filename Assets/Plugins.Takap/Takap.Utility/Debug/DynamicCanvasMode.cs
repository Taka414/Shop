//
// (C) 2024 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{

    /// <summary>
    /// 編集中と実行中でカメラ動作を変更するためのスクリプト
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Canvas))]
    public class DynamicCanvasMode : MonoBehaviour
    {
        // この機能が有効か無効かを表すフラグ
        // true: 有効 / false: 無効
        [SerializeField] bool _enabled = true;

        // 追従対象のカメラ, 未設定の時はメインカメラを使用する
        [EnableIf(nameof(_enabled))]
        [SerializeField] Camera _renderCamera;
        // モードを変更する対象のCanvas
        [EnableIf(nameof(_enabled)), ReadOnly]
        [SerializeField] Canvas _canvas;

        // 編集中のモード
        [EnableIf(nameof(_enabled))]
        [SerializeField] RenderMode _editingRenderMode = RenderMode.ScreenSpaceCamera;
        // 実行中のモード
        [EnableIf(nameof(_enabled))]
        [SerializeField] RenderMode _playingRenderMode = RenderMode.ScreenSpaceOverlay;

        // 
        // Unity impl
        // - - - - - - - - - - - - - - - - - - - -

        void OnValidate() => UpdateCanvas();

        void Awake() => UpdateCanvas();

        // 
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        void UpdateCanvas()
        {
            // とりあえずメインカメラを初期値に入れておく
            if (!_renderCamera)
            {
                _renderCamera = Camera.main;
#if UNITY_EDITOR // 編集中は設定をそのまま保存する
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
            if (!_canvas)
            {
                _canvas = GetComponent<Canvas>();
#if UNITY_EDITOR // 編集中は設定をそのまま保存する
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }

            if (!_enabled)
            {
                return; // 無効の時はモード変更を実行しない
            }

            _canvas.worldCamera = _renderCamera;
            _canvas.renderMode =
#if UNITY_EDITOR
                // 編集中は設定に従う
                UnityEditor.EditorApplication.isPlaying ? _playingRenderMode : _editingRenderMode;
#else
                // 実機動作時は実行中のモードを強制的に設定する
                _playingRenderMode;
#endif
        }
    }
}
