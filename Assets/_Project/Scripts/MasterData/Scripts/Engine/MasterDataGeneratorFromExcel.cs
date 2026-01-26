//
// (C) 2025 Takap.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;
using Takap.Utility;
using UnityEngine;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// マスターデータが処理できることを表します。
    /// </summary>
    public interface IMasterDataGenerator
    {
        /// <summary>
        /// 指定されたファイルパスから定義を生成します。
        /// </summary>
        /// <param name="filePath">定義ファイル(マスターデータなど)</param>
        /// <param name="outDir">生成物の出力先</param>
        /// <returns>生成したファイルパスの配列</returns>
        string[] Generate(string filePath, string outDir);
    }

    // - - - - - - - - - - - - - - - - - - - -

    /// <summary>
    /// エクセルからデータを生成します。
    /// </summary>
    public class MasterDataGeneratorFromExcel : IMasterDataGenerator
    {
        // シート名: 設定
        const int SHEET_SETTINGS = 0;
        // シート名: データ
        const int SHEET_DATA = 1;
        // データが出現するまでに読み飛ばす行数
        const int SKIP_LINES = 1;
        // 設定シートの値の列
        const int COL_DATA = 1;

        readonly IAppLogger _logger;
        public MasterDataGeneratorFromExcel(IAppLogger logger)
        {
            _logger = logger;
        }

        public string[] Generate(string filePath, string outDir)
        {
            List<string> fileList = new();
            try
            {
                _logger.Trace("★生成開始");

                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                // Excelから内容を読み出す
                using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream, GetEncode());
                DataSet dataSet = reader.AsDataSet();

                SettingsPage settings = ReadSettingPage(dataSet.Tables[SHEET_SETTINGS]);

                string path = Path.Combine(outDir, settings.FileName);
                using var fs = File.Create(path);
                using var sr = new StreamWriter(fs);

                fileList.Add(path); // 先に更新リストに追加してから書き出す

                CreateData(dataSet.Tables[SHEET_DATA], settings, sr);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            finally
            {
                _logger.Trace("★生成終了");
            }

            return fileList.ToArray();
        }

        //　エンコードの設定を作成
        static ExcelReaderConfiguration GetEncode()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return new() { FallbackEncoding = Encoding.GetEncoding("Shift_JIS") };
        }

        // '設定'シートを読み取って内容を返す
        SettingsPage ReadSettingPage(DataTable sheet)
        {
            string a = sheet.Rows[1][COL_DATA].ToString();
            string b = sheet.Rows[2][COL_DATA].ToString();
            string c = sheet.Rows[3][COL_DATA].ToString();
            string d = sheet.Rows[4][COL_DATA].ToString();
            return new SettingsPage(a, b, c, d);
        }

        // 'データ'シートからファイルを作成する
        void CreateData(DataTable sheet, SettingsPage settings, StreamWriter sr)
        {
            // 出力見本
            #region...
            // namespace Takap.Games.CheckAssets
            // {
            //     public readonly partial struct ItemInfo
            //     {
            //         /// <summary></summary>
            //         public static readonly ItemInfo Leaf = (0, "assets/icons/leaf.png");
            // 
            //         /// <summary></summary>
            //         public static readonly ItemInfo Wood = (1, "assets/icons/wood.png");
            // 
            //         /// <summary></summary>
            //         public static readonly ItemInfo Tree = (2, "assets/icons/tree.png");
            // 
            //         /// <summary>
            //         /// 全ての要素を取得します。
            //         /// </summary>
            //         public static readonly IReadOnlyList<ItemInfo> Items = new[] 
            //         {
            //             Leaf, 
            //             Wood, 
            //             Tree
            //         };
            //     }
            // }
            #endregion

            sr.WriteLine("//");
            sr.WriteLine($"// {DateTime.Now:yyyy} Takap.");
            sr.WriteLine("//");
            sr.WriteLine("");
            sr.WriteLine($"// このファイルは {DateTime.Now:yyyy/MM/dd HH:mm:ss} に自動生成されました。");
            sr.WriteLine("");
            sr.WriteLine("using System.Collections.Generic;");
            sr.WriteLine("");
            sr.WriteLine($"namespace {settings.NameSpace}");
            sr.WriteLine("{");
            sr.WriteLine("    /// <summary>");
            sr.WriteLine($"    /// {settings.Description}");
            sr.WriteLine("    /// </summary>");
            sr.WriteLine($"    public readonly partial struct {settings.ClassName}");
            sr.WriteLine("    {");

            var symbols = new List<string>();

            for (int i = 0; i < sheet.Rows.Count; i++)
            {
                if (i < SKIP_LINES)
                {
                    continue;
                }
                DataRow row = sheet.Rows[i];
                string id = row[0].ToString();
                string symbol = row[1].ToString();
                string address = row[2].ToString();
                string description = row[3].ToString();
                symbols.Add(symbol);

                sr.WriteLine($"        /// <summary>'{description}' を表します。</summary>");
                sr.WriteLine($"        public static readonly ItemInfo {symbol} = ({id}, \"{address}\");");
                sr.WriteLine("");
            }

            sr.WriteLine("        /// <summary>");
            sr.WriteLine($"        /// 全ての要素を取得します。");
            sr.WriteLine("        /// </summary>");
            sr.WriteLine("        public static readonly IReadOnlyList<ItemInfo> Items = new[] ");
            sr.WriteLine("        {");
            foreach (string symbol in symbols)
            {
                sr.WriteLine($"            {symbol},");
            }
            sr.WriteLine("        };");

            sr.WriteLine("    }");
            sr.WriteLine("}");
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        // 設定ページの読み取り結果を格納する構造体
        readonly struct SettingsPage
        {
            public readonly string FileName;
            public readonly string NameSpace;
            public readonly string ClassName;
            public readonly string Description;

            public SettingsPage(string fileName, string nameSpace, string className, string description)
            {
                FileName = fileName;
                NameSpace = nameSpace;
                ClassName = className;
                Description = description;
            }
        }
    }
}
