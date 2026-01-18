// 
// (C) 2025 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 暗号化に必要な乱数マップを取得できます。
    /// </summary>
    public interface ISeedInfo
    {
        /// <summary>
        /// 乱数マップ1を取得します。
        /// </summary>
        string Map1 { get; }

        /// <summary>
        /// 乱数マップ2を取得します。
        /// </summary>
        string Map2 { get; }

        /// <summary>
        /// 乱数マップ3を取得します。
        /// </summary>
        string Map3 { get; }

        /// <summary>
        /// 初期化済みかどうかを取得します。
        /// true: 初期化済み / false: 未初期化
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// シードオブジェクトを取得します。
        /// </summary>
        Seed Seed { get; }
    }
}
