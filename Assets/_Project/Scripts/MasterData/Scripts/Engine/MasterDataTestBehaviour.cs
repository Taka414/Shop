//
// (C) 2026 Takap.
//

using Sirenix.OdinInspector;
using Takap.Utility;
using UnityEngine;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// 
    /// </summary>
    public class MasterDataTestBehaviour : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // マスターデータの設定
        [SerializeField] string _masterFileName = "MasterData.xlsx";

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

#if UNITY_EDITOR
        // 生成テストの実行
        [Button]
        public void TestRun()
        {
            IAppLogger logger = new UnityDebugLogger();
            MasterDataGeneratorFromExcel generator = new(logger);
            MasterDataPipelineProcessor processor = new(generator);
            processor.Process(new[] { _masterFileName });
        }
#endif
    }
}
