//
// (C) 2022 Takap.
//

using UnityEngine;
using UnityEngine.Rendering;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Volume"/> の設定を行います。
    /// </summary>
    public class VolumeSetting : MonoBehaviour
    {
        // 
        // 説明:
        // URPのVolumeが編集時は邪魔だから
        // 実際に実行したときだけ有効化するためのスクリプト
        //
        // "Bloom"
        // "Vignette"
        // などの文字列を指定する
        // 

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // この機能が有効か無効かを設定するフラグ
        // true: 有効 / false: 無効
        [SerializeField] bool _enabled;
        //// 対象のボリューム
        //[SerializeField] Volume _volume;
        Volume _volume;
        // 起動時に有効化する効果の名前
        [SerializeField] string[] _wakeupVolumeItems;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            if (!_enabled)
            {
                Debug.Log("VolumeSetting is disabled.");
                return;
            }

            this.SetComponent(ref _volume);
            _volume.enabled = true;

            VolumeProfile profile = _volume.profile;
            foreach (VolumeComponent item in profile.components)
            {
                string volumeName = item.name.ToLower();

                _wakeupVolumeItems.ForEach(name =>
                {
                    if (volumeName.Contains(name.ToLower()))
                    {
                        if (!item.active)
                        {
                            item.active = true;
                            Debug.Log($"Set enabled. name={item.name}");
                        }
                    }
                });
            }
        }
    }
}
