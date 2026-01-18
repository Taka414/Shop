//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// UI要素のコンポーネントを表します。
    /// </summary>
    public abstract class UIMonoBehaviour : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= transform as RectTransform;
    }
}
