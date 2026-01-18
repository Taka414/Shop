// 
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    // 
    // 使い方
    // - - - - - - - - - - - - - - - - - - - -
    #region...

    //public class Sample
    //{
    //    MemoryProtector _p;
    //
    //    public void Init()
    //    {
    //        // 作成毎に初期化することで起動ごと・オブジェクト毎に
    //        // 値が変わるのでハッキングを困難にする
    //
    //        if (_p != null)
    //        {
    //            // 再初期化すると値が取り出せなくなるのでチェックする
    //            _p = new MemoryProtector();
    //        }
    //    }
    //
    //    private int _id;
    //    public int ID
    //    {
    //        get => _p.Mask(_id);
    //        set => _id = _p.Mask(value);
    //    }
    //}

    #endregion

    /// <summary>
    /// データ保護機能を提供します。
    /// </summary>
    public class MemoryProtector
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private readonly long _seed;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 既定の初期値でオブジェクトを生成します。
        /// </summary>
        public MemoryProtector()
        {
            //var rand = new Random(); // これすると死ぬほど遅くなるのでNG
            // longのseedを作りたいのでintを2回作ってlongに合成してる
            _seed = (long)UniRandom.MaxRand() << 32 | (long)UniRandom.MaxRand();
        }

        /// <summary>
        /// マスク用のシート値を指定してオブジェクトを生成します（こっちはseedの扱いが乱数以外で難しいので使わない方がよさそう）
        /// </summary>
        /// <param name="seed">シード値</param>
        public MemoryProtector(long seed)
        {
            _seed = seed;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在このオブジェクトが使用しているシード値を取得します。
        /// </summary>
        public long GetSeed() => _seed;

        /// <summary>
        /// <see cref="byte"/> 型を難読化します。
        /// </summary>
        public byte Mask(byte value) => (byte)(value ^ (byte)_seed);

        /// <summary>
        /// <see cref="char"/> 型を難読化します。
        /// </summary>
        public char Mask(char value) => (char)(value ^ (char)_seed);

        /// <summary>
        /// <see cref="short"/> 型を難読化します。
        /// </summary>
        public short Mask(short value) => (short)(value ^ (short)_seed);

        /// <summary>
        /// <see cref="ushort"/> 型を難読化します。
        /// </summary>
        public ushort Mask(ushort value) => (ushort)(value ^ (ushort)_seed);

        /// <summary>
        /// <see cref="int"/> 型を難読化します。
        /// </summary>
        public int Mask(int value) => value ^ (int)_seed;

        /// <summary>
        /// <see cref="uint"/> 型を難読化します。
        /// </summary>
        public uint Mask(uint value) => value ^ (uint)_seed;

        /// <summary>
        /// <see cref="long"/> 型を難読化します。
        /// </summary>
        public long Mask(long value) => value ^ _seed;

        /// <summary>
        /// <see cref="ulong"/> 型を難読化します。
        /// </summary>
        public ulong Mask(ulong value) => value ^ (ulong)_seed;

        /// <summary>
        /// <see cref="float"/> 型を難読化します。
        /// </summary>
        public unsafe float Mask(float value)
        {
            int f = *(int*)&value ^ (int)_seed;
            return *(float*)&f;
        }

        /// <summary>
        /// <see cref="double"/> 型を難読化します。
        /// </summary>
        public unsafe double Mask(double value)
        {
            long d = *(long*)&value ^ _seed;
            return *(double*)&d;
        }

        /// <summary>
        /// <see cref="bool"/> 型を難読化します。
        /// </summary>
        public unsafe bool Mask(bool value)
        {
            int b = *(byte*)&value ^ (byte)_seed;
            return *(bool*)&b;
        }

        /// <summary>
        /// <see cref="string"/> 型を難読化します。パフォーマンスが結構悪いので大きい文字列で何度も使用するのはNG
        /// </summary>
        public unsafe string Mask(string str)
        {
            char[] _temp = new char[str.Length];

            fixed (char* pstr = str)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    _temp[i] = (char)(*(pstr + i) ^ (char)_seed);
                }
            }

            return new string(_temp);
        }
    }
}
