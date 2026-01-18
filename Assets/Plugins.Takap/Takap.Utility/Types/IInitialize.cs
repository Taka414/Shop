//
// (C) 2022 Takap.
//

namespace Takap
{
    /// <summary>
    /// 外部から初期化が可能なことを表します。
    /// </summary>
    public interface IInitialize
    {
        /// <summary>
        /// 初期化を実行します。
        /// </summary>
        void Initialize();
    }
}
