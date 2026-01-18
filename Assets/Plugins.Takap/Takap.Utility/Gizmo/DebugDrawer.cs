//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Takap.Utility;
using Takap.Utility.Map;
using UnityEngine;

namespace Takap.Games.Sample
{
    /// <summary>
    /// デバッグ用の描画を行います。
    /// </summary>
    public static class DebugDrawer
    {
        /// <summary>
        /// [デバッグ用] 指定した位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(Vector2 pos, float size, Color color, float dulation = 2.0f) => DispCrossMark((Vector3)pos, size, color, dulation);
        /// <summary>
        /// [デバッグ用] 指定した位置にクロスマークを描画します。
        /// </summary>
        public static void DispCrossMark(Vector3 pos, float size, Color color, float dulation = 2.0f)
        {
            float cx = pos.x;
            float cy = pos.y;
            var tr = new Vector3(cx + size, cy + size);
            var tl = new Vector3(cx - size, cy + size);
            var br = new Vector3(cx + size, cy - size);
            var bl = new Vector3(cx - size, cy - size);
            Debug.DrawLine(tr, bl, color, dulation);
            Debug.DrawLine(tl, br, color, dulation);
        }


        /// <summary>
        /// [デバッグ用] 指定した位置にグリッドを描画します。
        /// </summary>
        public static void DispGrid(Vector2 pos, float size, Color color, float dulation = 2.0f) => DispGrid(pos, size, color, dulation);
        /// <summary>
        /// [デバッグ用] 指定した位置にグリッドを描画します。
        /// </summary>
        public static void DispGrid(Vector3 pos, float size, Color color, float dulation = 2.0f)
        {
            float x = pos.x;
            float y = pos.y;
            float harf = size / 2.0f;
            var tr = new Vector3(x + harf, y + harf);
            var bl = new Vector3(x - harf, y - harf);
            var tl = new Vector3(x - harf, y + harf);
            var br = new Vector3(x + harf, y - harf);
            Debug.DrawLine(tr, tl, Color.red, dulation);
            Debug.DrawLine(tl, bl, Color.red, dulation);
            Debug.DrawLine(bl, br, Color.red, dulation);
            Debug.DrawLine(br, tr, Color.red, dulation);
        }


        /// <summary>
        /// [デバッグ用] 指定したグリッド位置情報を使ってルートを描画します。
        /// </summary>
        public static void DispRoute(Map2DGrid map, IEnumerable<Point2Di> route, Color color, float dulation = 4.0f)
        {
            foreach (Point2Di pos in route)
            {
                Utility.Algorithm.GridSource item = map[pos];
                DispCrossMark(new Vector2(item.Position.x, item.Position.y), 0.1f, color, dulation);
            }
        }
    }
}