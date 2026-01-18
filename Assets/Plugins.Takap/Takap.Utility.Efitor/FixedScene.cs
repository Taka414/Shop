//
// (c) 2020 Takap.
//

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Takap.Utility.Editor
{
    //
    // このスクリプトを Editor フォルダに入れると Unity のメニュー上に以下項目が追加される。
    //
    // ★Debug > Begin FixedScene 
    //
    // このメニューを選択すると『FixedScene』というウインドウが表示される。
    // そこに開始したいシーンを指定すると「Play」ボタンを押したときに開始するシーンが
    // 「現在表示中のシーン」から「このウインドウで設定したシーン」に強制的に置き換わって実行される、
    // 
    // 初期化シーンを必ず先頭に始めたい場合にこのスクリプトを使用する。
    //

    /// <summary>
    /// 開始シーンを固定するためのクラス
    /// </summary>
    public class FixedScene : EditorWindow
    {
        //設定したシーンのパスを保存するKEY
        private const string saveKey = "Key_BeginScene";

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        [MenuItem("Tools/開始シーン設定")] // メニューの位置の指定
        public static void Open()
        {
            GetWindow<FixedScene>(nameof(FixedScene));
        }

        private void OnEnable()
        {
            // 保存されている最初のシーンのパスがあれば、読み込んで設定
            string startScenePath = EditorUserSettings.GetConfigValue(saveKey);
            if (!string.IsNullOrEmpty(startScenePath))
            {
                // パスからシーンを取得、シーンがなければ警告表示
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(startScenePath);
                if (sceneAsset == null)
                {
                    Debug.LogWarning(startScenePath + "がありません！");
                }
                else
                {
                    BeginSceneName = EditorSceneManager.playModeStartScene.name;
                    EditorSceneManager.playModeStartScene = sceneAsset;
                }
            }
        }

        private void OnGUI()
        {
            // 更新前のplayModeStartSceneに設定されてるシーンのパスを取得
            string beforeScenePath = "";
            if (EditorSceneManager.playModeStartScene != null)
            {
                beforeScenePath = AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene);
            }

            // GUIでシーンファイルを取得し、playModeStartSceneに設定する
            EditorSceneManager.playModeStartScene = 
                (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("シーン"), 
                    EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);

            // 更新後の playModeStartScene に設定されてるシーンのパスを取得
            string afterScenePath = "";
            if (EditorSceneManager.playModeStartScene != null)
            {
                afterScenePath = AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene);
            }

            // playModeStartSceneが変更されたらパスを保存
            if (beforeScenePath != afterScenePath)
            {
                EditorUserSettings.SetConfigValue(saveKey, afterScenePath);
            }
        }

        /// <summary>
        /// 開始したシーンの名前を取得します。
        /// </summary>
        public static string BeginSceneName { get; private set; }
    }
}

#endif