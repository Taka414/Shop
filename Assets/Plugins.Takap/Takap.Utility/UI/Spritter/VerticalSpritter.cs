//
// (C) 2022 Takap.
//

using Takap.Utility;
using UnityEngine;

namespace Takap.UI
{
    /// <summary>
    /// 上下にドラッグできるサイズ調整用の境界線を表します。
    /// </summary>
    public class VerticalSpritter : Spritter
    {
        // 上下のパネル
        [SerializeField] RectTransform _top;
        [SerializeField] RectTransform _bottom;
        [SerializeField] float _topMinSize;
        [SerializeField] float _bottomMinsize;

        protected override void Core(Vector2 scaledDelta)
        {
            if (!_top || !_bottom)
            {
                Log.Warn("Top or Bottom not set in Inspector.", this);
            }

            scaledDelta.x = 0;

            Vector2 bottomSize = _bottom.GetSizeDelta() + scaledDelta;
            Vector2 topSize = _top.GetSizeDelta() - scaledDelta;

            // 最小値がある場合これ以上変更できない
            if (bottomSize.y < _bottomMinsize || topSize.y < _topMinSize)
            {
                return;
            }

            // 領域外に出てったら戻って来るまで位置は更新しない
            var p = _cam.ScreenToWorldPoint(Input.mousePosition);
            if (scaledDelta.y > 0)
            {
                if (p.y < _rect.position.y)
                {
                    return;
                }
            }
            else
            {
                if (p.y > _rect.position.y)
                {
                    return;
                }
            }

            _bottom.SetSize(bottomSize);
            _top.SetSize(topSize);
            _rect.SetAnchoredPos(_rect.anchoredPosition + scaledDelta);
        }
    }
}
