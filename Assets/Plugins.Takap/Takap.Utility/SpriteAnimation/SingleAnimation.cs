//
// (C) 2022 Takap.
//

using System;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// スプライトアニメーションを実行する機能を表します。
    /// </summary>
    public class SingleAnimation : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - -

        // アニメーション時に使用する Sprite のリスト
        // → スクリプト実行モードの時はPlayを読んだ時に引数から設定される
        [SerializeField]
        AnimationParamAsset _animParam;

        // アクティブ化したときにアニメーションを開始するかどうか
        [SerializeField]
        bool _playOnAwake;

        // 設定をオーバーライドするかどうかのフラグ
        // true: する / false : しない
        [SerializeField]
        bool _overrideParam;

        // アニメーションのループ回数
        // -1 で無限繰り返し、無限繰り返しの場合Completeは発生しない
        [SerializeField, EnableIf(nameof(_overrideParam))]
        int _loopsCount = 1;

        // 画像の切り替え速度
        [SerializeField, Range(0.01f, 1.0f), EnableIf(nameof(_overrideParam))]
        float _frameSpeed = 0.1f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - -

        // 実際に再生するアニメーション
        private AnimationParam _animParamImpl;

        // 画像セレクター
        private ISpriteSelector _selector;

        // アニメーションが終了したかどうかのフラグ
        //  true : 終了 / false : まだ
        private bool _isCompleted;

        // アニメーション開始からの経過時間(秒)
        private float _elapsed;

        // 現在のフレームの位置
        private int _frameIndex;

        // 何回ループしたかのカウンター
        private int _currentLoopsCount;

        //
        // Events
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションが完了したことを通知します。
        /// </summary>
        public Observable<SingleAnimation> AnimationCompleted => _animationCompleted;
        private readonly Subject<SingleAnimation> _animationCompleted = new();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 画像を何回するのかを設定または取得します。<see cref="IsLoop"/> が false の場合のみ有効です。
        /// </summary>
        public int LoopsCount
        {
            get => _overrideParam ? _loopsCount : _animParam.LoopsCount;
            set
            {
                if (!_overrideParam)
                {
                    Log.Warn("Not set override param.", this);
                    return;
                }
                _loopsCount = value;
            }
        }

        /// <summary>
        /// 画像を切り替える速度を秒指定で設定または取得します。
        /// </summary>
        public float FrameSpeed
        {
            get => _overrideParam ? _frameSpeed : _animParam.FrameSpeed;
            set
            {
                if (!_overrideParam)
                {
                    Log.Warn("Not set override param.", this);
                    return;
                }
                _frameSpeed = value;
            }
        }

        /// <summary>
        /// 一時停止かどうかを設定または取得します。
        /// true : 一時停止 / false : 通常
        /// </summary>
        public bool Pause
        {
            get => enabled;
            set => enabled = value;
        }

        //
        // Runtime Impl
        // - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            enabled = false;

            this.SetComponent(ref _selector);
            _selector.Sprite = null; // Playが呼ばれるまで画像表示しない

            if (_animParam)
            {
                _animParamImpl = _animParam;
            }

            if (_playOnAwake)
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorApplication.isPlaying && _animParam)
                {
                    Play();
                }
#else
                if (_animParam)
                {
                    Play();
                }
#endif
            }
        }

        private void Update()
        {
            if (_isCompleted)
            {
                enabled = false;
                return;
            }

            _elapsed += Time.deltaTime; // 経過時間監視
            if (_elapsed <= FrameSpeed)
            {
                return;
            }
            _elapsed = 0f;

            _frameIndex++;
            if (_frameIndex >= _animParam.SpriteList.Count)
            {
                _currentLoopsCount++;

                if (LoopsCount == IAnimationParam.INFINIT_LOOPS)
                {
                    _frameIndex = 0; // 無限ループの場合最初から
                }
                else
                {
                    if (_currentLoopsCount >= LoopsCount) // 再生が終わったら自分を削除して終了
                    {
                        _isCompleted = true;
                        _animationCompleted.OnNext(this);
                        Destroy(gameObject);
                        enabled = false;
                        return;
                    }
                }
            }

            _selector.ChangeImage(_animParam.SpriteList[_frameIndex]);
        }

        private void OnDestroy()
        {
            _animationCompleted?.Dispose();
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在の設定値でアニメーションを実行します。
        /// </summary>
        [Button]
        public void Play()
        {
            if (_animParamImpl == null)
            {
                Log.Warn("AnimationParam is not set.");
                return;
            }

            _isCompleted = false;
            _frameIndex = 0;
            _currentLoopsCount = 0;
            _selector.ChangeImage(_animParamImpl.SpriteList[0]);
            enabled = true;
        }

        /// <summary>
        /// 指定したアニメーションを実行します。
        /// </summary>
        public void Play(AnimationParam param)
        {
            _animParamImpl = param;
            Play();
        }

        /// <summary>
        /// 指定したアニメーションを実行します。
        /// </summary>
        public void Play(AnimationParamAsset param)
        {
            _animParamImpl = param;
            Play();
        }

        /// <summary>
        /// 実行中のアニメーションを中断します。
        /// </summary>
        public void Abort()
        {
            enabled = false;
            _isCompleted = true;
            _selector.Sprite = null;
        }
    }
}