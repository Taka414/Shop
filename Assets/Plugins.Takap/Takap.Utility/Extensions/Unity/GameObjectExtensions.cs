//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="GameObject"/> の機能を拡張します。
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// オブジェクトにコンポーネントが無ければ新規追加して取得します。あれば既存のオブジェクトを応答します。
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
    }
}