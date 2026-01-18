// 
// (C) 2022 Takap.
// 

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// 探索中のあるグリッド状態を表します。
    /// </summary>
    public class ASterGrid
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 親のノードを設定または取得します。
        /// </summary>
        public ASterGrid Parent { get; set; }

        /// <summary>
        /// 探索済みかどうかを設定または取得します。
        /// </summary>
        public GridType Status { get; set; }

        /// <summary>
        /// Xの座標を設定または取得します。
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Yの座標を設定または取得します。
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 実コストを設定または取得します。
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// 推定コストを設定または取得します。
        /// </summary>
        public int H { get; set; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public ASterGrid(int xindex, int yindex)
        {
            X = xindex;
            Y = yindex;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このグリッドの重みを計算します。
        /// </summary>
        public int CalcScore() => C + H;
    }

    /// <summary>
    /// アルゴリズム全体の処理状態を表します。
    /// </summary>
    public enum SearchState
    {
        /// <summary>検索中</summary>
        Searching = 0,
        /// <summary>未完了で探索が打ち切られた</summary>
        Incomplete,
        /// <summary>ゴールを発見した</summary>
        Completed,
        /// <summary>エラー発生</summary>
        Error,
    }
}
