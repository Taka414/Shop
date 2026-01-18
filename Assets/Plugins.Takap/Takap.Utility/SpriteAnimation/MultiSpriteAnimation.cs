//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 複数のアニメーションを実行するためのクラス
    /// </summary>
    public class MultiSpriteAnimation : MonoBehaviour
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 経過時間
        float _elapsed;

        // 対象を選択するためのオブジェクト
        ISpriteSelector _selector;

        // アニメーションが終了したかどうかのフラグ、true : 終了した
        bool _isAnimationCompleted;

        // 現在実行中のアニメーション
        AnimationParam _currentAnimationParam;

        // [デバッグ用] 現在のアニメーションのフレーム位置
        [ShowInInspector, ReadOnly]
        int _frameIndex;

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 起動した直後にアニメーションを開始するかどうかのフラグ
        // true: 開始する / false: しない
        [SerializeField] bool _playOnAwake;

        // 起動直後に実行を開始するアニメーションがある場合に設定する
        [SerializeField, EnableIf(nameof(_playOnAwake))] string _defaultAnimationKey = "";

        // スプライトアニメーションの定義
        [SerializeField] List<AnimationParamAsset> _animationParamList = new();

        //
        // Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したアニメーションが終了した時に発生します。
        /// </summary>
        public Observable<string> AnimationCompleted => _animationCompleted;
        private readonly Subject<string> _animationCompleted = new();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションを記録するためのリストを取得します。
        /// </summary>
        public List<AnimationParam> AnimationParamList => _animationParamList.ConvertAll(p => (AnimationParam)p);

        /// <summary>
        /// 現在実行中のアニメーションのパラメーターを取得します。
        /// </summary>
        public AnimationParam CurrentAnimationParam => _currentAnimationParam;

        //
        // Runtime Impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _selector);

            //Log.Trace($"{_animationParamList[0].name}");

            // Awakeで非アクティブにしているが、これを
            // Startでやるとenableをtrueにしたときにこれが走って想定通りにならない
            if (!_playOnAwake)
            {
                enabled = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(_defaultAnimationKey))
            {
                Log.Warn("Not set default animation key.", this);
                return;
            }

            Play(_defaultAnimationKey);
        }

        private void Update()
        {
            if (_animationParamList.Count == 0)
            {
                return;
            }

            if (_currentAnimationParam == null)
            {
                return;
            }

            if (_isAnimationCompleted)
            {
                return;
            }

            _elapsed += Time.deltaTime; // 経過時間監視
            if (_elapsed <= _currentAnimationParam.FrameSpeed)
            {
                return;
            }
            _elapsed = 0f;

            _frameIndex++;
            if (_frameIndex >= _currentAnimationParam.SpriteList.Count)
            {
                if (_currentAnimationParam.LoopsCount == IAnimationParam.INFINIT_LOOPS)
                {
                    _frameIndex = 0;
                }
                else
                {
                    if (!_isAnimationCompleted)
                    {
                        _isAnimationCompleted = true;
                        _animationCompleted.OnNext(_currentAnimationParam.Name);
                        return;
                    }
                }
            }

            ChangeImage(_currentAnimationParam.SpriteList[_frameIndex]);
        }

        private void OnDestroy()
        {
            _animationCompleted?.Dispose();
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションを追加します。
        /// </summary>
        public void AddAnimation(AnimationParamAsset param)
        {
            _animationParamList.Add(param);
        }

        /// <summary>
        /// 指定した名前のアニメーションを開始します。
        /// </summary>
        /// <remarks>
        /// rewind = true: 同じアニメーションの場合先頭から再生をやり直すかかどうかのフラグ
        /// </remarks>
        [Button]
        public void Play(string name, bool rewind = false)
        {
            if (!rewind && _currentAnimationParam != null && _currentAnimationParam.Name == name)
            {
                return; // 既に起動している場合
            }

            if (!_animationParamList.TryFind(p => p.Name == name, out var param))
            {
                Log.Warn($"Not found animation parameter name={name}", this);
                return;
            }
            enabled = true;

            _selector.SetFlipX(param.FlipX);
            _selector.SetFlipY(param.FlipY);

            // 最初から実行
            _currentAnimationParam = param;
            _elapsed = 0f;
            _frameIndex = 0;
            _isAnimationCompleted = false;

            // 強制的に表示
            ChangeImage(_currentAnimationParam.SpriteList[_frameIndex]);
        }

        /// <summary>
        /// 指定したアニメーションが実行中かどうかを判定します。
        /// true : 実行中 / false : それ以外
        /// </summary>
        public bool IsPlayingAnimation(string name)
        {
            return _currentAnimationParam.Name == name;
        }

        /// <summary>
        /// アニメーションを一時停止します。
        /// </summary>
        public void PauseAnimation() => enabled = false;

        /// <summary>
        /// アニメーションを再開します。
        /// </summary>
        public void ResumeAnimation() => enabled = true;

        /// <summary>
        /// 指定した名前でアニメーションがリストにあるかどうかを確認します。
        /// true : 持っている / false : 持っていない
        /// </summary>
        public bool HasAnimation(string name)
        {
            return _animationParamList.FindIndex(p => p.Name == name) != -1;
        }

        /// <summary>
        /// 画像を差し替えるタイミングになった時に発生します。
        /// </summary>
        private void ChangeImage(Sprite sp)
        {
            _selector.ChangeImage(sp);
        }
    }
}