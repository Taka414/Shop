//
// (C) 2022 Takap.
// 

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 効果音の再生を表します。
    /// </summary>
    public interface ISePlayer
    {
        /// <summary>
        /// 効果音の音量を設定または取得します。
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// デフォルトチャンネルで効果音を再生します。
        /// </summary>
        public void PlaySE(AudioClip clip);

        /// <summary>
        /// 効果音を再生します。
        /// </summary>
        public void PlaySE(string name, AudioClip clip);
    }
}