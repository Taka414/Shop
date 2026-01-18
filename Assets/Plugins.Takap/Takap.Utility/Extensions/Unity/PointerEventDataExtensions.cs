//
// (C) 2022 Takap.
//

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap
{
    /// <summary>
    /// <see cref="PointerEventData"/> の機能を拡張しmス。
    /// </summary>
    public static class PointerEventDataExtensions
    {
        public static Vector2 ToLocalPos(this PointerEventData self, Graphic g, RectTransform rectTransform = null)
        {
            return self.position.ToLocalPos(g, rectTransform);
        }
    }
}
