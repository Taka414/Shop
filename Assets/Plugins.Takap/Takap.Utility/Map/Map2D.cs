//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;

namespace Takap.Utility.Map
{
    /// <summary>
    /// 任意の型(T)の2次元配列を表します。
    /// </summary>
    public class Map2D<T>
    {
        //
        // Descriptions
        // - - - - - - - - - - - - - - - - - - - -
        #region...
        //
        // 以下モデルを想定
        //
        //     xi → → → →
        //  yi 00 01 02 03 04
        //  ↓ 10 11 12 13 14
        //  ↓ 20 21 22 23 24
        //  ↓ 30 31 32 33 34
        //  ↓ 40 41 42 43 44
        //
        // もしくはこう
        //
        // ↑ 40 41 42 43 44
        // ↑ 30 31 32 33 34
        // ↑ 20 21 22 23 24
        // ↑ 10 11 12 13 14
        // yi 00 01 02 03 04
        //    xi → → → →
        //
        #endregion

        /// <summary>
        /// このオブジェクトが内部で管理している配列を取得します
        /// （外から内容を操作してもいいけど自己責任）
        /// </summary>
        public T[] Array { get; private set; }

        /// <summary>
        /// マップの横幅を取得します。
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// マップの縦幅を取得します。
        /// </summary>
        public int Height { get; private set; }

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した位置に要素を設定または取得します。
        /// </summary>
        public T this[int xi, int yi] { get => GetItem(xi, yi); set => SetItem(xi, yi, value); }

        /// <summary>
        /// 指定した位置に要素を設定または取得します。
        /// </summary>
        public T this[Point2Di pos] { get => GetItem(pos.X, pos.Y); set => SetItem(pos.X, pos.Y, value); }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 既定の初期値でオブジェクトを新規作成します。
        /// </summary>
        public Map2D() { }

        /// <summary>
        /// マップの縦・横の大きさを指定してオブジェクトを新規作成します。
        /// </summary>
        public Map2D(int xCount, int yCount) => Init(xCount, yCount);

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した値でオブジェクトを初期化します。
        /// </summary>
        public void Init(int xCount, int yCount)
        {
            Width = xCount;
            Height = yCount;
            Array = new T[yCount * xCount];
        }

        /// <summary>
        /// 指定した配列でオブジェクトを初期化します。
        /// </summary>
        public void Init(T[] src, int width)
        {
            Array = src;
            Width = width;
            Height = src.Length / width;
        }

        /// <summary>
        /// 指定した配列でオブジェクトを初期化します。
        /// </summary>
        public void Init(T[,] src)
        {
            Width = src.GetLength(1);
            Height = src.GetLength(0);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = y * Width + x;
                    Array[index] = src[y, x];
                }
            }
        }

        /// <summary>
        /// 指定位置にIDを設定します。
        /// </summary>
        public void SetItem(int xi, int yi, T id) => Array[yi * Width + xi] = id;

        /// <summary>
        /// 指定位置のIDを取得します。
        /// </summary>
        public T GetItem(int xi, int yi) => Array[yi * Width + xi];

        /// <summary>
        /// <para>
        /// 指定した位置がマップの範囲内かどうかを判定します。
        /// true : 範囲内 / false : 範囲外
        /// </para>
        /// <para>
        /// 基本的にすべてのメソッドは範囲チェックを行わないので指定するxi, yiが範囲内かどうかは
        /// このメソッドを使用して判定もしくは外部でチェックされていることを想定します。
        /// </para>
        /// </summary>
        public bool IsIn(int xi, int yi) => !(xi < 0 || yi < 0 || xi >= Width || yi >= Height);

        /// <summary>
        /// 指定したYの行要素をすべて列挙します。
        /// </summary>
        public IEnumerable<(int x, int y, T item)> GetRow(int yi)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return (x, yi, GetItem(x, yi));
            }
        }

        /// <summary>
        /// 指定したXの列要素をすべて列挙します。
        /// </summary>
        public IEnumerable<(int x, int y, T item)> GetColumn(int xi)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return (xi, y, GetItem(xi, y));
            }
        }

        /// <summary>
        /// 指定したYの行要素に対し述語で一括で処理を行います。
        /// </summary>
        public void ForRow(int yi, Action<int/*xi*/, int/*yi*/, T/*id*/> func)
        {
            foreach ((int x, int y, T id) in GetRow(yi))
            {
                func(x, y, id);
            }
        }

        /// <summary>
        /// 指定したYの列要素に対し述語で一括で処理を行います。
        /// </summary>
        public void ForColumn(int xi, Action<int/*xi*/, int/*yi*/, T/*id*/> func)
        {
            foreach ((int x, int y, T id) in GetColumn(xi))
            {
                func(x, y, id);
            }
        }

        /// <summary>
        /// 全ての要素を列挙し述語で処理を行います。
        /// </summary>
        public void ForEach(Action<int/*xi*/, int/*yi*/, T/*id*/> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    func(x, y, GetItem(x, y));
                }
            }
        }
    }
}