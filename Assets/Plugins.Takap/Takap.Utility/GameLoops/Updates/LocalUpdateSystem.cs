//
// (C) 2022 Takap.
//

using System;
using R3;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Updateを自分で管理するための管理システムを表します。
    /// </summary>
    public class LocalUpdateSystem : MonoBehaviour
    {
        //
        // Constant
        // - - - - - - - - - - - - - - - - - - - -

        // 初期配列サイズ
        private const int DEFAULT_ARRAY_SIZE = 16;

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // ヒエラルキー上に表示しないようにするかどうかを設定または取得します。既定では表示(=true)します。
        // true : 表示する(既定) / false : 表示しない
        [SerializeField] bool _hideInHierarchy;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        readonly IndexedList<IUpdatable> _list = new IndexedList<IUpdatable>();

        //
        // Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="IUpdatable"/> 実行中に例外が発生した時に通知を受け取ります。
        /// </summary>
        public Observable<LocalUpdateSystemErrorInfo> UnhandlEdexception => _unhandledExceptionSubject;
        readonly Subject<LocalUpdateSystemErrorInfo> _unhandledExceptionSubject = new();

        //
        // Unity impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            IDisposable context = UnhandlEdexception.Subscribe(info =>
            {
                Debug.LogError(info.Exception.ToString(), info.SourceObject as UnityEngine.Object);
            });
        }

        void Update()
        {
            var array = _list.GetArray();
            IUpdatable item = null;
            for (int i = 0; i < array.Length; i++)
            {
                item = array[i];
                try
                {
                    if (!item.IsEnabled)
                    {
                        continue;
                    }
                    array[i].UpdateLocalCore();
                }
                catch (Exception ex)
                {
                    _unhandledExceptionSubject.OnNext(new LocalUpdateSystemErrorInfo(array[i], ex));
                }
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// オブジェクトを管理に追加します。
        /// </summary>
        public void Add(IUpdatable item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// オブジェクトを管理から削除します。
        /// </summary>
        public void Remove(IUpdatable item)
        {
            _list.Remove(item);
        }
    }
}
