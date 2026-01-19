//
// (C) 2025 Takap.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using R3;
using Sirenix.OdinInspector;
using Takap.Utility;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// マスターデータの変更を検出して更新処理を起動するためのパイプラインを表します。
    /// </summary>
    public class MasterDataPipeline : AssetPostprocessor
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        static IAppLogger _logger;
        static IMasterDataGenerator _generator;

        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            _logger ??= new UnityDebugLogger();
            _generator ??= new MasterDataGeneratorFromExcel(_logger);
            MasterDataPipelineProcessor h = new(_generator);
            h.Process(importedAssets);
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        // ドメインリロード対応
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Setup()
        {
            _logger = null;
            _generator = null;
        }
    }

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
                Log.Trace("マスターデータを更新します。");
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
                Log.Warn("マスターデータパイプラインの設定が見つかりません。");
                return null;
            }
            else if (guids.Length > 1)
            {
                Log.Warn("マスターデータパイプラインの設定が複数存在します。");
                return null;
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            var settings = AssetDatabase.LoadAssetAtPath<MasterDataPipelineSettings>(assetPath);
            if (!settings)
            {
                Log.Warn("スターデータパイプラインの設定がロードできませんでした。");
                return null;
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

            return null;
        }

        void GenerateFromExcel(Info info)
        {
            // Excelを開いて何かする
            _generator.Generate(info.MasterFilePath, info.Settings.OutputPath);
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        // 検索した結果を入れる一時的な入れ物
        record Info(string MasterFilePath, MasterDataPipelineSettings Settings);
    }

    /// <summary>
    /// マスターデータが処理できることを表します。
    /// </summary>
    public interface IMasterDataGenerator
    {
        void Generate(string excelPath, string outDir);
    }

    /// <summary>
    /// エクセルからデータを生成します。
    /// </summary>
    public class MasterDataGeneratorFromExcel : IMasterDataGenerator
    {
        // Unity上で開発するとダルすぎるので標準のVisualStudioで別途開発予定

        readonly IAppLogger _logger;
        public MasterDataGeneratorFromExcel(IAppLogger logger)
        {
            _logger = logger;
        }

        public void Generate(string excelPath, string outDir)
        {
            // 何かする
        }
    }
}
