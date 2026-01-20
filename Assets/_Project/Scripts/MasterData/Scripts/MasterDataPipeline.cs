#if UNITY_EDITOR
//
// (C) 2025 Takap.
//

using Takap.Utility;
using UnityEditor;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// マスターデータの変更を検出して更新処理を起動するためのパイプラインを表します。
    /// </summary>
    [InitializeOnLoad]
    public class MasterDataPipeline : AssetPostprocessor
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        static IAppLogger _logger;
        static IMasterDataGenerator _generator;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        static MasterDataPipeline()
        {
            _logger ??= new UnityDebugLogger();
            _generator ??= new MasterDataGeneratorFromExcel(_logger);
        }

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
    }
}
#endif