//
// (C) 2022 Takap.
//

using System;
using System.Text;

namespace Takap.Utility
{
    /// <summary>
    /// 配列に対する汎用機能を提供します。
    /// </summary>
    public static class ArrayUtil
    {
        /// <summary>
        /// 指定した2次元配列を複製します。
        /// </summary>
        public static T[,] Clone<T>(T[,] src)
        {
            int h = src.GetLength(0);
            int w = src.GetLength(1);
            var map = new T[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    map[y, x] = src[y, x];
                }
            }
            return map;
        }

        /// <summary>
        /// 指定した2次元配列の位置 (x, y) から (w to h) の範囲を切り取ります。
        /// (w, h)の 指定が src の範囲を超える場合 src の範囲内で切り取りを行います。
        /// </summary>
        public static T[,] Cut<T>(T[,] src, int x, int y, int w, int h)
        {
            // コピー元の配列の大きさ
            int ymax = src.GetLength(0);
            int xmax = src.GetLength(1);

            // 入力値が範囲内に収まるかどうか
            if (x < 0 || x > xmax || y < 0 || y > ymax)
            {
                throw new ArgumentOutOfRangeException($"Parameter is out of range. src[y={ymax},x={xmax}], x={x}, y={y}");
            }
            if (w == 0 || h == 0)
            {
                throw new ArgumentException($"Invalid parameter. w={w}, h={h}");
            }

            // コピー先の配列の大きさ
            int rh = y + h <= ymax ? h : h - (y + h - ymax);
            int rw = x + w <= xmax ? w : w - (x + w - xmax);

            // 元の大きさからコピーする
            var map = new T[rh, rw];
            for (int my = 0, _y = y; my < rh; my++, _y++)
            {
                for (int mx = 0, _x = x; mx < rw; mx++, _x++)
                {
                    map[my, mx] = src[_y, _x];
                }
            }

            return map;
        }

        /// <summary>
        /// 2次元配列の中心位置 (cx, cy) から指定した半径 (rw, rh) の範囲を切り取ります。
        /// (rw, rh) が src の範囲を超える場合 src の範囲内で切り取りを行います。
        /// 
        /// 戻り値のタプルには src から切り取った範囲が設定されます。
        /// </summary>
        public static (T[,] map, Rect2Di rect) CutByCenter<T>(T[,] src, int cx, int cy, int rw, int rh)
        {
            int xmax = src.GetLength(1);
            int ymax = src.GetLength(0);

            if (cx < 0 || cx > xmax || cy < 0 || cy > ymax)
            {
                throw new ArgumentOutOfRangeException($"Parameter is out of range. src[y={ymax},x={xmax}], x={cx}, y={cy}");
            }

            // 切り取る範囲の最大・最小値を取得
            int min_x = cx - rw;
            int max_x = cx + rw;
            int min_y = cy - rh;
            int max_y = cy + rh;
            if (min_x < 0)
            {
                min_x = 0;
            }
            if (max_x >= xmax)
            {
                max_x = xmax - 1;
            }
            if (min_y < 0)
            {
                min_y = 0;
            }
            if (max_y >= ymax)
            {
                max_y = ymax - 1;
            }

            var map = new T[max_y - min_y + 1, max_x - min_x + 1];

            for (int my = 0, _y = min_y; _y <= max_y; my++, _y++)
            {
                for (int mx = 0, _x = min_x; _x <= max_x; mx++, _x++)
                {
                    map[my, mx] = src[_y, _x];
                }
            }

            return (map, new Rect2Di(min_x, max_x, min_y, max_y));
        }

        /// <summary>
        /// [デバッグ用] 指定した配列の内容を文字列に変換します。
        /// </summary>
        public static string TostringByDebug<T>(T[,] src)
        {
            var a = new StringBuilder();
            var b = new StringBuilder();
            int ymax = src.GetLength(0);
            int xmax = src.GetLength(1);
            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    b.Append($" {src[y, x]},");
                }
                a.Append(b.ToString().Trim(' ', ','));
                a.Append(Environment.NewLine);
                b.Clear();
            }
            return a.ToString();
        }

        /// <summary>
        /// 配列の中から適当に要素を一つ取り出します。
        /// </summary>
        public static T PickupOne<T>(T[] array)
        {
            if (array.Length == 0)
            {
                return default;
                //throw new InvalidOperationException("リストが空です。");
            }
            return array[UniRandom.Range(0, array.Length)];
        }
    }
}
