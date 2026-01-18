//
// (C) 2022 Takap.
//

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// UniTask の機能を拡張します。
    /// </summary>
    public static class UniTaskExtensions
    {
        // キャンセル付き宣言がfloatの秒指定だと長すぎるので短縮表現にする
        public static UniTask Delay(this MonoBehaviour self, float seconds)
        {
            return UniTask.Delay((int)TimeSpan.FromSeconds(seconds).TotalMilliseconds, cancellationToken: self.destroyCancellationToken);
        }

        /// <summary>
        /// キャンセルを無視してFire&Forgetする指定が長いので短縮表現を提供します。
        /// </summary>
        public static void By(this UniTask task)
        {
            task.SuppressCancellationThrow().Forget();
        }

        /// <summary>
        /// 指定した非同期処理を繰り返し実行します（オブジェクトが破棄されたときのCancel指定は必要ありません）
        /// </summary>
        /// <remarks>
        /// 
        /// 使い方:
        /// (1) 以下のようなメソッドを作成する
        ///   async UniTask Foo(CancellationToken token)
        ///   {
        ///       // 待機条件を記述する
        ///       await UniTask.WaitUntilValueChanged(transform, t => t.localPosition.y, cancellationToken: token);
        ///       
        ///       // やりたい処理を記述する
        ///       Debug.Log("ValueChanged. y=" + transform.localPosition.y);
        ///   }
        ///
        /// (2) 以下のように渡すと(1)の処理がキャンセルするまで繰り返し実行されるようになる
        ///   void Start()
        ///   {
        ///       this.ContinueProcessAsync(Foo, source.Token).Forget();
        ///   }
        ///   
        /// </remarks>
        public static async UniTask ContinueProcessAsync(this MonoBehaviour self, Func<CancellationToken, UniTask> action)
        {
            await ContinueProcessAsync(self, action, self.destroyCancellationToken);
        }

        /// <summary>
        /// 指定した非同期処理を繰り返し実行します（オブジェクトが破棄されたときのCancel指定は必要ありません）
        /// </summary>
        public static async UniTask ContinueProcessAsync(this MonoBehaviour self, Func<CancellationToken, UniTask> action, CancellationToken token)
        {
            // 自分が死んだときにタスク監視を停止するのは必須のため指定が無いときは追加する
            CancellationTokenSource source = null;
            if (self.destroyCancellationToken != token)
            {
                source = CancellationTokenSource.CreateLinkedTokenSource(token, self.destroyCancellationToken);
                Log.Trace("Create Lined");
            }

            // 継続的な監視を開始する
            try
            {
                while (true)
                {
                    CancellationToken t = source == null ? token : source.Token;
                    await action(t);
                }
            }
            catch (OperationCanceledException)
            {
                Log.Trace("Cenceled.");
            }
        }
    }
}