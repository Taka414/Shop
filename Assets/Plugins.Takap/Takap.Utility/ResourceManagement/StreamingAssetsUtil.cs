//
// (C) 2022 Takap.
//

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Takap.Utility;
using UnityEngine;
using UnityEngine.Networking;

namespace Takap.Utility
{
    /// <summary>
    /// StreamingAssetsからデータを読み取る機能を提供します。
    /// </summary>
    public static class StreamingAssetsUtil
    {
        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        // Assets > StreamingAssets にフォルダを作成して中にファイルが入ってる事

        // バイナリとテキスト以外は対象外
        // TextureやAudioとかのアセットは読み取り方が違うのでここでは扱わない

        /// <summary>
        /// StreamingAssetsからバイナリデータを読み取ります。
        /// </summary>
        public static async UniTask<byte[]> ReadStreamingAssetsData(string path, CancellationToken token)
        {
            string realPath = Application.streamingAssetsPath + "/" + path.TrimStart('/');
#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
            return await ReadDataSpesific(realPath, token);
#else
            return await ReadData(realPath, token);
#endif
        }

        /// <summary>
        /// StreamingAssetsからテキストを読み取ります。
        /// </summary>
        public static async UniTask<string> ReadStreamingAssetsText(string path, CancellationToken token)
        {
            string realPath = Application.streamingAssetsPath + "/" + path.TrimStart('/');
            Log.Trace($"Application.streamingAssetsPath={Application.streamingAssetsPath}");
#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
        return await ReadTextSpesific(realPath, token);
#else
            return await ReadText(realPath, token);
#endif
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        // Android + WebGL向けのバイナリ読み取り
        private static async UniTask<byte[]> ReadDataSpesific(string path, CancellationToken token)
        {
            var ret = await ReadSpesific(path, token);
            return ret.data;
        }

        // Android + WebGL向けのテキスト読み取り
        private static async UniTask<string> ReadTextSpesific(string path, CancellationToken token)
        {
            var ret = await ReadSpesific(path, token);
            return ret.text;
        }

        // Android + WebGL向けの読み取り処理
        private static async UniTask<DownloadHandler> ReadSpesific(string path, CancellationToken token)
        {
            // WebRequestを使って読み取る
            var req = UnityWebRequest.Get(path);
            await req.SendWebRequest().ToUniTask(cancellationToken: token);
            if (req.result != UnityWebRequest.Result.Success) // 
            {
                // エラーどう処理するかは各自決める
                throw new UnityException($"処理に失敗しました。path={path}, code={req.result}");
            }
            return req.downloadHandler;
        }

        // 通常の環境のバイナリ読み取り
        private static async ValueTask<byte[]> ReadData(string path, CancellationToken token)
        {
            // 普通のファイルの読み書き
            using var fs = new FileStream(path, FileMode.Open);
            var array = new byte[fs.Length];
            await fs.ReadAsync(array, 0, (int)fs.Length, token);
            return array;
        }

        // 通常の環境のバイナリ読み取り(UTF-8)
        private static async ValueTask<string> ReadText(string path, CancellationToken _)
        {
            // 普通のファイルの読み書き
            using var fs = new FileStream(path, FileMode.Open);
            using var sr = new StreamReader(fs);
            return await sr.ReadToEndAsync();
        }
    }
}
