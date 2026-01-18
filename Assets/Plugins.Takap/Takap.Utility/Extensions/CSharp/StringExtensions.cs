//
// (C) 2020 Takap
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="string"/> クラスを拡張します。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指定した n 番目の文字を大文字に変換します。
        /// </summary>
        public static string ToUpper(this string self, int no)
        {
            if (no > self.Length)
            {
                return self;
            }

            char[] _array = self.ToCharArray();
            char up = char.ToUpper(_array[no]);
            _array[no] = up;
            return new string(_array);
        }

        /// <summary>
        /// 指定した n 番目の文字を小文字に変換します。
        /// </summary>
        public static string ToLower(this string self, int no)
        {
            if (no > self.Length)
            {
                return self;
            }

            char[] _array = self.ToCharArray();
            char up = char.ToLower(_array[no]);
            _array[no] = up;
            return new string(_array);
        }

        /// <summary>
        /// この文字列を T の型に変換します。
        /// </summary>
        public static T Convert<T>(this string self, Func<string, T> converter = null)
        {
            Type type = typeof(T);

            if (type == PrimitiveSize.Bool) return (T)(object)bool.Parse(self);
            if (type == PrimitiveSize.Byte) return (T)(object)byte.Parse(self);
            if (type == PrimitiveSize.SByte) return (T)(object)sbyte.Parse(self);
            if (type == PrimitiveSize.Char) return (T)(object)char.Parse(self);
            if (type == PrimitiveSize.Decimal) return (T)(object)decimal.Parse(self);
            if (type == PrimitiveSize.Double) return (T)(object)double.Parse(self);
            if (type == PrimitiveSize.Float) return (T)(object)float.Parse(self);
            if (type == PrimitiveSize.Int) return (T)(object)int.Parse(self);
            if (type == PrimitiveSize.UInt) return (T)(object)uint.Parse(self);
            if (type == PrimitiveSize.Long) return (T)(object)long.Parse(self);
            if (type == PrimitiveSize.ULong) return (T)(object)ulong.Parse(self);
            if (type == PrimitiveSize.Short) return (T)(object)short.Parse(self);
            if (type == PrimitiveSize.UShort) return (T)(object)ushort.Parse(self);
            if (type == PrimitiveSize.String) return (T)(object)self;

            if (converter != null)
            {
                return converter(self);
            }

            throw new NotSupportedException($"値='{self}' は 型='{type.Name}' に変換できません。");
        }

        /// <summary>
        /// 末尾のゼロをいい感じに削るトリムを行います。
        /// </summary>
        public static string TrimEnd_Zero(this string self)
        {
            string _str = self.TrimEnd('0');
            if (_str[_str.Length - 1] == '.')
            {
                _str += "0";
            }
            return _str;
        }
    }
}