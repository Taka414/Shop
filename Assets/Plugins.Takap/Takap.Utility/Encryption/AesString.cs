//
// (C) 2022 Takap.
//

using System;
using System.Text;

namespace Takap.Utility
{
    /// <summary>
    /// 文字列を対象にしたAES暗号化を行います。
    /// </summary>
    public static class AesString
    {
        /// <summary>
        /// 任意の文字列を暗号化文字列化バイト列にエンコードします。
        /// </summary>
        /// <remarks>
        /// '11-12-1A-2F' のようなハイフンつなぎの文字列に変換します。
        /// </remarks>
        public static string Encrypt(AesInfo info, string str)
        {
            byte[] barray = AesCypher.Encrypt(info, Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(barray);
        }

        /// <summary>
        /// 特定の形式の暗号化済みの文字列化バイト列を元の文字列にデコードします。
        /// </summary>
        /// <remarks>
        /// '11-12-1A-2F' のようなハイフンつなぎの文字列を元の文字列に変換します。
        /// </remarks>
        public static string Decrypt(AesInfo info, string str)
        {
            // 00-11-22 のようなハイフンつなぎの文字列を byte[] に変換
            string[] hexChars = str.Split('-');
            byte[] src = new byte[hexChars.Length];
            for (int i = 0; i < hexChars.Length; i++)
            {
                src[i] = Convert.ToByte(hexChars[i], 16); // 16進文字を byte に変換
            }

            byte[] barray = AesCypher.Decrypt(info, src);
            return Encoding.UTF8.GetString(barray);
        }
    }
}
