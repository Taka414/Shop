//
// (C) 2022 Takap.
//

namespace Takap.Utility.Map
{
    //
    // 一番よく使うと思われるので事前に定義しておく
    //

    /// <summary>
    /// <see cref="int"/> 型の2次元配列を管理します。
    /// </summary>
    public class Map2Di : Map2D<int>
    {
        public Map2Di()
        {
        }
        public Map2Di(int width, int hegiht) : base(width, hegiht)
        {
        }
    }
}