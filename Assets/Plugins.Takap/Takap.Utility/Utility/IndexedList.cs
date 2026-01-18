//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// Listから機能を大幅に制限する代わりに特定の条件下で高効率のデータ管理を行うクラス
    /// </summary>
    public class IndexedList<T> where T : class
    {
        // 
        // 制約:
        // 1) この実装は同じ値やオブジェクトが複数回追加されることを想定していない
        // 2) あらかじめ一意と判明しているデータの追加/削除/列挙を高速に行うことができる
        // 
        // 備考:
        // 1) 低容量時は配列操作のみ、大容量時はテーブルを使ったアルゴリズムに切り替えを行って高速化している
        // 2) 環境によってアルゴリズムの切り替え閾値が多少異なるがデスクトップPCで200前後で切り替えると効果的
        // 3) 特に件数が増えたときに List の Remove が非常に重たい件を解消できる
        // 

        //
        // Constant
        // - - - - - - - - - - - - - - - - - - - -

        // 初期配列サイズ
        private const int DEFAULT_ARRAY_SIZE = 1024;

        // アルゴリズムを切り替える閾値
        private const int SWITCH_THRESHOLD = 200;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 現在の要素の数
        int _size = 0;

        // アップデート対象の要素を記録する配列
        private T[] _array;

        // 要素の位置を記録するテーブル
        private readonly Dictionary<T, int> _arrayPositionTable = new(DEFAULT_ARRAY_SIZE);

        // 最小の配列のサイズ
        readonly int _minArraySize = DEFAULT_ARRAY_SIZE;

        // 低容量/大容量用アルゴを切り替える閾値
        readonly int _swithThreshold = SWITCH_THRESHOLD;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在管理中の要素数を取得します。
        /// </summary>
        public int Count => _size;

        /// <summary>
        /// 内部バッファーのサイズを取得します。
        /// </summary>
        public int Capacity
        {
            get => _array.Length;
            set
            {
                if (value <= _array.Length)
                {
                    return;
                }

                if (value <= _minArraySize)
                {
                    return;
                }

                int newSize = value.GetNearPow2();
                Array.Resize(ref _array, newSize);
            }
        }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public IndexedList()
        {
            _array = new T[DEFAULT_ARRAY_SIZE];
            _minArraySize = _array.Length;
            _swithThreshold = SWITCH_THRESHOLD;
        }

        public IndexedList(int capacity, int swithThreshold = SWITCH_THRESHOLD)
        {
            if (capacity < DEFAULT_ARRAY_SIZE)
            {
                capacity = DEFAULT_ARRAY_SIZE;
            }
            _array = new T[capacity];
            _minArraySize = _array.Length;
            _arrayPositionTable.EnsureCapacity(capacity);

            if (swithThreshold < 100)
            {
                swithThreshold = 100; // どんなに小さくても50未満は高速アルゴを使う
            }
            _swithThreshold = swithThreshold;
        }

        //
        // Operators
        // - - - - - - - - - - - - - - - - - - - -

        public T this[int i] => _array[i];

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// オブジェクトを管理に追加します。
        /// </summary>
        public void Add(in T item)
        {
            if (_array.Length == _size)
            {
                Array.Resize(ref _array, _size * 2); // 最大サイズの場合倍の大きさに広げる
            }
            _array[_size] = item;

            if (_size > _swithThreshold)
            {
                // 大容量アルゴ
                _arrayPositionTable.Add(item, _size);
            }

            if (_size == _swithThreshold)
            {
                _arrayPositionTable.Clear();
                int len = _size + 1;
                for (int i = 0; i < len; i++)
                {
                    _arrayPositionTable.Add(_array[i], i); // 切り替えポイントならキャッシュ構築
                }
            }

            _size++;
        }

        /// <summary>
        /// オブジェクトを管理から削除します。
        /// </summary>
        public void Remove(T item)
        {
            //if (item == null)
            //{
            //    return;
            //}

            if (_size > _swithThreshold)
            {
                // 大容量アルゴ
                int itemPosition = _arrayPositionTable[item];
                _array[itemPosition] = null;
                _arrayPositionTable.Remove(item);

                if (--_size == 0)
                {
                    return; // 管理要素が存在しない
                }

                if (itemPosition != _size)
                {
                    T tmp = _array[itemPosition] = _array[_size]; // 末尾を空いた要素につめる
                    _array[_size] = null;
                    _arrayPositionTable[tmp] = itemPosition;
                }
            }
            else
            {
                // 低容量アルゴ
                int index = IndexOf(item);
                if (index == -1)
                {
                    return;
                }
                _array[index] = _array[--_size];
                _array[_size] = null;
            }
        }

        private int IndexOf(T item)
        {
            for (int i = 0; i < _size; ++i)
            {
                if (_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 現在管理中の要素を列挙します。
        /// </summary>
        public ReadOnlySpan<T> GetArray()
        {
            return new ReadOnlySpan<T>(_array, 0, _size); // 有効範囲だけを切り出す
        }

        /// <summary>
        /// 管理中の要素を全て破棄します。
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = null;
            }
            _arrayPositionTable.Clear();
            _size = 0;
        }

        /// <summary>
        /// 管理している配列のサイズを調整します。
        /// </summary>
        public void TrimExcess()
        {
            if (_size > _minArraySize)
            {
                return; // 最小値より小さい場合縮小しない
            }

            int newSize = _size.GetNearPow2();
            if (newSize < _minArraySize)
            {
                newSize = _minArraySize; // 最小サイズ以下には縮小しない
            }

            if (_array.Length == newSize)
            {
                return;
            }

            Array.Resize(ref _array, newSize);
        }

        //
        // Other Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したオブジェクトが管理配列のどの位置にあるかを取得します。
        /// </summary>
        private int FindIndex(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
