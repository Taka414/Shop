//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniObject = UnityEngine.Object;

namespace Takap.Utility
{
    /// <summary>
    /// オブジェクトプールを表します。
    /// </summary>
    public abstract class SimpleObjectPool<T> : IDisposable where T : Component
    {
        //
        // 使い方:
        // https://indie-du.com/entry/2017/04/13/200000
        //

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // Disposeされたかどうかのフラグ
        // true : された / false : まだ
        private bool _isDisposed;
        // プールされたオブジェクトを管理するキュー
        private readonly Queue<T> _queue = new Queue<T>();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Limit of instace count.
        /// </summary>
        public int MaxPoolCount { get; set; } = 3;

        /// <summary>
        /// 現在のプールのオブジェクト数を取得します。
        /// </summary>
        public int Count
        {
            get
            {
                if (_queue == null)
                {
                    return 0;
                }
                return _queue.Count;
            }
        }

        /// <summary>
        /// このプール経由で生成したオブジェクトの総数を取得します。
        /// </summary>
        protected int TotalInstance { get; private set; }

        //
        // Abstract & virtual
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Create instance when needed.
        /// </summary>
        protected abstract T CreateInstance();

        /// <summary>
        /// Called before return to pool, useful for set active object(it is default behavior).
        /// </summary>
        protected virtual void OnBeforeRent(T instance)
        {
            instance.gameObject.SetActive(true);
        }

        /// <summary>
        /// Called before return to pool, useful for set inactive object(it is default behavior).
        /// </summary>
        protected virtual void OnBeforeReturn(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when clear or disposed, useful for destroy instance or other finalize method.
        /// </summary>
        protected virtual void OnClear(T instance)
        {
            if (instance == null)
            {
                return;
            }

            GameObject go = instance.gameObject;
            if (go == null)
            {
                return;
            }

            UniObject.Destroy(go);
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Get instance from pool.
        /// </summary>
        public T Rent()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }

            T instance = (_queue.Count > 0) ? _queue.Dequeue() : CreateInstanceCore();

            OnBeforeRent(instance);

            return instance;
        }

        /// <summary>
        /// Return instance to pool.
        /// </summary>
        public void Return(T instance)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if ((_queue.Count + 1) > MaxPoolCount)
            {
                // 最大数を超えたものは削除する
                //throw new InvalidOperationException("Reached Max PoolSize");
                Debug.LogWarning("Reached Max PoolSize. name=" + instance.name);
                UniObject.Destroy(instance.gameObject);
                return;
            }

            OnBeforeReturn(instance);
            _queue.Enqueue(instance);
        }

        /// <summary>
        /// Clear pool.
        /// </summary>
        public void Clear(bool callOnBeforeRent = false)
        {
            if (_queue == null) return;
            while (_queue.Count != 0)
            {
                T instance = _queue.Dequeue();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }
                OnClear(instance);
            }
        }

        /// <summary>
        /// Trim pool instances. 
        /// </summary>
        /// <param name="instanceCountRatio">0.0f = clear all ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        public void Shrink(float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
        {
            if (_queue == null)
            {
                return;
            }
            if (instanceCountRatio <= 0)
            {
                instanceCountRatio = 0;
            }
            if (instanceCountRatio >= 1.0f)
            {
                instanceCountRatio = 1.0f;
            }

            int size = (int)(_queue.Count * instanceCountRatio);
            size = Math.Max(minSize, size);

            while (_queue.Count > size)
            {
                T instance = _queue.Dequeue();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }
                OnClear(instance);
            }
        }

        ///// <summary>
        ///// If needs shrink pool frequently, start check timer.
        ///// </summary>
        ///// <param name="checkInterval">Interval of call Shrink.</param>
        ///// <param name="instanceCountRatio">0.0f = clearAll ~ 1.0f = live all.</param>
        ///// <param name="minSize">Min pool count.</param>
        ///// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        //public IDisposable StartShrinkTimer(TimeSpan checkInterval, float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
        //{
        //    return Observable.Interval(checkInterval)
        //        .TakeWhile(_ => !_isDisposed)
        //        .Subscribe(_ => { Shrink(instanceCountRatio, minSize, callOnBeforeRent); });
        //}

        //
        // 非同期サイズ縮小の使い方
        // ShrinkAsync(TimeSpan.FromSeconds(2), 0.6f, 20, token).SuppressCancellationThrow().Forget();
        // ShrinkAsync(TimeSpan.FromSeconds(2), 0.6f, 20, token).By();
        //

        /// <summary>
        /// プールを頻繁に縮小する必要がある場合は、チェックタイマーを開始するします・
        /// </summary>
        /// <param name="checkInterval">チェック間隔</param>
        /// <param name="instanceCountRatio">0.0f = clearAll ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        public async UniTask ShrinkAsync(TimeSpan checkInterval, float instanceCountRatio, int minSize, bool callOnBeforeRent, CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                Shrink(instanceCountRatio, minSize, callOnBeforeRent);
                await UniTask.Delay(checkInterval, cancellationToken: token);
            }
        }

        //
        // プリロード メソッドの使い方:
        //
        // arg1: 作成するオブジェクトの個数
        // arg2: 1フレーム当たりの作成する個数
        // pool.PreloadAsync(10, 1).Subscribe(_ => Debug.Log("preload finished")));
        //
        ///// <summary>
        ///// プールにあらかじめオブジェクトをロードしておきます。
        ///// </summary>
        ///// <param name="preloadCount">生成するオブジェクトの個数</param>
        ///// <param name="threshold">1フレームに生成するオブジェクト数</param>
        ///// 
        //public IObservable<Unit> PreloadAsync(int preloadCount, int threshold)
        //{
        //    return UniRx.Observable.FromMicroCoroutine<Unit>((observer, cancel) => PreloadCore(preloadCount, threshold, observer, cancel));
        //}

        /// <summary>
        /// プールにあらかじめオブジェクトをロードしておきます。
        /// </summary>
        /// <param name="preloadCount">生成するオブジェクトの個数</param>
        /// <param name="threshold">1フレームに生成するオブジェクト数</param>
        /// 
        public UniTask PreloadAsync(int preloadCount, int threshold, CancellationToken token)
        {
            // - - - - - - - - - - - - - - - - - - - -
            // 補足:
            // 呼び出し側で
            // await PreloadAsync(10, 1, token):
            // もしくは以下のように呼び出される想定
            // PreloadAsync(10, 1, token).Forget();
            // - - - - - - - - - - - - - - - - - - - -
            return PreloadCoreAsync(preloadCount, threshold, token).SuppressCancellationThrow(); // だるいのでキャンセルは無視
        }

        /// <summary>
        /// 現在保持しているオブジェクトの一覧に <paramref name="fnc"/> で指定した処理を一律で適用します。
        /// </summary>
        public void ForEach(Action<T> fnc)
        {
            foreach (T item in _queue)
            {
                fnc(item);
            }
        }

        //
        // Other Methods
        // - - - - - - - - - - - - - - - - - - - -

        T CreateInstanceCore()
        {
            TotalInstance++;
            return CreateInstance();
        }

        //IEnumerator PreloadCore(int preloadCount, int threshold, IObserver<Unit> observer, CancellationToken cancellationToken)
        //{
        //    Log.Trace(">>>");
        //
        //    while (Count < preloadCount && !cancellationToken.IsCancellationRequested)
        //    {
        //        int requireCount = preloadCount - Count;
        //        if (requireCount <= 0)
        //        {
        //            break;
        //        }
        //
        //        int createCount = Math.Min(requireCount, threshold);
        //
        //        for (int i = 0; i < createCount; i++)
        //        {
        //            try
        //            {
        //                T instance = CreateInstanceCore();
        //                Return(instance);
        //            }
        //            catch (Exception ex)
        //            {
        //                observer.OnError(ex);
        //                yield break;
        //            }
        //        }
        //        yield return null; // next frame.
        //    }
        //
        //    observer.OnNext(Unit.Default);
        //    observer.OnCompleted();
        //
        //    Log.Trace("<<<");
        //}

        async UniTask PreloadCoreAsync(int preloadCount, int threshold, CancellationToken cancellationToken)
        {
            Log.Trace($"[START] {GetType().Name} Preload");

            Log.Trace(">>>");
            //try
            //{
            while (Count < preloadCount && !cancellationToken.IsCancellationRequested)
            {
                int requireCount = preloadCount - Count;
                if (requireCount <= 0)
                {
                    break;
                }

                int createCount = Math.Min(requireCount, threshold);

                for (int i = 0; i < createCount; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return; // キャンセルされた場合は処理を終了
                    }

                    T instance = CreateInstanceCore();
                    Return(instance);
                }

                // 次のフレームまで待機
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            Log.Trace($"[END] {GetType().Name} Preload");
        }

        //
        // Dipose pattern impl
        // - - - - - - - - - - - - - - - - - - - -

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            if (disposing)
            {
                Clear(false);
            }
            _isDisposed = true;
        }
    }
}