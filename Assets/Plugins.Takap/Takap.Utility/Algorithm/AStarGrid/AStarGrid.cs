// 
// (C) 2022 Takap.
// 

using System;
using System.Collections.Generic;
using Takap.Utility.Map;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// A-Starの経路探索アルゴリズムを表します。
    /// </summary>
    public class AStarGrid
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 検索範囲内のグリッド
        private Map2D<ASterGrid> _grid;
        // オープン済みのノードを記憶するリスト
        private readonly List<ASterGrid> _openList = new List<ASterGrid>();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 開始時に指定したオブジェクトを取得します。
        /// </summary>
        public AStarSource Source { get; private set; }

        // Sourceに対するエイリアス用のプロパティ
        public int StartX => Source.StartPos.X;
        public int StartY => Source.StartPos.Y;
        public int MaxCost => Source.MaxCost;
        public int EndX => Source.EndPos.X;
        public int EndY => Source.EndPos.Y;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 検索対象の情報を指定してオブジェクトを新規に作成します。
        /// </summary>
        public AStarGrid(AStarSource source)
        {
            Source = source;
            _grid = new Map2D<ASterGrid>(source.Map.Width, source.Map.Height);

            // Maxコストで行ける範囲の計算
            int minX = StartX - MaxCost;
            int maxX = StartX + MaxCost;
            int minY = StartY - MaxCost;
            int maxY = StartY + MaxCost;

            // 探索対象のデータ構造を作成する
            //var grid = new AsterGrid[this.Source.Map.Height, this.Source.Map.Width];
            for (int y = 0; y < _grid.Height; y++)
            {
                for (int x = 0; x < _grid.Width; x++)
                {
                    if (x > maxX || x < minX || y > maxY || y < minY)
                    {
                        _grid.SetItem(x, y, null); // コストオーバーな範囲外は最初から検索対象外にする
                    }
                    else
                    {
                        var _grid = new ASterGrid(x, y);
                        if (!Source.CanMove(x, y))
                        {
                            _grid.Status = GridType.Exclude;
                        }

                        this._grid.SetItem(x, y, _grid);
                    }
                }
            }

            Init();
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// アルゴリズムを初期化します。
        /// </summary>
        public void Init()
        {
            // 最初のタイルを決定して周囲を開く
            ASterGrid grid = _grid.GetItem(StartX, StartY);
            grid.C = 0;
            grid.H = CalcH(StartX, StartY);
            grid.Status = GridType.Open;
            _openList.Add(grid);
        }

        /// <summary>
        /// ルート検索を全て実行します。
        /// </summary>
        public SearchState NextAll()
        {
            SearchState state;
            while (true)
            {
                state = NextOne();
                if (state != SearchState.Searching)
                {
                    break;
                }
            }

            return state;
        }

        /// <summary>
        /// ルート検索を一回実行します。
        /// </summary>
        public SearchState NextOne()
        {
            // 最小コストのグリッドを基準ノードとして選択
            ASterGrid baseGrid = GetMinScoreGrid();
            if (baseGrid == null)
            {
                return SearchState.Incomplete; // 一つもOpenするものがなくなった未到達終了
            }

            // Openリストからオブジェクト削除して対象外にする
            baseGrid.Status = GridType.Close;
            _openList.Remove(baseGrid);

            if (baseGrid.C > MaxCost * 2)
            {
                return SearchState.Searching; // コストオーバーなら処理を打ち切って終了
            }

            if (baseGrid.X == EndX && baseGrid.Y == EndY)
            {
                return SearchState.Completed;
            }

            // 周囲のタイルをオープンする
            //var adjacentGridList = GameData.ground().getAdjacent4Grid(baseGrid.x, baseGrid.y);
            List<(int xi, int yi)> arounds = Source.GetAround4Tiles(baseGrid.X, baseGrid.Y);
            foreach ((int xi, int yi) in arounds)
            {
                ASterGrid _grid = this._grid.GetItem(xi, yi);
                if (_grid == null || _grid.Status != GridType.None)
                {
                    continue;
                }
                _grid.C = baseGrid.C + 1; // 移動するたびにコストが増加
                _grid.H = CalcH(_grid.X, _grid.Y);
                _grid.Status = GridType.Open;
                _grid.Parent = baseGrid;

                _openList.Add(_grid);

                if (_grid.X == EndX && _grid.Y == EndY)
                {
                    return SearchState.Completed;
                }
            }

            return SearchState.Searching;
        }

        /// <summary>
        /// 検索済みのルートを取得します。 
        /// </summary>
        public List<Point2Di> GetRoute()
        {
            var retList = new List<Point2Di>();
            ASterGrid gridInfo = _grid.GetItem(EndX, EndY);
            while (true)
            {
                retList.Add(new Point2Di(gridInfo.X, gridInfo.Y));
                if (gridInfo.Parent == null)
                {
                    retList.Reverse();
                    return retList;
                }

                gridInfo = gridInfo.Parent;
            }
        }

        /// <summary>
        /// 推定コストを計算します。
        /// </summary>
        public int CalcH(int xindex, int yindex) => Math.Abs(EndX - xindex) + Math.Abs(EndY - yindex);

        /// <summary>
        /// Open済みのノードから一番コストの小さいノードを取得する 
        /// </summary>
        public ASterGrid GetMinScoreGrid()
        {
            if (_openList.Count == 0)
            {
                return null;
            }

            // 一番小さいコストのノード
            ASterGrid _gridInfo = _openList[0];

            for (int i = 1; i < _openList.Count; i++)
            {
                ASterGrid _tempGridInfo = _openList[i];

                if (_gridInfo.CalcScore() > _tempGridInfo.CalcScore())
                {
                    _gridInfo = _tempGridInfo;
                }
            }

            return _gridInfo;
        }
    }
}
