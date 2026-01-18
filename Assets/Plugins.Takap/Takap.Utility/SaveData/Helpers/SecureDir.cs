// 
// (C) 2022 Takap.
//

using System.IO;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 端末のセキュアディレクトリを表します。
    /// </summary>
    public static class SecureDir
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 書き込み可能なディレクトリパスを取得します。
        /// </summary>
        public static string Root
        {
            get
            {
                // 補足:
                // Androidの場合、Application.persistentDataPathでは外部から読み出せる場所に保存されてしまうため
                // アプリをアンインストールしてもファイルが残ってしまう
                // ここではアプリ専用領域に保存するようにする

#if !UNITY_EDITOR && UNITY_ANDROID

                //
                // Android実機
                //

                using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                using var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                using var getFilesDir = currentActivity.Call<AndroidJavaObject>("getFilesDir");
                return getFilesDir.Call<string>("getCanonicalPath");

#elif !UNITY_EDITOR && UNITY_IOS

                //
                // iOS実機
                //

                return Application.persistentDataPath;

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

                //
                // 開発時のデバッグ用
                //

                return Application.persistentDataPath;

#else
                
                Debug.LogWarning($"想定しないパスが使用されている可能背があります。path='{Application.persistentDataPath}'");
                return Application.persistentDataPath;

#endif
            }
        }

        /// <summary>
        /// Asset ディレクトリパスを取得します。
        /// </summary>
        public static string Assets => Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_ASSETS).Replace('\\', '/');

        /// <summary>
        /// Common ディレクトリパスを取得します。
        /// </summary>
        public static string Common => Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_COMMON).Replace('\\', '/');

        /// <summary>
        /// Data ディレクトリパスを取得します。
        /// </summary>
        public static string Data => Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_DATA).Replace('\\', '/');

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        // -x-x- 以下のファイルパス取得メソッドはフォルダが無い場合作成してからパスを作成する副作用あり -x-x-

        /// <summary>
        /// Asset ディレクトリ以下のファイルパスを作成します。
        /// </summary>
        public static string GetAssetFilePath(string fileName)
        {
            string dataDir = Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_ASSETS);
            DirectoryUtil.SafeCreate(dataDir);
            return Path.Combine(dataDir, fileName).Replace('\\', '/'); // 置き換えておいた方が安全
        }

        /// <summary>
        /// Data ディレクトリ以下のファイルパスを作成します。
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public static string GetCommonFilePath(string fileName)
        {
            string dataDir = Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_COMMON);
            DirectoryUtil.SafeCreate(dataDir);
            return Path.Combine(dataDir, fileName).Replace('\\', '/'); // 置き換えておいた方が安全
        }

        /// <summary>
        /// Data ディレクトリ以下のファイルパスを作成します。
        /// </summary>
        /// <remarks>
        /// フォルダが無い場合作成してからパスを作成する副作用あり。
        /// </remarks>
        public static string GetDataFilePath(string fileName)
        {
            string dataDir = Path.Combine(Root, CommonDir.DIR_FILES, CommonDir.DIR_DATA);
            DirectoryUtil.SafeCreate(dataDir);
            return Path.Combine(dataDir, fileName).Replace('\\', '/'); // 置き換えておいた方が安全
        }
    }
}
