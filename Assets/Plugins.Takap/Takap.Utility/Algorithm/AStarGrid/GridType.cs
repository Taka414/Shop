// 
// (C) 2022 Takap.
// 

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// グリッドの探索状態を表します。
    /// </summary>
    public enum GridType
    {
        /// <summary>何もしていない</summary>
        None = 0,
        /// <summary>探索済み</summary>
        Open,
        /// <summary>検索除外</summary>
        Close,
        /// <summary>検索対象外</summary>
        Exclude,
    }
}
