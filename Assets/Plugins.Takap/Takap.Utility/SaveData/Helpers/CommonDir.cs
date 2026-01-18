// 
// (C) 2022 Takap.
//

using System.IO;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 端末の一般ストレージディレクトリを表します。
    /// </summary>
    public static class CommonDir
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -

        // 
        // 標準フォルダ構成を以下の通り
        //
        // Root/
        //   + files/
        //     + Assets/ … ダウンロードしたアセットとかの置き場所
        //     + Common/ … 一般的なデータとかを置いておく場所
        //     + Data/   … セーブデータとか置いておく場所
        // 

        public const string DIR_FILES = "files";
        public const string DIR_ASSETS = "Assets";
        public const string DIR_COMMON = "Common";
        public const string DIR_DATA = "Data";

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 一般ストレージの内部パス
        /// </summary>
        public static string Root => Application.persistentDataPath;

        /// <summary>
        /// Asset ディレクトリパスを取得します。
        /// </summary>
        public static string Assets => Path.Combine(Root, DIR_FILES, DIR_ASSETS).Replace('\\', '/');

        /// <summary>
        /// Common ディレクトリパスを取得します。
        /// </summary>
        public static string Common => Path.Combine(Root, DIR_FILES, DIR_COMMON).Replace('\\', '/');

        /// <summary>
        /// Data ディレクトリパスを取得します。
        /// </summary>
        public static string Data => Path.Combine(Root, DIR_FILES, DIR_DATA).Replace('\\', '/');

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
