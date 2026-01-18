//
// (C) 2022 Takap.
//

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Object"/> に関係する汎用操作を提供します。
    /// </summary>
    public static class ObjectUtil
    {
        /// <summary>
        /// 指定したゲームオブジェクトを安全に削除します。
        /// 実行中/編集画面上共通でオブジェクトを削除する必要がある場合に使用します。
        /// </summary>
        public static void SafeDestroyWithEditor(Object obj)
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                EditorApplication.delayCall += () => Object.DestroyImmediate(obj);
            }
#else
            GameObject.Destroy(obj);
#endif
        }

        /// <summary>
        /// 破棄した後呼び出し元のフィールドをnullにクリアします。
        /// </summary>
        /// <remarks>
        /// プロパティは無理なのであきらめること。
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDestroy<T>(ref T target) where T : Object
        {
            Object.Destroy(target);
            target = null;
        }

        /// <summary>
        /// 破棄した後呼び出し元のフィールドをnullにクリアします。
        /// </summary>
        /// <remarks>
        /// プロパティは無理なのであきらめること。
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDestroy<T>(ref T target, float t) where T : Object
        {
            Object.Destroy(target, t);
            target = null;
        }

        /// <summary>
        /// 配列のリストの内容をすべてDestroyした後に配列の内容をnullに設定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDestroy<T>(T[] array) where T : Object
        {
            if (array == null) return;
            for (int i = 0; i < array.Length; i++)
            {
                Object.Destroy(array[i]);
                array[i] = null;
            }
        }

        /// <summary>
        /// リストの内容をすべてDestroyした後に配列の内容をnullに設定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeDestroy<T>(ICollection<T> collection) where T : Object
        {
            if (collection == null) return;
            foreach (Object obj in collection)
            {
                Object.Destroy(obj);
            }
            collection.Clear();
        }
    }
}