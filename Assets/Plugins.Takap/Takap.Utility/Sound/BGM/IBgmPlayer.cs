// 
// (C) 2023 Takap.
// 

using R3;
using Takap.Utility.Sound;

namespace Takap.Utility
{
    /// <summary>
    /// BGM再生を制御します。
    /// </summary>
    public interface IBgmPlayer
    {
        // 
        // ボリューム関係
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在のボリュームを設定または取得します。
        /// 0.0 ～ 1.0
        /// </summary>
        /// <remarks>
        /// デフォルト実装は初期化時に音量を指定しないと音量ゼロなので注意
        /// </remarks>
        float Volume { get; set; }

        /// <summary>
        /// 音量を0.1上げます。
        /// </summary>
        void VolumeUp();

        /// <summary>
        /// 音量を0.1下げます。
        /// </summary>
        void VolumeDown();

        /// <summary>
        /// 音量が変更されたときに発生します。
        /// </summary>
        Observable<float> VolumeChanged { get; }

        // 
        // 再生・停止などの操作
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// BGMを再生します。
        /// </summary>
        void Play(BgmAudioClip introloopAudio, float fadeLengthSeconds = 0, float startTime = 0);

        /// <summary>
        /// BGMを再生します。
        /// </summary>
        void Play(BgmAudioClip introloopAudio);

        /// <summary>
        /// 指定位置にシークします。
        /// </summary>
        void Seek(float elapsedTime);

        /// <summary>
        /// BGMを停止します。
        /// </summary>
        void Stop();

        /// <summary>
        /// BGMを停止します。
        /// </summary>
        void Stop(float fadeLengthSeconds);

        /// <summary>
        /// BGMを一時停止します。
        /// </summary>
        void Pause();

        /// <summary>
        /// BGMを一時停止します。
        /// </summary>
        void Pause(float fadeLengthSeconds);

        /// <summary>
        /// BGMを再開します。
        /// </summary>
        void Resume(float fadeLengthSeconds = 0);

        /// <summary>
        /// メモリ上に展開する？
        /// </summary>
        void Preload(BgmAudioClip introloopAudio);
    }
}