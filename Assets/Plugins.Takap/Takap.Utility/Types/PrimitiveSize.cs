//
// (C) 2022 Takap.
//

#pragma warning disable IDE1006

using System;

namespace Takap.Utility
{
    /// <summary>
    /// ジェネリックで型判定するときのシンボル定義
    /// </summary>
    public static class PrimitiveSize
    {
        /// <summary>typeof(bool) の評価結果を表します。</summary>
        public static readonly Type Bool = typeof(bool);
        /// <summary>typeof(byte) の評価結果を表します。</summary>
        public static readonly Type Byte = typeof(byte);
        /// <summary>typeof(sbyte) の評価結果を表します。</summary>
        public static readonly Type SByte = typeof(sbyte);
        /// <summary>typeof(char) の評価結果を表します。</summary>
        public static readonly Type Char = typeof(char);
        /// <summary>typeof(decimal) の評価結果を表します。</summary>
        public static readonly Type Decimal = typeof(decimal);
        /// <summary>typeof(double) の評価結果を表します。</summary>
        public static readonly Type Double = typeof(double);
        /// <summary>typeof(float) の評価結果を表します。</summary>
        public static readonly Type Float = typeof(float);
        /// <summary>typeof(int) の評価結果を表します。</summary>
        public static readonly Type Int = typeof(int);
        /// <summary>typeof(uint) の評価結果を表します。</summary>
        public static readonly Type UInt = typeof(uint);
        /// <summary>typeof(long) の評価結果を表します。</summary>
        public static readonly Type Long = typeof(long);
        /// <summary>typeof(ulong) の評価結果を表します。</summary>
        public static readonly Type ULong = typeof(ulong);
        /// <summary>typeof(short) の評価結果を表します。</summary>
        public static readonly Type Short = typeof(short);
        /// <summary>typeof(string) の評価結果を表します。</summary>
        public static readonly Type UShort = typeof(ushort);
        /// <summary>typeof(string) の評価結果を表します。</summary>
        public static readonly Type String = typeof(string);
    }
}