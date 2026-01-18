// 
// (C) 2022 Takap.
//

using System;
using System.IO;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 指定したオブジェクトをディスク上に安全に保存します。
    /// </summary>
    public class SaveSystem : ISaveSystem
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -

        // セーブデータを保存するサブフォルダ
        private const string DIR = "Save";
        // セーブデータの拡張子
        private const string EXTENSION = ".dat";

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        readonly IAppLogger _log;
        
        // 暗号化用のシードオブジェクト
        private Seed _seed;
        
        // データ保存先ディレクトリを表します。
        readonly string BaseDir = Application.persistentDataPath + "/" + DIR;
        
        // 属性の型キャッシュ
        Type _attTypeCache;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 暗号化する際に利用するシード値を設定または取得します。
        /// </summary>
        public Seed Seed { get => _seed; set => _seed = value; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public SaveSystem(IAppLogger logger)
        {
            _log = logger;
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したオブジェクトをファイルに保存します。
        /// <para><paramref name="isBinary"/> が true の場合 暗号化バイナリ / false : JSONプレーンテキストでセーブします。</para>
        /// </summary>
        public void Save<T>(string key, T src, bool isBinary = false)
        {
            if (isBinary)
            {
                SaveToBinary(key, src);
            }
            else
            {
                SaveToJson(key, src);
            }
        }

        /// <summary>
        /// 指定したデータが存在するかどうか確認します。
        /// true: 存在する / false: 存在しない
        /// </summary>
        public bool ExistsSaveData(string key)
        {
            string path = GetPhysicalPath(key);
            return File.Exists(path);
        }

        /// <summary>
        /// ファイルに保存されているデータをロードします。
        /// <para><paramref name="isBinary"/> が true の場合 暗号化バイナリ / false : JSONプレーンテキストでロードします。</para>
        /// </summary>
        public T Load<T>(string name, bool isBinary = false)
        {
            if (isBinary)
            {
                return LoadFromBinary<T>(name);
            }
            else
            {
                return LoadFromJson<T>(name);
            }
        }

        /// <summary>
        /// Json形式の平文からオブジェクトを復元します。
        /// </summary>
        public T LoadFromJson<T>(string name)
        {
            string path = GetPhysicalPath(name);
            if (!File.Exists(path))
            {
                return default;
            }

            // 指定したパスからデータを読み取り
            string json = File.ReadAllText(path);

            _log.Trace($"Load fron {path}");

            // (1) JSON文字列 → [変換] → オブジェクト
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// バイナリ形式のファイルを読み取ってオブジェクトとしてロードします。
        /// 当然アスキー形式よりこっちのほうがサイズも速度も倍くらい良好
        /// </summary>
        public T LoadFromBinary<T>(string name)
        {
            if (_seed == null)
            {
                throw new InvalidOperationException("Not set seed.");
            }

            string path = GetPhysicalPath(name);
            if (!File.Exists(path))
            {
                return default;
            }

            // 指定したパスからデータを読み取り
            byte[] dexArray = File.ReadAllBytes(path);

            // (3) 暗号化バイト配列 → [AES復号] → 圧縮済みバイナリ配列
            byte[] decorded = AesCypher.Decrypt(_seed.GetAesInfo(), dexArray);

            // (2) 圧縮済みバイナリ配列 → [解凍+変換] → JSON文字列
            string decompedJson = Deflate.DecompressToStr(decorded);

            _log.Trace($"Load from {path}");

            // (1) JSON文字列 → [変換] → オブジェクト
            return JsonUtility.FromJson<T>(decompedJson);
        }

        /// <summary>
        /// 指定したキーに対応するデータを削除します。
        /// </summary>
        public void Delete(string name)
        {
            string path = GetPhysicalPath(name);
            if (!File.Exists(path))
            {
                _log.Trace($"Skip to delete file. {path}");
                return;
            }

            File.Delete(path);
        }

        // 
        // Non-Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// キーに対応する保存先のファイルパスを取得します。
        /// </summary>
        /// <remarks>
        /// [保存場所]
        ///   Windonws:
        ///     - C:/Users/${UserName}/AppData/LocalLow/${CompanyName}/${ProductName}
        ///   Android:
        ///     - /data/data/xxx.xxx.xxx/files
        /// </remarks>
        string GetPhysicalPath(string key)
        {
            return $"{BaseDir}/{key}{EXTENSION}";
        }

        /// <summary>
        /// Json形式の平文でファイルを保存します。
        /// </summary>
        void SaveToJson<T>(string key, T src)
        {
            DirectoryUtil.SafeCreate(BaseDir);

            string path = GetPhysicalPath(key);

            // (1) オブジェクト → Json文字列
            string json = JsonUtility.ToJson(src);

            // 別の場所にファイルを保存
            string tempPath = BaseDir + "/" + Guid.NewGuid().ToString();
            try
            {
                // 指定したパスにデータを保存
                File.WriteAllText(tempPath, json);
                File.Copy(tempPath, GetPhysicalPath(key), true);
            }
            finally
            {
                SafeDelete(tempPath);
            }

            _log.Trace($"Save to {path}");
        }

        /// <summary>
        /// Json化した後に暗号化してバイナリ形式でオブジェクトを保存します。
        /// （平文形式よりこちらの方が保存サイズも処理速度も倍くらい良好）
        /// </summary>
        void SaveToBinary<T>(string key, T src)
        {
            if (_seed == null)
            {
                throw new InvalidOperationException("Not init seed.");
            }

            DirectoryUtil.SafeCreate(BaseDir);

            string path = GetPhysicalPath(key);

            // (1) オブジェクト → Json文字列
            string json = JsonUtility.ToJson(src);

            // (2) JSON文字列 → [圧縮+変換] → 圧縮済みバイナリ配列
            byte[] compArray = Deflate.CompressFromStr(json);

            // (3) 圧縮済みバイナリ配列 → [ARS暗号化] → 暗号化バイト配列
            byte[] encArray = AesCypher.Encrypt(_seed.GetAesInfo(), compArray);

            // 別の場所にファイルを保存
            string tempPath = BaseDir + "/" + Guid.NewGuid().ToString();
            try
            {
                // 指定したパスにデータを保存
                File.WriteAllBytes(tempPath, encArray);
                File.Copy(tempPath, GetPhysicalPath(key), true);
            }
            finally
            {
                SafeDelete(tempPath);
            }

            _log.Trace($"Save to {path}");
        }

        /// <summary>
        /// ファイルがあれば削除します。
        /// </summary>
        void SafeDelete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
