//
// (C) 2022 Takap.
//

using Takap.Utility.Algorithm;

namespace Takap.Utility.Map
{
    /// <summary>
    /// <see cref="GridSource"/> 型の2次元配列を管理します。
    /// </summary>
    public class Map2DGrid : Map2D<GridSource>
    {
        public Map2DGrid()
        {
        }
        public Map2DGrid(int width, int hegiht) : base(width, hegiht)
        {
        }
    }
}