// 
// (C) 2022 Takap.
//

using System.IO;

namespace Takap.Utility
{
    /// <summary>
    /// ディレクトリ操作の定型的な処理を定義します。
    /// </summary>
    public static class DirectoryUtil
    {
        /// <summary>
        /// 指定したディレクトリパスが存在しないときだけディレクトリを作成します。
        /// </summary>
        /// <remarks>
        /// 若干意味ないかも。
        /// </remarks>
        public static void SafeCreate(string dir)
        {
            if (Directory.Exists(dir))
            {
                return;
            }
            Directory.CreateDirectory(dir);
        }
    }
}
