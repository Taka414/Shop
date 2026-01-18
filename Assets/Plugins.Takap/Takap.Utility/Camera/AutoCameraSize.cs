//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// カメラサイズを自動的に更新するための機能を定義します。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class AutoCameraSize : MonoBehaviour
    {
        //
        // 説明:
        // 今のところ、縦長の画面の時に横に表示される情報量が
        // どんな画面サイズ(比率)でもだいたい同じくらいになるように調整するスクリプト
        //

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 基準の画面の幅
        [SerializeField] private float _width = 1080;
        // 基準の画面の高さ
        [SerializeField] private float _height = 1920;
        // 基準のカメラサイズ
        [SerializeField] private float _size = 5;
        // どちらの大きさで合わせるか
        [SerializeField, ReadOnly] private FitType _fitType = FitType.Width;

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// サイズを合わせる基準を表します。
        /// </summary>
        private enum FitType
        {
            /// <summary>横幅に合わせる</summary>
            Width,
            /// <summary>縦幅に合わせる</summary>
            Height
        }

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        [ShowInInspector, ReadOnly]
        private Camera _camera;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _camera);
            UpdateCameraSize();
        }

#if UNITY_EDITOR
        private void Update()
        {
            UpdateCameraSize();
        }
#endif

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        [Button]
        public void UpdateCameraSize()
        {
            if (!_camera || !_camera.orthographic)
            {
                return;
            }

            if (_fitType == FitType.Width)
            {
                // デザイン時のアスペクト比
                float targetAspectRatio = _height / _width;
                // 現在のアスペクト比
                float currentAspectRatio = Screen.height / (float)Screen.width;

                // 画面が壊れないように結果が不正にならないようにチェックする
                if (float.IsNaN(targetAspectRatio) || float.IsNaN(currentAspectRatio) || targetAspectRatio == 0)
                {
                    //Debug.LogWarning("アスペクト比の計算に失敗しました(1)");
                    return;
                }
                float size = currentAspectRatio / targetAspectRatio * _size;
                if (float.IsNaN(size) || size < 0)
                {
                    //Debug.LogWarning("アスペクト比の計算に失敗しました(2)");
                    return;
                }

                _camera.orthographicSize = size;
            }
            else if (_fitType == FitType.Height)
            {
                Debug.Log("このケースはサポートされません。");
                // TODO: とりあえず横長のゲームを作るときまで実装を保留
            }
        }
    }
}
