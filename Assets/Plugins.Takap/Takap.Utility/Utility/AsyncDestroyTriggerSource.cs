//
// (C) 2022 Takap.
//

using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Takap.Utility
{
    //
    // 使い方
    // - - - - - - - - - - - - - - - - - - - -
    #region...
    //public sealed class Square : MonoBehaviour
    //{
    //    AsyncDestroyTrigger2 _ctm;
    //
    //    private void Awake()
    //    {
    //        // 最初に初期化する → 管理用のコンポーネントが追加される
    //        _ctm = this.GetAsyncDestoryTrigger();
    //    }
    //
    //    private void Start()
    //    {
    //        Move().Forget();
    //    }
    //
    //    private async UniTaskVoid Move()
    //    {
    //        try
    //        {
    //            Log.Trace("Start");
    //
    //            for (int i = 0; i < 10; i++)
    //            {
    //                Log.Trace($"[{i}] Exec");
    //                await this.transform.DOLocalMoveX(0.5f, 0.5f).SetRelative().BindTo(this, _ctm.Token);
    //                await this.transform.DOLocalMoveX(-0.5f, 0.5f).SetRelative().BindTo(this, _ctm.Token);
    //            }
    //
    //            Log.Trace("END");
    //        }
    //        catch (OperationCanceledException ex)
    //        {
    //            Log.Trace(ex.ToString());
    //        }
    //    }
    //
    //    public void Cancel()
    //    {
    //        Log.Trace("Cancel");
    //        _ctm.TokenSource.Cancel(); // 自分でキャンセルしたい時はマネージャー経由でキャンセルする
    //    }
    //
    //    public void Remove()
    //    {
    //        Log.Trace("Remove");
    //        Destroy(this.gameObject);
    //    }
    //
    //    // Destroyの時にキャンセル処理は管理オブジェクト側で勝手にやってくれる
    //    //private void OnDestroy()
    //    //{
    //    //    _cts.Cancel();
    //    //    using (_cts) { }
    //    //}
    //}
    #endregion

    /// <summary>
    /// <see cref="AsyncDestroyTriggerSource"/> の機能を拡張します。
    /// </summary>
    public static class AsyncDestoryTriggerSourceExtensions
    {
        ///// <summary>
        ///// <see cref="AsyncDestroyTriggerSource"/> から <see cref="CancellationToken"/> を取得します。
        ///// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static CancellationToken GetCancellationTokenOnDestroyEx(this GameObject gameObject)
        //{
        //    return gameObject.GetAsyncDestroyTrigger().Token;
        //}
        //
        ///// <summary>
        ///// <see cref="AsyncDestroyTriggerSource"/> から <see cref="CancellationToken"/> を取得します。
        ///// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static CancellationToken GetCancellationTokenOnDestroyEx(this Component component)
        //{
        //    return component.GetAsyncDestroyTrigger().Token;
        //}
        //
        ///// <summary>
        ///// <see cref="AsyncDestroyTriggerSource"/> を取得します。なければ追加します。
        ///// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static AsyncDestroyTriggerSource GetAsyncDestroyTrigger(this GameObject gameObject)
        //{
        //    return gameObject.GetOrAddComponent<AsyncDestroyTriggerSource>();
        //}
        //
        ///// <summary>
        ///// <see cref="AsyncDestroyTriggerSource"/> を取得します。なければ追加します。
        ///// </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static AsyncDestroyTriggerSource GetAsyncDestroyTrigger(this Component component)
        //{
        //    return component.GetOrAddComponent<AsyncDestroyTriggerSource>();
        //}
    }

    ///// <summary>
    ///// オブジェクトを破棄するときにTokenSourceにキャンセルを自動で発行する管理オブジェクト
    ///// </summary>
    //[DisallowMultipleComponent]
    //public sealed class AsyncDestroyTriggerSource : MonoBehaviour
    //{
    //    // Cysharp.Threading.Tasks.Triggers.AsyncDestroyTrigger と同じ機構
    //
    //    //
    //    // Fields
    //    // - - - - - - - - - - - - - - - - - - - -
    //
    //    bool _awakeCalled;
    //    bool _called;
    //    CancellationTokenSource cancellationTokenSource;
    //
    //    //
    //    // Props
    //    // - - - - - - - - - - - - - - - - - - - -
    //
    //    /// <summary>
    //    /// Taskをキャンセルするためのソースを取得します
    //    /// </summary>
    //    public CancellationTokenSource TokenSource
    //    {
    //        get
    //        {
    //            if (cancellationTokenSource == null)
    //            {
    //                cancellationTokenSource = new CancellationTokenSource();
    //            }
    //
    //            if (!_awakeCalled)
    //            {
    //                PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, new AwakeMonitor(this));
    //            }
    //
    //            return cancellationTokenSource;
    //        }
    //    }
    //
    //    /// <summary>
    //    /// Taskに渡すTokenを取得します
    //    /// </summary>
    //    public CancellationToken Token => TokenSource.Token;
    //
    //    //
    //    // Runtime impl
    //    // - - - - - - - - - - - - - - - - - - - -
    //
    //    private void Awake()
    //    {
    //        _awakeCalled = true;
    //    }
    //
    //    private void OnDestroy()
    //    {
    //        _called = true;
    //        cancellationTokenSource?.Cancel();
    //        cancellationTokenSource?.Dispose();
    //    }
    //
    //    //
    //    // Methods
    //    // - - - - - - - - - - - - - - - - - - - -
    //
    //    // UniTaskと同じ動きになるように引継ぎ実装
    //    public UniTask OnDestroyAsync()
    //    {
    //        if (_called)
    //        {
    //            return UniTask.CompletedTask;
    //        }
    //
    //        var tcs = new UniTaskCompletionSource();
    //        Token.RegisterWithoutCaptureExecutionContext(state =>
    //        {
    //            var tcs2 = (UniTaskCompletionSource)state;
    //            tcs2.TrySetResult();
    //        }, tcs);
    //
    //        return tcs.Task;
    //    }
    //
    //    private class AwakeMonitor : IPlayerLoopItem
    //    {
    //        readonly AsyncDestroyTriggerSource _trigger;
    //
    //        public AwakeMonitor(AsyncDestroyTriggerSource trigger)
    //        {
    //            _trigger = trigger;
    //        }
    //
    //        public bool MoveNext()
    //        {
    //            if (_trigger._called)
    //            {
    //                return false;
    //            }
    //            if (_trigger == null)
    //            {
    //                _trigger.OnDestroy();
    //                return false;
    //            }
    //            return true;
    //        }
    //    }
    //}
}
