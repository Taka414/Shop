//
// (C) 2022 Takap.
//

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Takap.Utility
{
    // 
    // ★使用するときはシンボル定義に以下を追加する
    //   → プロジェクトを新規に取り込んだ時に抜けちゃうので必要になる
    // 
    // UNITASK_DOTWEEN_SUPPORT
    //

    /// <summary>
    /// UniTask の機能を拡張します。
    /// </summary>
    public static class UniTaskDOTweenExtensions
    {
        // 
        // TweenCancelBehaviourの説明
        // 
        // * Kill
        //    → tween.Kill(false/*OnCompleteばない*/) + 例外なし
        // * KillAndCancelAwait
        //    → tweeb.Kill(false) + OperationCanceledExceptionが発生
        // 
        // * KillWithCompleteCallback
        //    → tween.Kill(true/*OnComplete呼ぶ*/) + 例外なし
        // * KillWithCompleteCallbackAndCancelAwait
        //    → tween.Kill(true) + OperationCanceledExceptionが発生
        // 
        // * Complete
        //    → tween.Complete(false/*最終位置に移動 + OnComplete呼ぶ*/) + 例外なし
        // * CompleteAndCancelAwait
        //    → tween.Complete(false) + OperationCanceledExceptionが発生
        // 
        // * CompleteWithSequenceCallback
        //    → tween.Complete(true/*最終位置にジャンプ + 途中のコールバック全部呼び出し + OmComplete*/) + 例外なし
        // * CompleteWithSequenceCallbackAndCancelAwait
        //    → tween.Complete(true) + OperationCanceledExceptionが発生
        // 
        // * CancelAwait
        //    → よくわからないけどOperationCanceledExceptionが発生
        // 

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする。
        /// </summary>
        /// <exception cref="OperationCanceledException">タスクがキャンセルされた</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask BindTo(this Tween tween, MonoBehaviour comp, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetLink(comp).ToUniTask(tcb, comp.GetCancellationTokenOnDestroy2());
            return tween.ToUniTask(tcb, comp.destroyCancellationToken);
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする + tween.SetRelative() する
        /// </summary>
        /// <exception cref="OperationCanceledException">タスクがキャンセルされた</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask BindToRltv(this Tween tween, MonoBehaviour comp, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetRelative().SetLink(comp).ToUniTask(tcb, comp.GetCancellationTokenOnDestroy2());
            return tween.SetRelative().ToUniTask(tcb, comp.destroyCancellationToken);
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// </summary>
        /// <exception cref="OperationCanceledException">タスクがキャンセルされた</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask BindTo(this Tween tween, MonoBehaviour comp, CancellationToken token, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetLink(comp).ToUniTask(tcb, token);
            return tween.ToUniTask(tcb, token);
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// </summary>
        /// <exception cref="OperationCanceledException">タスクがキャンセルされた</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask BindToRltv(this Tween tween, MonoBehaviour comp, CancellationToken token, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetRelative().SetLink(comp).ToUniTask(tcb, token);
            return tween.SetRelative().ToUniTask(tcb, token);
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// キャンセルされた場合戻り値をboolで受け取ります。
        /// </summary>
        /// <returns>true: キャンセルされた / false: それ以外</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<bool> BindToSprs(this Tween tween, MonoBehaviour comp, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetLink(comp).ToUniTask(tcb, comp.GetCancellationTokenOnDestroy2()).SuppressCancellationThrow();
            return tween.ToUniTask(tcb, comp.destroyCancellationToken).SuppressCancellationThrow();
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// キャンセルされた場合戻り値をboolで受け取ります。
        /// </summary>
        /// <returns>true: キャンセルされた / false: それ以外</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<bool> BindToSprsRltv(this Tween tween, MonoBehaviour comp, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetRelative().SetLink(comp).ToUniTask(tcb, comp.GetCancellationTokenOnDestroy2()).SuppressCancellationThrow();
            return tween.SetRelative().ToUniTask(tcb, comp.destroyCancellationToken).SuppressCancellationThrow();
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// キャンセルされた場合戻り値をboolで受け取ります。
        /// </summary>
        /// <returns>true: キャンセルされた / false: それ以外</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<bool> BindToSprs(this Tween tween, CancellationToken token, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetLink(comp).ToUniTask(tcb, token).SuppressCancellationThrow();
            return tween.ToUniTask(tcb, token).SuppressCancellationThrow();
        }

        /// <summary>
        /// 宣言が長すぎるので短縮表現にする
        /// キャンセルされた場合戻り値をboolで受け取ります。
        /// </summary>
        /// <returns>true: キャンセルされた / false: それ以外</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<bool> BindToSprsRltv(this Tween tween, CancellationToken token, TweenCancelBehaviour tcb = TweenCancelBehaviour.KillAndCancelAwait)
        {
            //return tween.SetRelative().SetLink(comp).ToUniTask(tcb, token).SuppressCancellationThrow();
            return tween.SetRelative().ToUniTask(tcb, token).SuppressCancellationThrow();
        }
    }
}