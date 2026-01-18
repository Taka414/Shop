//
// (C) 2022 Takap.
//

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Takap.Utility
{
    /// <summary>
    /// デバッグ中と実行中でライトを切り替える機能
    /// </summary>
    public class GlobalLightSwitcher : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // この機能が有効か無効かを設定するフラグ
        // true: 有効 / false: 無効
        [SerializeField] bool _enabled;

        [SerializeField] Light2D release;
        [SerializeField] Light2D debug;

        private void Awake()
        {
            if (!(_enabled && release && debug))
            {
                Log.Trace("Skip global light2d swithcing.");
                return;
            }
            debug.gameObject.SetActive(false);
            release.gameObject.SetActive(true);
        }
    }
}
