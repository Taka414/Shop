//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Gizmoの描画関係の汎用機能を提供します。
    /// </summary>
    public static class GizmoDrawer
    {
        /// <summary>
        /// 指定した位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(in Vector2 pos, float size, in Color color) => DispCrossMark((Vector3)pos, size, color);

        /// <summary>
        /// 指定したオブジェクトの位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(Component component, float size, in Color color)
        {
            DispCrossMark(component.transform.localPosition, size, color);
        }

        /// <summary>
        /// 指定した位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(in Vector3 pos, float size, in Color color)
        {
            Color previousColor = Gizmos.color; // 書いたら元に戻す

            float cx = pos.x;
            float cy = pos.y;
            float xp = cx + size;
            float xm = cx - size;
            float yp = cy + size;
            float ym = cy - size;
            var tr = new Vector3(xp, yp, pos.z);
            var tl = new Vector3(xm, yp, pos.z);
            var br = new Vector3(xp, ym, pos.z);
            var bl = new Vector3(xm, ym, pos.z);
            Gizmos.color = color;
            Gizmos.DrawLine(tr, bl);
            Gizmos.DrawLine(tl, br);
            Gizmos.color = previousColor;
        }

        /// <summary>
        /// 指定した位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(float x, float y, float z, float size, in Color color)
        {
            float xp = x + size;
            float xm = x - size;
            float yp = y + size;
            float ym = y - size;
            var tr = new Vector3(xp, yp, z);
            var tl = new Vector3(xm, yp, z);
            var br = new Vector3(xp, ym, z);
            var bl = new Vector3(xm, ym, z);
            Gizmos.color = color;
            Gizmos.DrawLine(tr, bl);
            Gizmos.DrawLine(tl, br);
        }
    }
}