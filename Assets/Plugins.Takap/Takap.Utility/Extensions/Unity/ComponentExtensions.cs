//
// (C) 2022 Takap.
//

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Component"/> の機能を拡張します。
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Transform を RectTransform として取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectTransform GetRectTransform(this Component self)
        {
            return self.transform as RectTransform; // 取得できない場合null
        }

        /// <summary>
        /// 親要素を RectTransform として取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectTransform GetParentRectTransform(this Component self)
        {
            return self.transform.parent as RectTransform;
        }

        /// <summary>
        /// オブジェクトにコンポーネントが無ければ新規追加して取得します。あれば既存のオブジェクトを応答します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (!component.TryGetComponent<T>(out var ret))
            {
                ret = component.AddComponent<T>();
            }
            return ret;
        }

        /// <summary>
        /// コンポーネントを指定した引数に設定します。
        /// Editor 実行時は取得できない場合警告メッセージを出力します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetComponent<T>(this Component self, ref T target) where T : Component
        {
            if (target is RectTransform)
            {
                var item = self.transform as T;
                target = item;
                return true;
            }
            else
            {
                T item = self.GetComponent<T>();
                CheckComponent(item, self);
                target = item;
                return item != null;
            }
        }

        /// <summary>
        /// 自分の子要素の末尾に指定したオブジェクトを追加します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SetChildComponent<T>(this Component self, ref T target) where T : Component
        {
            Transform tfm = self.transform;
            int cnt = tfm.childCount;

            if (cnt == 0)
            {
                target = null;
                return false;
            }

            for (int i = 0; i < cnt; i++)
            {
                GameObject go = tfm.GetChild(i).gameObject;
                T component = go.GetComponent<T>();
                if (component != null)
                {
                    target = component;
                    return true;
                }
            }
            target = null;
            return false;
        }

        /// <summary>
        /// 指定したコンポーネントの親のGameObjectのレイヤー番号を比較します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MatchLayer(this Component self, int layer)
        {
            return self.gameObject.layer == layer;
        }

        /// <summary>
        /// <see cref="GameObject.SetActive(bool)"/> へのショートカットです。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActive(this Component self, bool value)
        {
            self.gameObject.SetActive(value);
        }

        /// <summary>
        /// コンポーネントからアンロードされないオブジェクトを指定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DontDestroyOnLoadAnyware(this Component self)
        {
            self.transform.parent = null;
            GameObject.DontDestroyOnLoad(self.gameObject);
        }

        //
        // Priave Methos
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// コンポーネントが存在しない場合ログメッセージを表示します。
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        private static void CheckComponent<T>(T item, Component component)
        {
            if (item is null)
            {
                Log.Warn($"コンポーネントが取得できませんでした。T='{typeof(T).Name}'", component);
            }
        }
    }
}