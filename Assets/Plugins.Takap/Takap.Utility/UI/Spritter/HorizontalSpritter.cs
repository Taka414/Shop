//
// (C) 2022 Takap.
//

using Takap.Utility;
using UnityEngine;

namespace Takap.UI
{
    /// <summary>
    /// 左右にドラッグできるサイズ調整用の境界線を表します。
    /// </summary>
    public class HorizontalSpritter : Spritter
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 左右のパネル
        [SerializeField] RectTransform _left;
        [SerializeField] RectTransform _right;
        [SerializeField] float _rightMinSize;
        [SerializeField] float _leftMinsize;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        protected override void Core(Vector2 scaledDelta)
        {
            scaledDelta.y = 0;

            Vector2 leftSize = _left.GetSizeDelta() + scaledDelta;
            Vector2 rightSize = _right.GetSizeDelta() - scaledDelta;

            // 最小値がある場合これ以上変更できない
            if (leftSize.x < _leftMinsize || rightSize.x < _rightMinSize)
            {
                return;
            }

            // 領域外に出てったら戻って来るまで位置は更新しない
            var p = _cam.ScreenToWorldPoint(Input.mousePosition);
            if (scaledDelta.x > 0)
            {
                if (p.x < _rect.position.x)
                {
                    return;
                }
            }
            else
            {
                if (p.x > _rect.position.x)
                {
                    return;
                }
            }

            if (_left) _left.SetSize(leftSize);
            if (_right) _right.SetSize(rightSize);
            _rect.SetAnchoredPos(_rect.anchoredPosition + scaledDelta);
        }
    }
}
