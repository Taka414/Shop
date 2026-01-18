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
    /// <see cref="MonoBehaviourExtensions"/> の動作を拡張します。
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// 指定した述語をUniTaskで遅延実行します。async/await対応。
        /// </summary>
        /// <param name="self">MonoBehaviourインスタンス</param>
        /// <param name="seconds">遅延秒数</param>
        /// <param name="action">実行するアクション</param>
        /// <param name="cancellationToken">キャンセル用トークン（オプション）</param>
        public static UniTask DelayAsync(this MonoBehaviour self, float seconds, Action action)
        {
            return DelayAsync(self, seconds, action, self.destroyCancellationToken);
        }

        public static async UniTask DelayAsync(this MonoBehaviour _, float seconds, Action action, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), DelayType.DeltaTime, cancellationToken: ct);
            action();
        }

        /// <summary>
        /// 指定した述語をUniTaskで遅延実行します。async/await対応（引数付き）。
        /// </summary>
        /// <param name="self">MonoBehaviourインスタンス</param>
        /// <param name="seconds">遅延秒数</param>
        /// <param name="arg">アクションに渡す引数</param>
        /// <param name="action">実行するアクション</param>
        /// <param name="cancellationToken">キャンセル用トークン（オプション）</param>
        public static UniTask DelayAsync<T>(this MonoBehaviour self, float seconds, T arg, Action<T> action)
        {
            return DelayAsync(self, seconds, arg, action, self.destroyCancellationToken);
        }
        public static async UniTask DelayAsync<T>(this MonoBehaviour _, float seconds, T arg, Action<T> action, CancellationToken ct = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), DelayType.DeltaTime, cancellationToken: ct);
            action(arg);
        }

        /// <summary>
        /// 指定した非同期メソッドをUniTaskで遅延実行します。
        /// </summary>
        public static UniTask DelayAsync(this MonoBehaviour self, float seconds, Func<UniTask> asyncAction)
        {
            return DelayAsync(self, seconds, asyncAction, self.destroyCancellationToken);
        }
        public static async UniTask DelayAsync(this MonoBehaviour _, float seconds, Func<UniTask> asyncAction, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds), DelayType.DeltaTime, cancellationToken: ct);
            await asyncAction();
        }

        /// <summary>
        /// 指定時間経過後にactionを実行します。
        /// </summary>
        public static void Delay(this MonoBehaviour self, float seconds, Action action) => Delay(self, TimeSpan.FromSeconds(seconds), action);
        public static void Delay(this MonoBehaviour self, TimeSpan span, Action action)
        {
            var token = self.destroyCancellationToken;
            UniTask.RunOnThreadPool(async () =>
            {
                await UniTask.Delay((int)span.TotalMilliseconds, cancellationToken: token);
                action();
            }).By();
        }
    }
}