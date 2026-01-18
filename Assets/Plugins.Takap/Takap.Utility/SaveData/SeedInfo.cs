// 
// (C) 2025 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Takap.Utility
{
    /// <summary>
    /// 暗号化に必要な乱数マップを指定するためのコンポーネント
    /// </summary>
    public class SeedInfo : MonoBehaviour, ISeedInfo
    {
        //
        // Inspector & prps
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 乱数マップ1を取得します。
        /// </summary>
        public string Map1 => _map1;
        [SerializeField] string _map1;

        /// <summary>
        /// 乱数マップ2を取得します。
        /// </summary>
        public string Map2 => _map2;
        [SerializeField] string _map2;

        /// <summary>
        /// 乱数マップ3を取得します。
        /// </summary>
        public string Map3 => _map3;
        [SerializeField] string _map3;

        /// <summary>
        /// 初期化済みかどうかを取得します。
        /// </summary>
        [ShowInInspector, ReadOnly]
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// シードオブジェクトを取得します。
        /// </summary>
        public Seed Seed { get; private set; }

        //
        // Inject
        // - - - - - - - - - - - - - - - - - - - -

        ISaveSystem _saveSystem;
        [Inject]
        public void Construct(ISaveSystem saveSystem)
        {
            Validator.SetValueIfThrowNull(ref _saveSystem, saveSystem);
        }

        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        void Start()
        {
            // 暗号化用の処理の登録
            TextAsset a2 = Resources.Load<TextAsset>(_map1);
            TextAsset b2 = Resources.Load<TextAsset>(_map2);
            TextAsset c2 = Resources.Load<TextAsset>(_map3);

            var seed = new Seed();
            seed.Init(a2.bytes, b2.bytes.ToUshortArray(), c2.bytes.ToUshortArray());

            Seed = seed;
            IsInitialized = true;

            Log.Trace($"初期化完了: {GetType().Name}");
        }
    }
}
