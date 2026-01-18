// 
// (C) 2022 Takap.
// 

using System.Collections.Generic;
using System.Text;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// サイズが変更できるグリッド
    /// </summary>
    public class ResizeableGrid<T>
    {
        //
        // こういうグリッドインデックスを想定して実装
        //
        //  y
        //  3
        //  2
        //  1
        //  0 1 2 3 x
        //

        // 内部リスト
        private readonly List<List<T>> _list = new List<List<T>>();

        private int _xioffset;
        private int _yioffset;

        /// <summary>
        /// 最小のグリッドサイズでオブジェクトを初期化します。
        /// </summary>
        public ResizeableGrid()
        {
            _list.Add(new List<T>());
            _list[0].Add(default);
        }

        /// <summary>
        /// 初期サイズを指定してオブジェクトを初期化します。
        /// </summary>
        public ResizeableGrid(int w, int h)
        {
            for (int y = 0; y < h; y++)
            {
                var _tmp = new List<T>();
                _list.Add(_tmp);
                for (int x = 0; x < w; x++)
                {
                    var item = default(T);
                    _tmp.Add(item);
                }
            }
        }

        /// <summary>
        /// 現在のグリッドのサイズ高さを取得します。
        /// </summary>
        public int Hieght => _list.Count;

        /// <summary>
        /// 現在のグリッドの横幅を取得します。
        /// </summary>
        public int Width => _list.Count == 0 ? 0 : _list[0].Count;

        /// <summary>
        /// 指定された位置に値を設定ます。
        /// </summary>
        public void SetValue(int xi, int yi, T v)
        {
            // 管理領域の範囲外なら拡張する
            (int xi, int yi) min = MinTLGridIndex;
            (int xi, int yi) max = MaxBRGridIndex;
            if (xi > max.xi)
            {
                AppendRight(xi - max.xi);
            }
            else if (xi < min.xi)
            {
                AppendLeft(min.xi - xi);
            }

            if (yi > max.yi)
            {
                AppendTop(yi - max.yi);
            }
            else if (yi < min.yi)
            {
                AppendButtom(min.yi - yi);
            }

            _list[GetYir(yi)][GetXir(xi)] = v;
        }

        /// <summary>
        /// 指定された位置から値を取得します。
        /// </summary>
        public T GetValue(int x, int y)
        {
            return _list[GetYir(y)][GetXir(x)];
        }

        /// <summary>
        /// 左上(=最小)のグリッドインデックスを取得します。
        /// </summary>
        public (int xi, int yi) MinTLGridIndex => (GetXi(0), GetYi(0));

        /// <summary>
        /// 右下(=最大)のグリッドインデックスを取得します。
        /// </summary>
        public (int xi, int yi) MaxBRGridIndex => (GetXi(_list[0].Count - 1), GetYi(_list.Count - 1));

        /// <summary>
        /// 左側に列を追加します。
        /// </summary>
        public void AppendLeft(int size)
        {
            for (int i = 0; i < size; i++)
            {
                _xioffset += 1;
                foreach (List<T> row in _list)
                {
                    row.Insert(0, default);
                }
            }
        }

        /// <summary>
        /// 右側に列を追加します。
        /// </summary>
        public void AppendRight(int size)
        {
            for (int i = 0; i < size; i++)
            {
                foreach (List<T> row in _list)
                {
                    row.Add(default);
                }
            }
        }

        /// <summary>
        /// 上側に行を追加します。
        /// </summary>
        public void AppendTop(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var _tmp = new List<T>();
                for (int x = 0; x < _list[0].Count; x++)
                {
                    _tmp.Add(default);
                }
                _list.Add(_tmp);
            }
        }

        /// <summary>
        /// 下側に行を追加します。
        /// </summary>
        public void AppendButtom(int size)
        {
            for (int i = 0; i < size; i++)
            {
                _yioffset += 1;
                var _tmp = new List<T>();
                for (int x = 0; x < _list[0].Count; x++)
                {
                    _tmp.Add(default);
                }
                _list.Insert(0, _tmp);
            }
        }

        // 現在管理中のインデックスを出力する
        public string PrintIndex()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < _list.Count; y++)
            {
                List<T> row = _list[y];
                for (int x = 0; x < row.Count; x++)
                {
                    sb.Append($"({GetXi(x)} {GetYi(y)}), ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // 現在管理中の値を出力する
        public string PrintValue()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < _list.Count; y++)
            {
                List<T> row = _list[y];
                for (int x = 0; x < row.Count; x++)
                {
                    sb.Append($"{_list[y][x]}, ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // リストのインデックスを取得する(外 -> 中)
        private int GetXi(int x) => x - _xioffset;
        private int GetYi(int y) => y - _yioffset;
        //// 実際のインデクスを取得する(中 -> 外)
        private int GetXir(int x) => x + _xioffset;
        private int GetYir(int y) => y + _yioffset;
    }
}
