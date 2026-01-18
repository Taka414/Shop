//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 2つの整数の組を表す
    /// </summary>
    public class Point2Di
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public int X { get; set; }
        public int Y { get; set; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public Point2Di() { }

        public Point2Di(int x, int y)
        {
            X = x;
            Y = y;
        }

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        public static bool operator ==(Point2Di a, Point2Di b) { return a.X == b.X && a.Y == b.Y; }
        public static bool operator !=(Point2Di a, Point2Di b) { return a.X != b.X || a.Y != b.Y; }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public uint CreateKey() => CreateKey(this);

        public static uint CreateKey(Point2Di pos) => (uint)((pos.Y << 16) + pos.X);

        public override bool Equals(object obj)
        {
            return obj is Point2Di di &&
                   X == di.X &&
                   Y == di.Y;
        }

        public override int GetHashCode() // VisualStudio2019おすすめ設定のまま(Dictionaryのキーに入れる想定はしていないのでこのままにしとく
        {
            int hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
