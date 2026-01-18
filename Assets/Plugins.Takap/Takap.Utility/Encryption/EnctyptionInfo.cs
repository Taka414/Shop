//
// (C) 2022 Takap.
//

using System;
using System.Security.Cryptography;

namespace Takap.Utility
{
    /// <summary>
    /// AES暗号化を行うときに指定するパラメータを格納するコンテナを表します。
    /// </summary>
    public readonly struct AesInfo
    {
        public const int KEY_LENGTH = 32;
        public const int IV_LENGTH = 16;

        /// <summary>
        /// 暗号化キーサイズを設定または取得します。256bit固定です。
        /// </summary>
        public readonly int KeySize;

        /// <summary>
        /// 暗号化ブロックサイズを設定または取得します。128bit固定です。
        /// </summary>
        public readonly int BlockSize;

        /// <summary>
        /// 暗号化モードを取得します。CBC固定です。
        /// </summary>
        public readonly CipherMode Mode;

        /// <summary>
        /// パディングモードを設定または取得します。
        /// </summary>
        public readonly PaddingMode Padding;

        /// <summary>
        /// 初期ベクトルを設定または取得します。
        /// </summary>
        public readonly byte[] IV;

        /// <summary>
        /// 暗号化キーを設定または取得します。
        /// </summary>
        public readonly byte[] Key;

        public AesInfo(byte[] key, byte[] iv)
        {
            if (key.Length != 32) throw new ArgumentException("key length is not 32");
            if (iv.Length != 16) throw new ArgumentException("iv length is not 16");
            
            Key = key;
            IV = iv;
            
            KeySize = 256;
            BlockSize = 128;
            Mode = CipherMode.CBC;
            Padding = PaddingMode.PKCS7;
        }

        // AesManagedへの暗黙の型変換
        // ** 戻り値の型はIDisposableなのでusingで囲む or 開放処理を記述する事
        public static implicit operator AesManaged(AesInfo info)
        {
            return new AesManaged
            {
                KeySize = info.KeySize,
                BlockSize = info.BlockSize,
                Mode = info.Mode,
                IV = info.IV,
                Key = info.Key,
                Padding = info.Padding
            };
        }
    }
}
