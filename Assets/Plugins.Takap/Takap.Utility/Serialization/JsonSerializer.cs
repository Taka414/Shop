// 
// (C) 2022 Takap.
//

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Takap.Utility
{
    /// <summary>
    /// Jsonに関係するSerialize機能を提供します。
    /// </summary>
    //[Obsolete("Use the JsonUtility")]
    public static class JsonSerializer
    {
        //
        // ものすごく動作速度が遅いので使用禁止
        //

        /// <summary>
        /// 指定したファイルパスからJsonを読み取りオブジェクトとして取得します。
        /// </summary>
        public static T Deserialize<T>(string loadPath)
        {
            using var sr = new StreamReader(loadPath);
            return DeserialiseFromString<T>(sr.ReadToEnd());
        }

        /// <summary>
        /// 指定したファイルパスからJsonを読み取りオブジェクトとして取得します。
        /// </summary>
        public static bool TryDeserialize<T>(string loadPath, out T obj)
        {
            obj = default;
            
            try
            {
                obj = Deserialize<T>(loadPath);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 指定したJson文字列をオブジェクトへデシリアライズします。
        /// </summary>
        public static T DeserialiseFromString<T>(string message)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            var setting = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
            };

            var serializer = new DataContractJsonSerializer(typeof(T), setting);
            return (T)serializer.ReadObject(stream);
        }

        /// <summary>
        /// 指定したオブジェクトをJson形式でファイルに保存します。
        /// </summary>
        public static void Serialize<T>(string savePath, T src)
        {
            using var sw = new StreamWriter(savePath);
            sw.Write(SerializeToString(src));
        }

        /// <summary>
        /// 指定したオブジェクトをJsonへ変換します。
        /// </summary>
        public static string SerializeToString<T>(T src)
        {
            // Serializerを使ってオブジェクトをMemoryStream に書き込み
            using var ms = new MemoryStream();
            using var sr = new StreamReader(ms);
            var setting = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
            };

            new DataContractJsonSerializer(typeof(T), setting).WriteObject(ms, src);
            ms.Position = 0;

            return sr.ReadToEnd();
        }
    }
}
