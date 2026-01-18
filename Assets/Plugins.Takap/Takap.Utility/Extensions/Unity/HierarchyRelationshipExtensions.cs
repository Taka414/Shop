//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// オブジェクトの親子関係に関係する設定を拡張します。
    /// </summary>
    public static class HierarchyRelationshipExtensions
    {
        // 引数で指定した要素を自分の子要素に設定する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this GameObject self, GameObject child, bool worldPositionStays = false)
        {
            child.SetParent(self, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this GameObject self, Component child, bool worldPositionStays = false)
        {
            child.SetParent(self, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this Component self, GameObject child, bool worldPositionStays = false)
        {
            child.SetParent(self, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChild(this Component self, Component child, bool worldPositionStays = false)
        {
            child.SetParent(self, worldPositionStays);
        }

        //---

        // 引数で指定した要素を自分の親に設定する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetParent(this GameObject self, GameObject parent, bool worldPositionStays = false)
        {
            self.transform.SetParent(parent.transform, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetParent(this GameObject self, Component parent, bool worldPositionStays = false)
        {
            self.transform.SetParent(parent.transform, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetParent(this Component self, GameObject parent, bool worldPositionStays = false)
        {
            self.transform.SetParent(parent.transform, worldPositionStays);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetParent(this Component self, Component parent, bool worldPositionStays = false)
        {
            self.transform.SetParent(parent.transform, worldPositionStays);
        }

        //---

        // 子要素を取得する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject GetChild(this GameObject self, string name)
        {
            Transform childTtrans = self.transform.Find(name);
            if (childTtrans == null) { return null; }
            return childTtrans.gameObject;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject GetChild(this Component self, string name)
        {
            Transform childTtrans = self.transform.Find(name);
            if (childTtrans == null) { return null; }
            return childTtrans.gameObject;
        }

        // 名前を指定して子要素のゲームオブジェクトを取得してその<T>のコンポーネントを取得する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetChildComponent<T>(this GameObject self, string name) where T : Component
        {
            GameObject go = self.GetChild(name);
            if (go == null) { return default; }
            return go.GetComponent<T>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetChildComponent<T>(this Component self, string name) where T : Component
        {
            GameObject go = self.GetChild(name);
            if (go == null) { return default; }
            return go.GetComponent<T>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetChildComponentPath<T>(this Component self, string path) where T : Component
        {
            // Sample/Child/Sub のようなパスを指定する

            GameObject go = self.gameObject;
            foreach (string name in path.Split('/'))
            {
                go = go.GetChild(name);
                if (go == null)
                {
                    Log.Warn($"GameObject not found. name={name}", self);
                    return default;
                }
            }

            T dest = go.GetComponent<T>();
            if (dest == null)
            {
                Log.Warn($"Component not found. Type={typeof(T).Name}", self);
            }

            return dest;
        }

        // 子要素のコンポーネントを設定する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetChildComponent<T>(this Component self, string name, ref T dest) where T : Component
        {
            GameObject go = self.GetChild(name);
            if (go == null)
            {
                Log.Warn($"GameObject not found. name={name}", self);
                return;
            }
            dest = go.GetComponent<T>();
            if (dest == null)
            {
                Log.Warn($"Component not found. Type={typeof(T).Name}", self);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetChildComponentPath<T>(this Component self, string path, ref T dest) where T : Component
        {
            // Sample/Child/Sub のようなパスを指定する

            GameObject go = self.gameObject;
            foreach (string name in path.Split('/'))
            {
                go = go.GetChild(name);
                if (go == null)
                {
                    Log.Warn($"GameObject not found. name={name}", self);
                    return;
                }
            }

            dest = go.GetComponent<T>();
            if (dest == null)
            {
                Log.Warn($"Component not found. Type={typeof(T).Name}", self);
            }
        }

        //---

        // 子要素をすべて列挙する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GameObject> GetChilds(this GameObject self)
        {
            Transform tfm = self.transform;
            int cnt = tfm.childCount;
            for (int i = 0; i < cnt; i++)
            {
                yield return tfm.GetChild(i).gameObject;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GameObject> GetChilds(this Component self)
        {
            Transform tfm = self.transform;
            int cnt = tfm.childCount;
            for (int i = 0; i < cnt; i++)
            {
                yield return tfm.GetChild(i).gameObject;
            }
        }

        // 子要素で指定したコンポーネントを持つものだけを列挙する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetChilds<T>(this GameObject self) where T : Component
        {
            Transform tfm = self.transform;
            int cnt = tfm.childCount;
            for (int i = 0; i < cnt; i++)
            {
                GameObject go = tfm.GetChild(i).gameObject;
                T component = go.GetComponent<T>();
                if (component != null)
                {
                    yield return component;
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetChilds<T>(this Component self) where T : Component
        {
            Transform tfm = self.transform;
            int cnt = tfm.childCount;
            for (int i = 0; i < cnt; i++)
            {
                GameObject go = tfm.GetChild(i).gameObject;
                T component = go.GetComponent<T>();
                if (component != null)
                {
                    yield return component;
                }
            }
        }

        // 親を取得
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GameObject GetParent(this Component self)
        {
            return self.transform.parent.gameObject;
        }

        // 親のコンポーネントを取得
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetParentComponent<T>(this Component self) where T : Component
        {
            return self.transform.parent.gameObject.GetComponent<T>();
        }

        // コンポーネントを追加する
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AddComponent<T>(this Component self) where T : Component
        {
            return self.gameObject.AddComponent<T>();
        }
    }
}