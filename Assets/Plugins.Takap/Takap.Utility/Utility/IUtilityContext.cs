//
// (C) 2023 Takap.
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// ユーティリティ機能をオブジェクトに追加するためのマーカーインターフェース
    /// </summary>
    public interface IUtilityContext { }

    /// <summary>
    /// オブジェクトクラスに追加するほどじゃないけど広い範囲で機能を提供したい機能定義です。
    /// </summary>
    public static class IUtilityContextExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FindObject<T>(this IUtilityContext _, ref T target, bool includeInactive = false) where T : UnityEngine.Object
        {
            target = __FindObjectCore<T>(includeInactive);
            if (target == null)
            {
                Log.Warn($"オブジェクトが見つかりませんでした。");
                throw new ObjectNotFoundException($"FindFirstObjectByType の実行時に型 '{typeof(T).Name}' が見つかりませんでした。");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FindObject<T>(this UnityEngine.Object _, ref T target, bool includeInactive = false) where T : UnityEngine.Object
        {
            target = __FindObjectCore<T>(includeInactive);
            if (target == null)
            {
                Log.Warn($"オブジェクトが見つかりませんでした。");
                throw new ObjectNotFoundException($"FindFirstObjectByType の実行時に型 '{typeof(T).Name}' が見つかりませんでした。");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T __FindObjectCore<T>(bool includeInactive) where T : UnityEngine.Object
        {
            if (includeInactive)
            {
                return UnityEngine.Object.FindFirstObjectByType<T>(FindObjectsInactive.Include); // こっちのほうがOfTypeを使うより速い
            }
            else
            {
                return UnityEngine.Object.FindFirstObjectByType<T>();
            }
        }

        /// <summary>
        /// 子要素の中からコンポーネントを検索して全て取得します。
        /// </summary>
        public static void SetComponentsInChildren<T>(this Component self, ref T[] items, bool includeInactive = false) where T : Component
        {
            items = self.GetComponentsInChildren<T>(includeInactive);
        }
        /// <summary>
        /// 子要素の中からコンポーネントを検索して最初に見つかったものを取得します。
        /// </summary>
        public static void SetComponentInChildren<T>(this Component self, ref T items, bool includeInactive = false) where T : Component
        {
            items = self.GetComponentInChildren<T>(includeInactive);
        }
    }
}
