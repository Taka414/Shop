//
// (C) 2022 Takap.
//

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Timeline;

namespace Takap.Utility.Timeline
{
    /// <summary>
    /// エディタ上のTimeLineでDOTweenのアニメーションをプレビューします。
    /// </summary>
    public abstract class TimeLinePreviwer : MonoBehaviour, ITimeControl
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private Sequence _sequence;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アニメーションの基にしているオブジェクトを取得します（Playしないと設定されないので使用時は注意）
        /// </summary>
        public Tween AnimationSource => _sequence;

        //
        // ITimeControl impl
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// クリップ侵入時に呼び出されます。
        /// <see cref="ITimeControl.OnControlTimeStart"/> の実装
        public void OnControlTimeStart()
        {
            // nop
        }

        /// <summary>
        /// クリップ退出時に呼び出されます。
        /// <see cref="ITimeControl.OnControlTimeStop"/> の実装
        /// </summary>
        public void OnControlTimeStop()
        {
            // nop
        }

        /// <summary>
        /// エディター上のエディットモードの時だけタイムライン更新時に表示を更新します。
        /// <see cref="ITimeControl.OnControlTimeStart"/> の実装
        /// </summary>
        public void SetTime(double _time)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (_sequence == null)
                {
                    return;
                }
                _sequence.Goto((float)_time);
            }
#endif
        }

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// エディター状のエディットモードの時だけ実行します。
        /// </summary>
        public void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                SetupSequence();
            }
#endif
        }

        //
        // Abstracts
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 派生クラスで実装され、受けっとったパラメータに対して設定をすることでアニメーションを構築します。
        /// </summary>
        protected abstract void ImplementSequence(Sequence seq);

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// プレビュー用のアニメーションを実行します。(スクリプト制御 and 実行時用)
        /// </summary>
        public void Play(Action<TimeLinePreviwer> completed = null)
        {
            Sequence seq = SetupSequence();
            if (completed != null)
            {
                seq.AppendCallback(() => completed(this));
            }
            seq.Play();
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// シーケンスを作成し作成したインスタンスを取得します。
        /// 返されるインスタンスはフィールド <see cref="_sequence"/> にも設定されます。
        /// </summary>
        private Sequence SetupSequence()
        {
            if (_sequence != null)
            {
                _sequence.Goto(0);
                _sequence.Kill();
            }
            Sequence seq = DOTween.Sequence();
            ImplementSequence(seq);
            _sequence = seq;
            return seq;
        }
    }
}