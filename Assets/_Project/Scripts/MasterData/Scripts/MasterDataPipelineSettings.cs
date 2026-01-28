//
// (C) 2026 Takap.
//

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;
using R3;
using Sirenix.OdinInspector;
using Takap.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Takap.Games.CheckAssets
{
    /// <summary>
    /// マスターデータ更新用の設定を表します。
    /// </summary>
    [CreateAssetMenu(fileName = "MasterDataPipelineSettings", menuName = "Settings/MasterDataPipelineSettings")]
    public class MasterDataPipelineSettings : ScriptableObject
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // マスターデータ生成が有効かどうかの設定
        // true: 有効 / false: 無効
        [SerializeField] bool _isEnabled;
        public bool IsEnabled => _isEnabled;

        // マスターデータのファイル名
        [SerializeField]
        [EnableIf(nameof(_isEnabled))]
        string _masterDataFileName = "MasterData.xlsx";
        public string MasterDataFileName => _masterDataFileName;

        // 生成結果を保存するフォルダパス
        [SerializeField]
        [EnableIf(nameof(_isEnabled))]
        string _outputPath = "Assets/Generated/Data";
        public string OutputPath => _outputPath;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Props & Events
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        //void Awake()
        //{
        //
        //}
    
        //void Start()
        //{
        //
        //}

        //void Update()
        //{
        //
        //}
    
        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

    }
}
