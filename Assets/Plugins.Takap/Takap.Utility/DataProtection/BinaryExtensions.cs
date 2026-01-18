// 
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// byte配列の機能を拡張します。
    /// </summary>
    public static class BinaryExtensions
    {
        /// <summary>
        /// byte配列をushort配列に変換します。
        /// </summary>
        public static ushort[] ToUshortArray(this byte[] array)
        {
            var span = new ReadOnlySpan<byte>(array);
            if (array.Length % 2 != 0)
            {
                Console.WriteLine($"配列の長さは2で割り切れる必要があります。len={array.Length}");
            }
            int len = array.Length / 2;
            ushort[] _array = new ushort[len];
            for (int i = 0; i < _array.Length; i++)
            {
                ushort value = BitConverter.ToUInt16(span.Slice(i * 2, 2));
                _array[i] = value;
            }
            return _array;
        }
    }
}
