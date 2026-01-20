//
// (C) 2025 Takap.
//

using System;
using System.Collections.Generic;
using System.IO;
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
        readonly IAppLogger _logger;
        public MasterDataGeneratorFromExcel(IAppLogger logger)
        {
            _logger = logger;
        }

        public string[] Generate(string excelPath, string outDir)
        {
            List<string> fileList = new();
            try
            {
                _logger.Trace("★生成開始");

                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                // 何かする

                string path = Path.Combine(outDir, "Sample.cs");
                fileList.Add(path);

                using var sr = File.Create(path);
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
    }
}
