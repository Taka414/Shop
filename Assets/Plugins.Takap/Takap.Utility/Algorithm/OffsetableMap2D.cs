// 
// (C) 2022 Takap.
// 

using System;
using System.Collections.Generic;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// 任意の型(T)の2次元配列を管理します。
    /// オフセットが可能なのでインデックスが(-10, -10) とか、(30, 30) でも大丈夫。
    /// </summary>
    public class OffsetableMap2D<T>
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

        // 論理位置 = マイナス方向を含むインデックス
        // 物理位置 = 配列の位置

        private int _offsetx;
        private int _offsety;

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

        /// <summary>
        /// 最小のインデックスを取得します。
        /// </summary>
        public (int xi, int yi) MinIndex => (ToLogicalX(0), ToLogicalY(0));

        /// <summary>
        /// 最大のインデックスを取得します。
        /// </summary>
        public (int xi, int yi) MaxIndex => (ToLogicalX(Width - 1), ToLogicalY(Height - 1));

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 多次元配列のインデクサーにより値を設定または取得します。
        /// </summary>
        public T this[int xi, int yi] { get => GetItem(xi, yi); set => SetItem(xi, yi, value); }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 既定の初期値でオブジェクトを新規作成します。
        /// </summary>
        public OffsetableMap2D() { }

        /// <summary>
        /// マップの縦・横の大きさを指定してオブジェクトを新規作成します。
        /// </summary>
        public OffsetableMap2D(int width, int height) => Init(width, height);

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した大きさでオブジェクトを初期化します。
        /// </summary>
        public void Init(int width, int height)
        {
            Width = width;
            Height = height;
            Array = new T[height * width];
        }

        /// <summary>
        /// 指定した2次元配列でオブジェクトを初期化します。
        /// </summary>
        public void Init(T[] src)
        {
            Array = src;
            Width = src.GetLength(1);
            Height = src.GetLength(0);
        }

        /// <summary>
        /// インデックスの最小値を設定します。(-20, -40)が最小とかの指定をする。
        /// </summary>
        public void SetMinIndex(int xi, int yi)
        {
            _offsetx = xi;
            _offsety = yi;
        }

        /// <summary>
        /// 指定位置にIDを設定します。例えば(-10, -22)とかの指定
        /// </summary>
        public void SetItem(int xi, int yi, T id) => Array[ToPhysicY(yi) * Width + ToPhysicX(xi)] = id;

        /// <summary>
        /// 論理座標を使って指定位置のIDを取得します。
        /// </summary>
        public T GetItem(int xi, int yi) => Array[ToPhysicY(yi) * Width + ToPhysicX(xi)];

        // 物理座標を使って指定位置のIDを取得する
        private T GetItemLocal(int x, int y) => Array[y * Width + x];

        // 論理座標を物理座標に変換する
        public int ToPhysicX(int xi) => xi - _offsetx;
        public int ToPhysicY(int yi) => yi - _offsety;
        // 物理座標路論理座標に変換する
        public int ToLogicalX(int xi) => xi + _offsetx;
        public int ToLogicalY(int yi) => yi + _offsety;

        /// <summary>
        /// <para>
        /// 指定した座標がマップの範囲内かどうかを判定します。
        /// true : 範囲内 / false : 範囲外
        /// </para>
        /// <para>
        /// 基本的にすべてのメソッドは範囲チェックを行わないので指定するxi, yiが範囲内かどうかは
        /// このメソッドを使用して判定もしくは外部でチェックされていることを想定します。
        /// </para>
        /// </summary>
        public bool IsIn(int xi, int yi)
        {
            int x = ToPhysicX(xi);
            int y = ToPhysicY(yi);
            return !(x < 0 || y < 0 || x >= Width || y >= Height);
        }

        /// <summary>
        /// 指定したYの行要素をすべて列挙します。
        /// </summary>
        public IEnumerable<(int x, int y, T item)> GetRow(int yi)
        {
            int y = ToPhysicY(yi);
            for (int x = 0; x < Width; x++)
            {
                yield return (ToLogicalX(x), yi, GetItemLocal(x, y));
            }
        }

        /// <summary>
        /// 指定したXの列要素をすべて列挙します。
        /// </summary>
        public IEnumerable<(int x, int y, T item)> GetColumn(int xi)
        {
            int x = ToPhysicX(xi);
            for (int y = 0; y < Height; y++)
            {
                yield return (xi, ToLogicalY(y), GetItemLocal(x, y));
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
                    func(ToLogicalX(x), ToLogicalY(y), GetItemLocal(x, y));
                }
            }
        }
    }
}
