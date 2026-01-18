//
// (C) 2025 Takap.
//

using System;
using System.Runtime.CompilerServices;

namespace Takap.Utility
{
    /// <summary>
    /// VContainerでインジェクションする時の動作を補助します
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// <paramref name="to"/> に指定したフィールドに <paramref name="from"/> を設定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetValueIfThrowNull<T>(ref T to, T from) where T : class
        {
            if (to != null) throw new InvalidOperationException($"{typeof(T).Name} has already been set.");
            to ??= from ?? throw new ArgumentNullException(nameof(from));
        }
    }
}
