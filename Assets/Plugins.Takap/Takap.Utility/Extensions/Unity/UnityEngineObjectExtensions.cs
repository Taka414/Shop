//
// (C) 2022 Takap.
//

using System;
using System.Threading;
using R3;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="UnityEngine.Object"/> の機能を拡張します。
    /// </summary>
    public static class UnityEngineObjectExtensions
    {
        /// <summary>
        /// 指定したコンポーネントの寿命に関連付けます。
        /// </summary>
        public static CancellationTokenRegistration BindTo(this IDisposable disposable, MonoBehaviour obj) => BindTo(disposable, obj.destroyCancellationToken);
        public static CancellationTokenRegistration BindTo(this IDisposable disposable, CancellationToken ct)
        {
            if (!ct.CanBeCanceled)
            {
                throw new ArgumentException("Require CancellationToken CanBeCanceled");
            }

            if (ct.IsCancellationRequested)
            {
                disposable.Dispose();
                return default;
            }

            return ct.UnsafeRegister(delegate (object state)
            {
                ((IDisposable)state).Dispose();
            },
            disposable);
        }

        public static void BindTo(this IDisposable disposable, DisposableBag bug)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }
            bug.Add(disposable);
        }

        /// <summary>
        /// 更新フラグを経てます。
        /// </summary>
        public static void SetDirty(this UnityEngine.Object self)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(self);
#endif
        }
    }

    // R3からパクってきたやつ
    internal static class CancellationTokenExtensions
    {
        public static CancellationTokenRegistration UnsafeRegister(this CancellationToken cancellationToken, Action<object> callback, object state)
        {
            return cancellationToken.Register(callback, state, useSynchronizationContext: false);
        }
    }
}
