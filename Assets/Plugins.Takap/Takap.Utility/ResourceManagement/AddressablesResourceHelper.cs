//
// (C) 2022 Takap.
// 

using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Takap.Utility
{
    /// <summary>
    /// BGM・SE用のAddressables 向けの <see cref="SoundManager"/> で使用するリソースを管理します。
    /// </summary>
    /// <remarks>
    /// ロード/アンロードの管理までしか行いません。
    /// </remarks>
    public class AddressablesResourceHelper<T> where T : Object
    {
        // ロードしたリソースを記録しておく貯めのテーブル
        //  TKey: リソースの名前
        //  TValue: リソースのハンドル
        private Dictionary<string, (AsyncOperationHandle<T> Hanlde, T Asset)> _table = new();

        /// <summary>
        /// リソースを非同期ロードします。
        /// </summary>
        public async Task<T> LoadAsync(string name, string path)
        {
            CheckName(name);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            T asset = await handle;
            _table[name] = (handle, asset);
            return asset;
        }
        public async Task<T> LoadAsync(string name, AssetReference path)
        {
            CheckName(name);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            T asset = await handle;
            _table[name] = (handle, asset);
            return asset;
        }
        public async Task<T> LoadAsync(string name, AssetReferenceT<T> path)
        {
            return await LoadAsync(name, (AssetReference)path);
        }

        /// <summary>
        /// リソースを同期ロードします。
        /// </summary>
        public T Load(string name, string path)
        {
            CheckName(name);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            T asset = handle.WaitForCompletion();
            _table[name] = (handle, asset);
            return asset;
        }
        public T Load(string name, AssetReference path)
        {
            CheckName(name);

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            T asset = handle.WaitForCompletion();
            _table[name] = (handle, asset);
            return asset;
        }
        public T Load(string name, AssetReferenceT<T> path)
        {
            return Load(name, (AssetReference)path);
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Release(string name)
        {
            if (!_table.ContainsKey(name))
            {
                Log.Warn($"Name is not found. name={name}");
                return;
            }
            Addressables.Release(_table[name].Hanlde);
            _table.Remove(name);
        }

        /// <summary>
        /// 現在管理中のリソースを全て開放します。
        /// </summary>
        public void ReleaseAll()
        {
            _table.Keys.ForEach(name => Release(name));
        }

        /// <summary>
        /// 指定した名前に関連するアセットを取得します。
        /// </summary>
        public T Get(string name)
        {
            if (!_table.ContainsKey(name))
            {
                throw new System.ArgumentException($"The name not exists. name={name}");
            }
            return _table[name].Asset;
        }

        /// <summary>
        /// 管理テーブルをクリアします。
        /// </summary>
        internal void Clear()
        {
            _table.Clear();
        }

        /// <summary>
        /// 現在管理中のキーを列挙します。
        /// </summary>
        public IEnumerable<string> GetKeys()
        {
            return _table.Keys;
        }

        /// <summary>
        /// テーブルに既に名前が存在する場合例外を throw します。
        /// </summary>
        private void CheckName(string name)
        {
            if (_table.ContainsKey(name))
            {
                throw new System.ArgumentException($"The name already exists. name={name}");
            }
        }
    }
}