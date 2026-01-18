//
// (c) 2020 Takap.
//

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Takap.Utility.Editor
{
    /// <summary>
    /// アセットが何処から参照されているか確認するツール
    /// </summary>
    public class CheckDependencies : EditorWindow
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -
        
        // 依存関係を調べるオブジェクト
        private GameObject targetObject = null;
        // フィルター条件
        private string filterString = "";
        // 対象のオブジェクトが依存しているオブジェクト群
        private UnityEngine.Object[] dependenciesObjects;
        // スクロールバーの位置
        private Vector2 rightScrollPos = Vector2.zero;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        // メニューからウィンドウを開く
        [MenuItem("Assets/CheckDependencies")]
        public static void Open()
        {
            GetWindow(typeof(CheckDependencies), false, nameof(CheckDependencies));
        }

        protected void OnGUI()
        {
            //// フィルター条件
            //this.filterString = EditorGUIUtils.SearchField(new Rect(0, 0, 100, 100), this.filterString);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter:", GUILayout.Width(45));
            GUI.SetNextControlName("filterField");
            filterString = GUILayout.TextField(filterString, "SearchTextField", GUILayout.Width(120));
            GUI.FocusControl("filterField");
            GUI.enabled = !string.IsNullOrEmpty(filterString);
            if (GUILayout.Button("Clear", "SearchCancelButton"))
            {
                filterString = string.Empty;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            // 対象のオブジェクト取得
            this.targetObject = EditorGUILayout.ObjectField("", this.targetObject, typeof(GameObject), allowSceneObjects: true) as GameObject;

            // ボタンが押されたら依存関係の取得
            if (GUILayout.Button("依存関係を取得"))
            {
                this.dependenciesObjects = EditorUtility.CollectDependencies(new UnityEngine.Object[] { this.targetObject });
            }

            if (GUILayout.Button("クリア"))
            {
                this.dependenciesObjects = null;
            }

            // 依存しているオブジェクトの表示
            if (this.dependenciesObjects == null)
            {
                return;
            }

            var list = new List<UnityEngine.Object>(this.dependenciesObjects);
            list.Sort((a, b) => string.Compare(a.name, b.name, true));

            // 部分一致するかどうかのフィルター
            string keyword = this.filterString.ToLower();
            IEnumerable<UnityEngine.Object> filterd = list.Where(o =>
            {
                if (string.IsNullOrEmpty(this.filterString))
                {
                    return true;
                }
                else
                {
                    return o.name.ToLower().Contains(keyword);
                }
            });

            this.rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos);

            var map = new Dictionary<Type, List<UnityEngine.Object>>();
            foreach (UnityEngine.Object obj in filterd)
            {
                Type type = obj.GetType();
                if (!map.ContainsKey(type))
                {
                    map[type] = new List<UnityEngine.Object>();
                }
                map[type].Add(obj);
            }

            foreach (KeyValuePair<Type, List<UnityEngine.Object>> g in map)
            {
                EditorGUILayout.LabelField("--- [" + g.Key.Name + "] ---");
                GUILayout.Box("", GUILayout.Width(this.position.width), GUILayout.Height(1));
                foreach (UnityEngine.Object p in g.Value)
                {
                    EditorGUILayout.ObjectField(p.name, p, typeof(UnityEngine.Object), allowSceneObjects: true);
                }
            }

            //foreach (var obj in filterd)
            //{
            //    EditorGUILayout.ObjectField(obj.name, obj, typeof(UnityEngine.Object), allowSceneObjects: true);
            //}

            EditorGUILayout.EndScrollView();
        }

        public static class EditorGUIUtils
        {
            private static MethodInfo mSearchField;

            static EditorGUIUtils()
            {
                mSearchField = typeof(EditorGUI).GetMethod("SearchField", BindingFlags.Static | BindingFlags.NonPublic);
            }

            public static string SearchField(Rect position, string text)
            {
                if (mSearchField == null)
                {
                    return text;
                }
                object[] args = new object[] { position, text };
                return mSearchField.Invoke(null, args) as string;
            }
        }
    }
}

#endif