//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// キャンバスに対する汎用機能を提供します。
    /// </summary>
    public class RectUtil
    {
        /// <summary>
        /// <paramref name="target"/> のサイズを親要素の大きさに合わせます。
        /// </summary>
        public static void FitParent(RectTransform target)
        {
            target.pivot = new Vector2(0.5f, 0.5f);
            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.offsetMax = Vector2.zero;
            target.offsetMin = Vector2.zero;
        }
    }
}