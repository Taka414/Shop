//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Takap.Utility
{
    /// <summary>
    /// CSV形式のマスターデータを読み取るための汎用機能を定義します。
    /// </summary>
    public static class CsvMasterDataUtil
    {
        /// <summary>
        /// Excel から出力したCSVファイルの内容を一括でロードします。
        /// </summary>
        public static IEnumerable<CsvRow> LoadCsv(string path, int skipLine = 0)
        {
            //
            // Excel から CSV をエクスポートしたときのルール
            //
            // (1) 改行コードは \r\n
            // (2) ファイルのエンコードはShift-JIS
            // (3) カンマ区切りのみサポートする
            //     ★★途中にカンマが入っているデータはサポートしない
            // (4) # もしくは // から始まる行は全てコメント扱い
            //

            string[] lines = null;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var tr = new StreamReader(fs, Encoding.GetEncoding("Shift-JIS")))
            {
                lines = tr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }

            int p = 0;
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue; // 空の行は無視
                }

                if (line.StartsWith("#") || line.StartsWith("//"))
                {
                    continue; // コメント行のなのでスキップ
                }

                string[] parts = line.Split(',');
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Trim();
                }

                if (p++ >= skipLine)
                {
                    yield return new CsvRow(parts);
                }
            }
        }
    }
}