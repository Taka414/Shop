//
// (C) 2022 Takap.
//

using System;
using R3;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// スプライトアニメーションを実行する機能を表します。動的スプライト生成版。
    /// </summary>
    public abstract class SingleAnimanionBase : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - -

        // 起動したときにアニメーションを開始するかどうかのフラグ
        //  true : する / false : しない
        [SerializeField] private bool _playOnawake = true;
        // アニメーションをループするかどうかのフラグ
        // true : ループする / false : ループしない
        [SerializeField] bool _isLoop;
        // 画像を何回するのか、isLoop が true の時だけ有効
        [SerializeField, Range(1, 100)] int _loopsCount = 1;
        // 画像の切り替え速度 [sec]
        [SerializeField, Range(0.01f, 1.0f)] float _frameSpeed = 0.1f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - -

        protected ISpriteSelector Selector;
        // アニメーションが終了したかどうかのフラグ
        //  true : 終了 / false : まだ
        private bool _isCompleted;
        // アニメーション開始からの経過時間(秒)
        private float _elapsed;
        // 再生中かどうかのフラグ
        // true : 再生中 / false : 待機中
        private bool _isPlay;
        // 現在のフレームの位置
        private int _frameIndex;
        // 何回目のループかを表すカウンター
        private int _currentLoopsCount;

        //
        // Events
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションが完了したことを通知します。
        /// </summary>
        public Observable<SingleAnimanionBase> AnimationCompleted => _animationCompleted;
        private readonly Subject<SingleAnimanionBase> _animationCompleted = new();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションをループするかどうかのフラグを設定または取得します。
        /// </summary>
        public bool IsLoop { get => _isLoop; set => _isLoop = value; }

        /// <summary>
        /// 画像を何回するのかを設定または取得します。<see cref="IsLoop"/> が false の場合のみ有効です。
        /// </summary>
        public int LoopsCount { get => _loopsCount; set => _loopsCount = value; }

        /// <summary>
        /// 画像を切り替える速度を秒指定で設定または取得します。
        /// </summary>
        public float FrameSpeed { get => _frameSpeed; set => _frameSpeed = value; }

        /// <summary>
        /// 再生中かどうかを設定または取得します。
        /// true: 再生 / false: 待機
        /// </summary>
        public bool IsPlaying { get => _isPlay; set => _isPlay = value; }

        //
        // Abstract Methods
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した番号の <see cref="Sprite"/> を取得します。
        /// </summary>
        protected abstract Sprite GetSprite(int index);

        /// <summary>
        /// 管理画像の個数を取得します。
        /// </summary>
        protected abstract int GetImageCount();

        //
        // Runtime Implementes
        // - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref Selector);
            Selector.ChangeImage(GetSprite(0));
        }

        private void Start()
        {
            _currentLoopsCount = 0;
            _frameIndex = 1;

            if (_playOnawake)
            {
                _isPlay = true;
            }
        }

        private void Update()
        {
            if (!_isPlay)
            {
                return;
            }

            if (_isCompleted)
            {
                return; // 終了している
            }

            _elapsed += Time.deltaTime;
            if (_elapsed <= _frameSpeed)
            {
                return; // まだ時間が経過していない
            }

            _elapsed = 0.0f;
            _frameIndex++;
            if (_frameIndex >= GetImageCount())
            {
                _currentLoopsCount++;

                if (IsLoop)
                {
                    _frameIndex = 0; // 最初から
                }
                else
                {
                    if (_currentLoopsCount >= LoopsCount) // 再生が終わったら自分を削除して終了
                    {
                        _isCompleted = true;
                        //AnimationCompleted?.Invoke(this); // 死ぬ前にイベント通知する
                        _animationCompleted.OnNext(this);
                        Destroy(gameObject);
                        return;
                    }
                }
            }

            OnFrameChange(GetSprite(_frameIndex));
        }

        private void OnDestroy()
        {
            using (_animationCompleted) { }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 再生を開始します。
        /// </summary>
        public void Play()
        {
            _isPlay = true;
        }

        /// <summary>
        /// 再生を一時停止します。
        /// </summary>
        public void Pause()
        {
            _isPlay = false;
        }

        /// <summary>
        /// 画像を差し替えます。
        /// </summary>
        public virtual void OnFrameChange(Sprite sp) => Selector.ChangeImage(sp);
    }
}