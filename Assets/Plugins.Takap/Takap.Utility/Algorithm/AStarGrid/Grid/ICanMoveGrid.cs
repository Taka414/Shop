// 
// (C) 2022 Takap.
// 

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// 移動可能かどうかを取得できることを表すインターフェース
    /// </summary>
    public interface ICanMoveGrid
    {
        /// <summary>
        /// 移動可能かどうかのフラグを取得します。
        /// true : 移動できる / false : 不可
        /// </summary>
        bool CanMove { get; }
    }
}