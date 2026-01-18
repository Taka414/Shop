//
// (C) 2024 Takap.
//

using System;
using System.IO;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 画面のスクリーンショットを自動で撮影します。
    /// </summary>
    public class AutoScreenShot : MonoBehaviour
    {
#if UNITY_EDITOR
        // 
        // 説明:
        // 1) オブジェクトにスクリプトをアタッチする
        // 2) ゲームを再生する
        // 3) _isEnabledにチェックを入れる
        //  → 勝手に撮影が始まって一定間隔でゲームの画面がキャプチャーされる
        // 

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 撮影間隔(秒)
        [SerializeField] float _interval = 0.25f;
        // 機能が有効かどうか
        [SerializeField] bool _isEnabled;
        // 画像キャプチャーを保存するフォルダパス
        [SerializeField] string _dir = "Capture";

        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        void OnValidate()
        {
            enabled = _isEnabled;
            _elapsed = 0;
        }

        float _elapsed;

        void Update()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed > _interval)
            {
                if (!Directory.Exists(_dir))
                {
                    Directory.CreateDirectory(_dir);
                }
                CaptureScreenShot($"{_dir}\\{DateTime.Now:yyyyMMddHHmmssfff}.png");
            }
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        // 画面全体のスクリーンショットを保存する
        private void CaptureScreenShot(string filePath)
        {
            ScreenCapture.CaptureScreenshot(filePath);
        }
#endif
    }
}