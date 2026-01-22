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
using System.Linq;
using Newtonsoft.Json;

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
        const int SkipLines = 1;

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
                DataTable sheet = reader.AsDataSet().Tables[SHEET_DATA];

                string path = Path.Combine(outDir, "Sample.cs");
                using var fs = File.Create(path);
                using var sr = new StreamWriter(fs);

                for (int i = 0; i < sheet.Rows.Count; i++)
                {
                    if (i < SkipLines)
                    {
                        continue;
                    }
                    DataRow row = sheet.Rows[i];
                    sr.WriteLine(row[1]);
                }

                fileList.Add(path);
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
    }
}
