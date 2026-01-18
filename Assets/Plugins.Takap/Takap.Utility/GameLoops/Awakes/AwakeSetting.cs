//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 順番にAwakeを呼び出します。
    /// </summary>
    public class AwakeSetting : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] MonoBehaviour[] _items;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                MonoBehaviour item = _items[i];
                if (item is null)
                {
                    Debug.LogWarning($"AwakeObjects, [{i}] is null.");
                }

                foreach (IAwake obj in item.GetComponents<IAwake>())
                {
                    if (obj is IAwake awakeObj)
                    {
                        Debug.Log($"AwakeObjects, [{i}] Call to LocalAwake. name={item.gameObject.name}, type={obj.GetType().Name}");
                        awakeObj.LocalAwake();
                    }
                }
            }
        }
    }
}
