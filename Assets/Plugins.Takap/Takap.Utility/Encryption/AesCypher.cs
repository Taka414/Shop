//
// (C) 2022 Takap.
//

using System.Security.Cryptography;

namespace Takap.Utility
{
    /// <summary>
    /// AES暗号化を行います。
    /// </summary>
    public static class AesCypher
    {
        // コア機能

        /// <summary>
        /// 指定したパラメーターでバイト列を暗号化します。
        /// </summary>
        public static byte[] Encrypt(AesInfo info, byte[] buffer)
        {
            using AesManaged aes = info;
            using ICryptoTransform e = aes.CreateEncryptor();
            return e.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 指定したパラメーターでバイト列を復号します。
        /// </summary>
        public static byte[] Decrypt(AesInfo info, byte[] buffer)
        {
            using AesManaged aes = info;
            using ICryptoTransform e = aes.CreateDecryptor();
            return e.TransformFinalBlock(buffer, 0, buffer.Length);
        }
    }
}
