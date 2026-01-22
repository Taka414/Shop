#if UNITY_EDITOR
//
// (C) 2026 Takap.
//

using System.IO;
using Takap.Utility;
using UnityEditor;
using UnityEngine;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// マスターデータの更新処理を行います。
    /// </summary>
    public class MasterDataPipelineProcessor
    {
        // このクラスはUnity内から移動しない想定

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        readonly IMasterDataGenerator _generator;
        readonly ILogger _logger;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public MasterDataPipelineProcessor(IMasterDataGenerator generator)
        {
            _generator = generator;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void Process(string[] importedAssets)
        {
            Info info = GetAsset(importedAssets);
            if (info != null)
            {
                GenerateFromExcel(info);
            }
        }

        //
        // Privat Methods
        // - - - - - - - - - - - - - - - - - - - -

        Info GetAsset(string[] importedAssets)
        {
            // 設定が存在するかチェックする(最初の1つだけ)
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(MasterDataPipelineSettings)}");
            if (guids.Length == 0)
            {
                throw new GameException("マスターデータパイプラインの設定が見つかりません");
            }
            else if (guids.Length > 1)
            {
                throw new GameException("マスターデータパイプラインの設定が複数存在します");
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            var settings = AssetDatabase.LoadAssetAtPath<MasterDataPipelineSettings>(assetPath);
            if (!settings)
            {
                throw new GameException("スターデータパイプラインの設定がロードできませんでした");
            }

            // 更新されたアセットから対象が含まれているかチェックする
            foreach (string assetTempPath in importedAssets)
            {
                string fileName = Path.GetFileName(assetTempPath);
                if (string.Compare(fileName, settings.MasterDataFileName, true) == 0)
                {
                    return new(assetTempPath, settings); // 処理対象の確定
                }
            }

            return null; // 見つからない場合無視
        }

        void GenerateFromExcel(Info info)
        {
            // Excelを開いて何かする
            string[] createdFiles = _generator.Generate(info.MasterFilePath, info.Settings.OutputPath);
            foreach (var path in createdFiles)
            {
                AssetDatabase.ImportAsset(path);
            }
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        // 検索した結果を入れる一時的な入れ物
        record Info(string MasterFilePath, MasterDataPipelineSettings Settings);
    }
}
#endif