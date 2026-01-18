//
// (C) 2022 Takap.
//

using UnityEngine;
using UniObject = UnityEngine.Object;

namespace Takap.Utility.Samples
{
    /// <summary>
    /// オブジェクトプールのサンプル実装を表します。
    /// </summary>
    public class SimpleObjectPool_Sample : SimpleObjectPool<Sample>
    {
        // 生成するオブジェクト
        private readonly Sample _source;
        // 生成したオブジェクトを配置する親オブジェクト
        private readonly Transform _parent;

        /// <summary>
        /// プールを指定した値で初期化します。
        /// </summary>
        public SimpleObjectPool_Sample(Sample source, int maxCount, Transform parent)
        {
            _source = source;
            _parent = parent;
            MaxPoolCount = maxCount;
        }

        /// <summary>
        /// オブジェクトを新規作成します。
        /// </summary>
        protected override Sample CreateInstance()
        {
            Sample f()
            {
                if (_parent)
                {
                    return UniObject.Instantiate(_source, _parent);
                }
                else
                {
                    return UniObject.Instantiate(_source, _source.transform.parent);
                }
            }

            Sample p = f();
            p.PlayEnd = Return;
            p.name = p.name + "_Pool(" + TotalInstance + ")";
            return p;
        }

        // プールからオブジェクトを取得する前に実行される
        protected override void OnBeforeRent(Sample instance)
        {
            Debug.Log($"{instance.name}がプールから取り出されました");
            base.OnBeforeRent(instance);
        }

        // オブジェクトがプールに戻る前に実行される
        protected override void OnBeforeReturn(Sample instance)
        {
            Debug.Log($"{instance.name}がプールに戻されました");
            base.OnBeforeReturn(instance);
        }
    }

    /// <summary>
    /// オブジェクトプールのサンプル実装用のクラスを表します。
    /// </summary>
    public class Sample : MonoBehaviour
    {
        /// <summary>
        /// オブジェクトを返却するときに発生します。
        /// </summary>
        public System.Action<Sample> PlayEnd { get; set; }
    }
}