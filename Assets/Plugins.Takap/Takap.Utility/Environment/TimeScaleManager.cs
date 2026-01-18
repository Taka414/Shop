//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// デバッグ用にタイムスケールを変更するスクリプト
    /// </summary>
    public class TimeScaleManager : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 再生速度
        [SerializeField, Range(0.01f, 2.0f)] private float _timeScale = 1.0f;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            ChangeTimeScale(1.0f); // 設定しっぱなしだと困るときの方が多いので起動時はx1にしておく
        }

        private void OnValidate()
        {
            Time.timeScale = _timeScale;
        }

        //
        // Public methods
        // - - - - - - - - - - - - - - - - - - - -

        [Button]
        public void ChangeTimeScale(float tscale = 1.0f)
        {
            Time.timeScale = _timeScale = tscale;
        }

        [Button, LabelText("スケールをリセット")]
        public void ResetTimeScale()
        {
            Time.timeScale = 1.0f;
        }
    }
}