//
// (C) 2022 Takap.
//
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// FPSを画面に表示します。
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        //
        // 使い方:
        // ゲームオブジェクトにアタッチしてテキストを設定すると
        // 指定したテキストにFPSを表示するようになる
        //

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 表示を更新する間隔(sec)
        [SerializeField, Range(0.1f, 1.0f)] float _updateInterval = 0.5f;
        // テキストを表示する領域(1)
        [SerializeField] Text _fpsText;
        // テキストを表示する領域(2)
        [SerializeField] TextMeshProUGUI _fpsTextTmp;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 経過したフレーム数
        int _frameCount;
        // 前回表示した時間
        float _prevTime;

        //
        // Runtimes
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            if (_fpsText)
                _fpsText.text = "0";

            if (_fpsTextTmp)
                _fpsTextTmp.text = "0";
        }

        private void Update()
        {
            _frameCount++;
            float time = Time.realtimeSinceStartup - _prevTime;

            if (time <= _updateInterval)
            {
                return;
            }

            float fps = Mathf.Round(_frameCount / time);
            _frameCount = 0;
            _prevTime = Time.realtimeSinceStartup;

            if (_fpsText)
            {
                _fpsText.text = fps.ToString("F0");
            }
            if (_fpsTextTmp)
            {
                _fpsTextTmp.text = fps.ToString("F0");
            }
        }
    }
}