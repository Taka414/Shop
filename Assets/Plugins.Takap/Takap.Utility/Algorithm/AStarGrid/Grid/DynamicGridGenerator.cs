// 
// (C) 2022 Takap.
// 

using System;
using Takap.Utility.Map;
using UnityEngine;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// 通行可能か不可能かを表すグリッドマップを動的に作成します。
    /// </summary>
    public class DynamicGridGenerator
    {
        //
        // InnerTypes
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 何が通行可能・不可能かを判断する関数を設定または取得します。
        /// true : 通行不可能(ヒットした) / false : 通行可能
        /// </summary>
        public Func<RaycastHit2D[], bool> HitFunc { get; set; } = p => p.Length != 0;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した位置を中心に移動可能かどうかのマップを取得します。
        /// </summary>
        public (Map2DGrid map, GridSource center) CreateMap(Vector2 center, float width, float height, float gridSize)
        {
            (Map2DGrid map, GridSource center) ret = CreateGrid(center, width, height, gridSize);
            SetCanMoves(ret.map, gridSize);
            return ret;
        }

        /// <summary>
        /// デバッグ用に計算結果を画面に表示します。
        /// </summary>
        public void DisplayForDebug(Map2DGrid map, float gridSize, float dispTime)
        {
            float harf = gridSize / 2.0f;

            GridSource blItem = map[0, 0];
            var bl = new Vector2(blItem.Position.x - harf, blItem.Position.y - harf);
            GridSource brItem = map[map.Width - 1, 0];
            var br = new Vector2(brItem.Position.x + harf, brItem.Position.y - harf);
            GridSource tlItem = map[0, map.Height - 1];
            var tl = new Vector2(tlItem.Position.x - harf, tlItem.Position.y + harf);

            for (int y = 0; y < map.Height; y++)
            {
                var s = new Vector2(bl.x, bl.y + y * gridSize);
                var e = new Vector2(br.x, br.y + y * gridSize);
                Debug.DrawLine(s, e, Color.red, dispTime);
            }

            for (int x = 0; x < map.Width; x++)
            {
                var s = new Vector2(bl.x + x * gridSize, bl.y);
                var e = new Vector2(tl.x + x * gridSize, tl.y);
                Debug.DrawLine(s, e, Color.red, dispTime);
            }

            map.ForEach((x, y, item) =>
            {
                var _tr = new Vector2(item.Position.x + harf, item.Position.y + harf);
                var _bl = new Vector2(item.Position.x - harf, item.Position.y - harf);
                Vector2 direction = _bl - _tr;
                if (!item.CanMove)
                {
                    Debug.DrawRay(_tr, direction, Color.green, dispTime, false);
                }
            });
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した範囲のグリッドマップを作成します。
        /// </summary>
        private (Map2DGrid map, GridSource center) CreateGrid(Vector2 center, float width, float height, float gridSize)
        {
            int xCount = (int)(width / gridSize);
            int yCount = (int)(height / gridSize);
            var map = new Map2DGrid(xCount * 2 + 1, yCount * 2 + 1);

            for (int yi = 0; yi < map.Height; yi++)
            {
                for (int xi = 0; xi < map.Width; xi++)
                {
                    var g = new GridSource
                    {
                        XIndex = xi,
                        YIndex = yi
                    };
                    float xPos = ((xi - xCount) * gridSize) + center.x;
                    float yPos = ((yi - yCount) * gridSize) + center.y;
                    g.Position = new Vector2(xPos, yPos);
                    map[xi, yi] = g;
                }
            }

            return (map,
                    map[map.Width - xCount - 1, map.Height - yCount - 1]); // center で指定したグリッドの情報
        }

        /// <summary>
        /// 指定した位置が通行できるかをマップに設定します。
        /// </summary>
        private void SetCanMoves(Map2DGrid map, float gridSize)
        {
            float harf = gridSize / 2.0f;
            map.ForEach((x, y, item) =>
            {
                // Rayを右上から左下に飛ばして何も泣ければそこは通行可能と判断する
                var tr = new Vector2(item.Position.x + harf, item.Position.y + harf);
                var bl = new Vector2(item.Position.x - harf, item.Position.y - harf);
                Vector2 direction = bl - tr;
                float distance = Vector2.Distance(tr, bl);
                RaycastHit2D[] hits = Physics2D.RaycastAll(tr, direction, distance);
                item.CanMove = !HitFunc(hits);

                //// デバッグ用の表示
                //// Ray表示
                //Color c = item.CanMove ? Color.blue : Color.green;
                //Debug.DrawRay(tr, direction, c, span, false);
                //// Grid表示
                //var tl = new Vector2(item.Position.x - harf, item.Position.y + harf);
                //var br = new Vector2(item.Position.x + harf, item.Position.y - harf);
                //Debug.DrawLine(tr, tl, Color.red, span);
                //Debug.DrawLine(tl, bl, Color.red, span);
                //Debug.DrawLine(bl, br, Color.red, span);
                //Debug.DrawLine(br, tr, Color.red, span);
            });
        }
    }
}