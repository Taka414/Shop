// 
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 暗号化に必要な情報を保持します。
    /// </summary>
    public class Seed
    {
        // メモリにKeyとIVを直接持ってると安全性が低下するので元データを保持して都度生成する
        private byte[] _byteMap;
        private ushort[] _indexMap1;
        private ushort[] _indexMap2;

        /// <summary>
        /// 事前に生成したマップデータでシードを初期化します。
        /// </summary>
        public void Init(byte[] byteMap, ushort[] indexMap1, ushort[] indexMap2)
        {
            _byteMap = byteMap;
            _indexMap1 = indexMap1;
            _indexMap2 = indexMap2;
        }

        /// <summary>
        /// 指定した長さのバイナリを取得します。
        /// </summary>
        public byte[] GetArray1(int length)
        {
            // 中身の処理は同じ値を毎回返す仕組みであればなんでもいい
            byte[] key = new byte[length];
            for (int i = 0; i < key.Length; i++)
            {
                // b.binのushort[]の先頭10個のインデックスが指し示す先の
                // 数字をbyteMapから探してキーに変換する
                key[i] = (byte)~_byteMap[_indexMap1[i]];
            }
            return key;
        }

        /// <summary>
        /// 指定した長さのバイナリを取得します。
        /// </summary>
        public byte[] GetArray2(int length)
        {
            // 中身の処理は同じ値を毎回返す仕組みであればなんでもいい
            byte[] iv = new byte[length];
            for (int i = 0, p = iv.Length; i < iv.Length; i++, p--)
            {
                // b.binのushort[]の先頭10個のインデックスが指し示す先の
                // 数字をbyteMapから探してキーに変換する
                iv[i] = (byte)~_byteMap[_indexMap2[p]];
            }
            return iv;
        }

        /// <summary>
        /// オブジェクトを <see cref="AesInfo"/> に変換します。
        /// </summary>
        public AesInfo GetAesInfo()
        {
            return new AesInfo(GetArray1(AesInfo.KEY_LENGTH), GetArray2(AesInfo.IV_LENGTH));
        }
    }
}
