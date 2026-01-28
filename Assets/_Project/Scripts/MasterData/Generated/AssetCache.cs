//
// (C) 2026 Takap.
//

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Takap.Games.Shopping
{
    /// <summary>
    /// Spriteを管理するクラス
    /// </summary>
    public class SpriteCache : AssetCache<Sprite> { }

    /// <summary>
    /// Addressablesからのデータを管理します。
    /// </summary>
    /// <typeparam name="T">Unity内のオブジェクトの種類を表す</typeparam>
    public abstract class AssetCache<T> where T : UnityEngine.Object
    {
        Dictionary<uint, Entry> _cache = new Dictionary<uint, Entry>();

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// リソースのロードのみを行います。
        /// </summary>
        public void Preload(ItemInfo info)
        {
            if (_cache.TryGetValue(info, out Entry e))
            {
                return; // 既にロード済み
            }
            AsyncOperationHandle<T> h = Addressables.LoadAssetAsync<T>(info.Address);
            _cache[info] = new Entry(info.Address, h);
        }

        /// <summary>
        /// オブジェクトをAddressablesからロードします。
        /// </summary>
        public async UniTask<T> GetResourceAsync(ItemInfo info)
        {
            if (_cache.TryGetValue(info, out Entry e))
            {
                if (e.Handle.IsValid() && e.Handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return e.Handle.Result;
                }
                return await e.Handle.ToUniTask();
            }

            Preload(info);
            return await _cache[info].Handle.ToUniTask();
        }

        /// <summary>
        /// ロード済みのオブジェクトを解放します。
        /// </summary>
        public void Release(ItemInfo info)
        {
            if (!_cache.TryGetValue(info, out var e))
            {
                return;
            }

            if (e.Handle.IsValid())
            {
                Addressables.Release(e.Handle);
            }
            _cache.Remove(info);
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        // キャッシュに使用する一時オブジェクト
        readonly struct Entry
        {
            public readonly string Address;
            public readonly AsyncOperationHandle<T> Handle;
            public Entry(string address, AsyncOperationHandle<T> handle)
            {
                Address = address;
                Handle = handle;
            }
        }
    }
}
