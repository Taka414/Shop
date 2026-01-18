// 
// (C) 2022 Takap.
// 

using System.Collections.Generic;
using Takap.Utility.Map;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// A-Starで検索する範囲のマップとパラメーターを表します。
    /// </summary>
    public class AStarSource
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 検索対象のマップを取得します。
        /// </summary>
        public Map2DGrid Map { get; private set; }

        /// <summary>
        /// 検索開始位置を取得します。
        /// </summary>
        public Point2Di StartPos { get; private set; } = new Point2Di();

        /// <summary>
        /// 検索終了位置を取得します。
        /// </summary>
        public Point2Di EndPos { get; private set; } = new Point2Di();

        /// <summary>
        /// 検索時の最大コストを設定または取得します。
        /// </summary>
        public int MaxCost { get; set; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 対象のマップを指定してオブジェクトを初期化します。
        /// </summary>
        public AStarSource(Map2DGrid map)
        {
            Map = map;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 検索対象のマップと検索開始/終了位置を指定してオブジェクトを初期化します。
        /// </summary>
        public void Init(Point2Di s, Point2Di e)
        {
            StartPos = s;
            EndPos = e;
        }

        /// <summary>
        /// 通行可能かどうかを取得します。
        /// true : 通行可能 / false : 不可
        /// </summary>
        public bool CanMove(int xindex, int yindex)
        {
            GridSource item = Map.GetItem(xindex, yindex);
            return item.CanMove;
        }

        /// <summary>
        /// 通行可能な上下左右4タイルを取得します。
        /// </summary>
        public List<(int xi, int yi)> GetAround4Tiles(int xindex, int yindex)
        {
            var list = new (int x, int y)[]
            {
                (xindex, yindex - 1),
                (xindex, yindex + 1),
                (xindex - 1, yindex),
                (xindex + 1, yindex),
            };

            var retList = new List<(int xi, int yi)>();
            for (int i = 0; i < list.Length; i++)
            {
                (int x, int y) = list[i];
                if (Map.IsIn(x, y) && Map.GetItem(x, y).CanMove)
                {
                    retList.Add((x, y));
                }
            }

            return retList;
        }

        /// <summary>
        /// [デバッグ用] マップの内容をコンソールに出力します。
        /// </summary>
        public void PrintMap()
        {
            //Console.WriteLine(this.Array.ToStringByDebug());
            //Console.WriteLine($"s=({this.StartPos.X}, {this.StartPos.Y}), e=({this.EndPos.X}, {this.EndPos.Y})");
            //Console.WriteLine($"X-In={this.IsInMapBeginPos()}, Y-In{this.IsInMapEndPos()}");
        }
    }
}
