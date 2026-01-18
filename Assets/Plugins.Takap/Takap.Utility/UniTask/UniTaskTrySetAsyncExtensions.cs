//
// (C) 2022 Takap.
//

using System;
using Cysharp.Threading.Tasks;

namespace Takap.Utility
{
    public delegate void MyAction(Action callback);
    public delegate void MyAction<T1>(T1 arg1, Action callback);
    public delegate void MyAction<T1, T2>(T1 arg1, T2 arg2, Action callback);
    public delegate void MyAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3, Action callback);

    public delegate void MyFunc<TResult>(Action<TResult> callback);
    public delegate void MyFunc<T1, TResult>(T1 arg1, Action<TResult> callback);
    public delegate void MyFunc<T1, T2, TResult>(T1 arg1, T2 arg2, Action<TResult> callback);
    public delegate void MyFunc<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3, Action<TResult> callback);

    /// <summary>
    /// コールバックでの終了待ちをawaitできるようにする拡張機能を定義します。
    /// </summary>
    public static class UniTaskTrySetAsyncExtensions
    {
        // -------------------------------------------------------------
        // 戻り値なし
        // -------------------------------------------------------------
        // (1) こんな感じのコールバックで通知があるメソッドを...
        // public void ShowDialog(Action onClose)
        // {
        //     onClose?.Invoke(); // 終わったらコールバックで終了が通知される
        // }
        // public void ShowDialog1(string arg1, Action onClose)
        // {
        //     onClose?.Invoke();
        // }
        //
        // (2) 以下のようにawaitで終了を待機できるように変換します
        // public async void Foo()
        // {
        //     await this.ToAsync(_p1.ShowDialog);
        //     await this.ToAsync(_p1.ShowDialog1, "hoge");
        // }
        //
        // キャンセルしたいときは以下のように記述すると
        // この待機はOperationCanceledExceptionを投げて終了する
        // ** ただし待機させてるメソッドの動作は停止しないので別途停止する必要がある。
        // await this.ToAsync(_currentCircle.Play).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        //
        //
        // -------------------------------------------------------------
        public static UniTask ToAsync(MyAction method)
        {
            var source = new UniTaskCompletionSource();
            method(() => { source.TrySetResult(); });
            return source.Task;
        }
        public static UniTask ToAsync<T1>(MyAction<T1> method, T1 arg1)
        {
            var source = new UniTaskCompletionSource();
            method(arg1, () => { source.TrySetResult(); });
            return source.Task;
        }
        public static UniTask ToAsync<T1, T2>(MyAction<T1, T2> method, T1 arg1, T2 arg2)
        {
            var source = new UniTaskCompletionSource();
            method(arg1, arg2, () => { source.TrySetResult(); });
            return source.Task;
        }
        public static UniTask ToAsync<T1, T2, T3>(MyAction<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            var source = new UniTaskCompletionSource();
            method(arg1, arg2, arg3, () => { source.TrySetResult(); });
            return source.Task;
        }

        public static UniTask ToAsync(this UnityEngine.Object _, MyAction method) => ToAsync(method);
        public static UniTask ToAsync<T1>(this UnityEngine.Object _, MyAction<T1> method, T1 arg1) => ToAsync(method, arg1);
        public static UniTask ToAsync<T1, T2>(this UnityEngine.Object _, MyAction<T1, T2> method, T1 arg1, T2 arg2) => ToAsync(method, arg1, arg2);
        public static UniTask ToAsync<T1, T2, T3>(this UnityEngine.Object _, MyAction<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) => ToAsync(method, arg1, arg2, arg3);

        public static UniTask ToAsync(this IUtilityContext _, MyAction method) => ToAsync(method);
        public static UniTask ToAsync<T1>(this IUtilityContext _, MyAction<T1> method, T1 arg1) => ToAsync(method, arg1);
        public static UniTask ToAsync<T1, T2>(this IUtilityContext _, MyAction<T1, T2> method, T1 arg1, T2 arg2) => ToAsync(method, arg1, arg2);
        public static UniTask ToAsync<T1, T2, T3>(this IUtilityContext _, MyAction<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) => ToAsync(method, arg1, arg2, arg3);

        // -------------------------------------------------------------
        // 戻り値あり
        // -------------------------------------------------------------
        // (1) こんな感じのコールバックで実行結果の値を受け取るメソッドを...
        // public void LoadItems(Action<string> completed)
        // {
        //     completed?.Invoke("hoge"); // 終了したら実行結果の値をコールバックで受け取る
        // }
        // 
        // public void LoadItems1(string arg1, Action<string> completed)
        // {
        //     completed?.Invoke("hoge1");
        // }
        //
        // (2) 以下のようにawaitで終了を待機できるように変換します
        // public async void Foo()
        // {
        //     string ret1 = await this.ToAsync<string>(_p1.LoadItems);
        //     string ret2 = await this.ToAsync<string, string>(_p1.LoadItems1, "hoge");
        // }
        // -------------------------------------------------------------
        public static UniTask<TResult> ToAsync<TResult>(MyFunc<TResult> method)
        {
            var source = new UniTaskCompletionSource<TResult>();
            method(ret => { source.TrySetResult(ret); });
            return source.Task;
        }
        public static UniTask<TResult> ToAsync<TResult, T1>(MyFunc<T1, TResult> method, T1 arg1)
        {
            var source = new UniTaskCompletionSource<TResult>();
            method(arg1, ret => { source.TrySetResult(ret); });
            return source.Task;
        }
        public static UniTask<TResult> ToAsync<TResult, T1, T2>(MyFunc<T1, T2, TResult> method, T1 arg1, T2 arg2)
        {
            var source = new UniTaskCompletionSource<TResult>();
            method(arg1, arg2, ret => { source.TrySetResult(ret); });
            return source.Task;
        }
        public static UniTask<TResult> ToAsync<TResult, T1, T2, T3>(MyFunc<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3)
        {
            var source = new UniTaskCompletionSource<TResult>();
            method(arg1, arg2, arg3, ret => { source.TrySetResult(ret); });
            return source.Task;
        }

        public static UniTask<TResult> ToAsync<TResult>(this UnityEngine.Object _, MyFunc<TResult> method) => ToAsync(method);
        public static UniTask<TResult> ToAsync<TResult, T1>(this UnityEngine.Object _, MyFunc<T1, TResult> method, T1 arg1) => ToAsync(method, arg1);
        public static UniTask<TResult> ToAsync<TResult, T1, T2>(this UnityEngine.Object _, MyFunc<T1, T2, TResult> method, T1 arg1, T2 arg2) => ToAsync(method, arg1, arg2);
        public static UniTask<TResult> ToAsync<TResult, T1, T2, T3>(this UnityEngine.Object _, MyFunc<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3) => ToAsync(method, arg1, arg2, arg3);

        public static UniTask<TResult> ToAsync<TResult>(this IUtilityContext _, MyFunc<TResult> method) => ToAsync(method);
        public static UniTask<TResult> ToAsync<TResult, T1>(this IUtilityContext _, MyFunc<T1, TResult> method, T1 arg1) => ToAsync(method, arg1);
        public static UniTask<TResult> ToAsync<TResult, T1, T2>(this IUtilityContext _, MyFunc<T1, T2, TResult> method, T1 arg1, T2 arg2) => ToAsync(method, arg1, arg2);
        public static UniTask<TResult> ToAsync<TResult, T1, T2, T3>(this IUtilityContext _, MyFunc<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3) => ToAsync(method, arg1, arg2, arg3);
    }
}
