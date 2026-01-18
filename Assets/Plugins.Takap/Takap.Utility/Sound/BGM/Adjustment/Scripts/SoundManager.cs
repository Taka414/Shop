//
// (C) 2023 Takap.
//

using Sirenix.OdinInspector;
using Takap.Utility.Sound;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 再生対象のオーディオ
        [SerializeField, InlineEditor] BgmAudioClip _clip;
        // 飛ぶ時にどれくらい前にジャンプするか？
        [SerializeField, Range(0.5f, 3f)] float _jumpOffset = 1f;
        // 再生時のサウンドのフェード量
        [SerializeField] float _fadeLength = 0.5f;
        // 任意の再生位置の指定
        [SerializeField, MinValue(0)] float _startPos;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        //BgmPlayer _player;
        //IPub<PlayHead> _pub_PlayHead;
        //IPub<ChangeBgm> _pub_ChangeBgm;

        IBgmPlayer _bgmPlayer;
        PlayingLabelUpdater _playingLabelUpdater;
        PlayingTimeUpdater _playingTimeUpdater;
        [VContainer.Inject]
        public void Construct(IBgmPlayer bgmPlayer,
                              PlayingLabelUpdater playingLabelUpdater,
                              PlayingTimeUpdater playingTimeUpdater)
        {
            Validator.SetValueIfThrowNull(ref _bgmPlayer, bgmPlayer);
            Validator.SetValueIfThrowNull(ref _playingLabelUpdater, playingLabelUpdater);
            Validator.SetValueIfThrowNull(ref _playingTimeUpdater, playingTimeUpdater);
        }


        //
        // Props & Events
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                return;
            }

            if (_clip)
            {
                PublishClipInfo();
            }
        }
#endif

        private void Awake()
        {
            //_player = BgmPlayer.Instance;
            //this.SetDefault(ref _pub_PlayHead);
            //this.SetDefault(ref _pub_ChangeBgm);
        }

        private void Start()
        {
            if (!_clip)
            {
                return;
            }
            PublishClipInfo();
            Log.Trace($"Publish {_clip.name}");
        }

        private void Update()
        {
            BgmPlayer player = (BgmPlayer)_bgmPlayer;

            if (_clip)
            {
                _playingTimeUpdater.OnChangeText(player.GetPlayheadTime(), _clip.Length);
            }
            //_pub_PlayHead.Publish(new PlayHead(_player.GetPlayheadTime(), _clip.Length));
            //Log.Trace($"Time={_player.GetPlayheadTime():F1}");
        }

        private void PublishClipInfo()
        {
            if (_clip)
            {
                //_pub_ChangeBgm.Publish(new ChangeBgm(_clip.name));
                _playingLabelUpdater.OnChangeText(_clip.name);
            }
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void Play()
        {
            if (!_clip)
            {
                Log.Warn("BGMが設定されていません。");
                return;
            }

            //_player.Play(_clip, _fadeLength, startTime: _startPos);
            _bgmPlayer.Volume = 1;
            _bgmPlayer.Play(_clip, _fadeLength, startTime: _startPos);
        }

        public void Stop()
        {
            if (!_clip)
            {
                Log.Warn("BGMが設定されていません。");
                return;
            }
            //_player.Stop(_fadeLength);
            _bgmPlayer.Stop(_fadeLength);
        }

        public void JumpToNereLoop()
        {
            if (!_clip)
            {
                Log.Warn("BGMが設定されていません。");
                return;
            }

            Log.Trace($"NonLooping={_clip.NonLooping}, LoopWholeAudio={_clip.LoopWholeAudio}");

            //_player.Play(_clip, startTime: _clip.EndPositon - _jumpOffset);
            _bgmPlayer.Play(_clip, startTime: _clip.EndPositon - _jumpOffset);
        }
    }
}
