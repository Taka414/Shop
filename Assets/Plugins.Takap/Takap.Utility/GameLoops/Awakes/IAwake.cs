//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 標準のAwakeの代わりに呼び出すAwake操作を表します。
    /// </summary>
    public interface IAwake
    {
        /// <summary>
        /// <see cref="AwakeSetting"/> から呼び出される順序が決まったAwakeを行います。
        /// </summary>
        void LocalAwake();
    }
}
