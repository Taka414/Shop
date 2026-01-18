//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 一定周期で指定した処理を実行するためのクラス
    /// </summary>
    public class IntervalAction
    {
        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        public const int Infilit = -1;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 前回実行時からの経過時間
        private float _elapsed;
        // 処理の呼び出し回数
        private int _totalCount;

        //
        // Eventes
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 一定間隔で実行する処理の本体を設定または取得します。
        /// </summary>
        public Action Impl { get; set; }

        /// <summary>
        /// <see cref="MaxCount"/> が <see cref="Infilit"/> 以外の時に繰り返しが終了した場合呼び出されます。
        /// </summary>
        public Action<IntervalAction> Completed { get; set; }

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在の処理を実行する間隔を秒指定で取得します。
        /// </summary>
        public float CurrentInterval { get; private set; }

        /// <summary>
        /// 現在定周期処理を処理を実行中かどうかを表すフラグを取得します。
        /// true : 実行中 / false : それ以外
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// 処理が遅延した場合、1フレーム内で複数回処理を実行するかどうかを設定または取得します。
        /// true : 実行する(既定値) / false : 実行しない
        /// </summary>
        public bool RunMultipleOnDelay { get; set; } = true;

        /// <summary>
        /// <see cref="RunMultipleOnDelay"/> が true の時に1フレーム内で実行回数の上限を設定または取得します。
        /// 既定値は-1(無制限)です。
        /// </summary>
        public int MaxMultipleCount { get; set; } = -1;

        /// <summary>
        /// 最大繰り返し回数を設定または取得します。
        /// </summary>
        public int MaxCount { get; set; } = Infilit;

        /// <summary>
        /// 初回の処理を即時実行下かどうかを
        /// </summary>
        public bool ImmediatelyFirst { get; private set; }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 一定時間が経過したときに処理を実行します。
        /// MonoBehavior.Update() で呼び出してください。
        /// </summary>
        public void Update()
        {
            if (!IsActive)
            {
                return;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed <= CurrentInterval)
            {
                return; // 時間が経過していない
            }

            int cnt = 0;
            while (true)
            {
                if (_elapsed <= CurrentInterval)
                {
                    break; // 時間が経過していない
                }

                if (++cnt > MaxMultipleCount && MaxMultipleCount != -1)
                {
                    IsActive = false;
                    break; // 1回に実行できる回数を超過した
                }

                if (RunMultipleOnDelay)
                {
                    // 遅延した場合、同一フレーム複数回をで実行する場合
                    try
                    {
                        _ExecImpl();
                    }
                    finally
                    {
                        _elapsed -= CurrentInterval;
                    }
                }
                else
                {
                    // 遅延しても時間が経過したときに1回しか実行しない場合
                    try
                    {
                        _ExecImpl(); // 時間が経過したので処理を実行
                        break;
                    }
                    finally
                    {
                        _elapsed = 0; // カウンターをリセット
                    }
                }

                if (MaxCount != Infilit && _totalCount >= MaxCount)
                {
                    Completed?.Invoke(this); // 呼び出し回数が超過したので終了
                    IsActive = false;
                }
            }
        }

        /// <summary>
        /// 定周期に処理を実行する間隔を秒単位で指定して実行を開始します。
        /// </summary>
        public void Start(float interval, bool immediatelyFirst = false)
        {
            CurrentInterval = interval;
            _totalCount = 0;
            IsActive = true;

            ImmediatelyFirst = immediatelyFirst;
            if (immediatelyFirst)
            {
                _elapsed = interval;
            }
        }

        /// <summary>
        /// 定周期処理を停止します。
        /// </summary>
        public void Stop()
        {
            //this.CurrentInterval = -1;
            IsActive = false;
        }

        /// <summary>
        /// 繰り返し回数をリセットします。
        /// </summary>
        public void Reset()
        {
            _totalCount = 0;

            if (ImmediatelyFirst)
            {
                _elapsed = CurrentInterval;
            }
        }

        /// <summary>
        /// 処理の本体を実行します。
        /// </summary>
        public void _ExecImpl()
        {
            Impl();
            _totalCount++;
        }
    }
}