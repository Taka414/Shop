//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 最大・最小の2点の組み合わせを表します。
    /// </summary>
    public readonly struct Rect2Di
    {
        public int XMin { get; }
        public int YMin { get; }
        public int XMax { get; }
        public int YMax { get; }
        public Rect2Di(int xmin, int ymin, int xmax, int ymax)
        {
            XMin = xmin;
            YMin = ymin;
            XMax = xmax;
            YMax = ymax;
        }
    }
}
