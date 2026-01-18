//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Takap.Utility
{
    /// <summary>
    /// あるタイルマップを操作しやすいよにラップします。
    /// </summary>
    public class TileMapWrapper
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 制御対象のマップを設定または取得します。
        /// </summary>
        public Tilemap Target { get; set; }

        /// <summary>
        /// グリッドの横幅を設定または取得します。
        /// </summary>
        public int GridWidth { get; set; }

        /// <summary>
        /// グリッドの高さを設定または取得します。
        /// </summary>
        public int GridHeight { get; set; }

        /// <summary>
        /// タイル生成方法を設定または取得します。
        /// </summary>
        public Func<string, Tile> GetTileImpl { get; set; }

        /// <summary>
        /// 現在配置済みのタイル一覧を取得します。
        /// TKey   : インデックス
        /// TValue : 実位置
        /// </summary>
        public Dictionary<Vector3Int, Vector3Int> Tiles { get; private set; } = new Dictionary<Vector3Int, Vector3Int>();

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// タイルを配置します。
        /// </summary>
        public void SetTile(Vector3Int posi)
        {
            Vector3Int rpos = ToRealPos(posi);
            Tile tile = GetTileImpl("");
            Target.SetTile(rpos, tile);
            Target.SetTileFlags(rpos, TileFlags.None);
            Tiles[posi] = rpos;
            //return this.GetTile(rpos);
        }
        public void SetTile(int xindex, int yindex) => SetTile(new Vector3Int(xindex, yindex, 0));

        /// <summary>
        /// タイルを取得します。
        /// </summary>
        public Tile GetTile(Vector3Int posi)
        {
            return Target.GetTile<Tile>(ToRealPos(posi));
        }
        public Tile GetTile(int xindex, int yindex) => GetTile(new Vector3Int(xindex, yindex, 0));

        /// <summary>
        /// タイルに色を設定します。
        /// </summary>
        public void SetColor(Vector3Int posi, Color c)
        {
            Target.SetColor(posi, c);
        }
        public void SetColor(int xindex, int yindex, Color c) => SetColor(new Vector3Int(xindex, yindex, 0), c);

        /// <summary>
        /// タイルの色を取得します。
        /// </summary>
        public Color GetColor(Vector3Int posi)
        {
            return Target.GetColor(ToRealPos(posi));
        }
        public Color GetColor(int xindex, int yindex) => GetColor(new Vector3Int(xindex, yindex, 0));

        public void DelTile(Vector3Int posi)
        {
            Tiles.Remove(posi);
            Target.SetTile(ToRealPos(posi), null);
        }

        //
        // Helpers
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// グリッドインデックスを実位置に変換します。
        /// </summary>
        public Vector3Int ToRealPos(int xindex, int yindex)
        {
            return new Vector3Int(xindex * GridWidth, yindex * GridHeight, 0);
        }
        public Vector3Int ToRealPos(Vector3Int pos)
        {
            return ToRealPos(pos.x, pos.y);
        }

        /// <summary>
        /// 実位置をグリッドインデックスに変換します。
        /// </summary>
        public Vector3Int ToIndex(float world_x, float world_y)
        {
            return new Vector3Int((int)(world_x / GridWidth), (int)(world_y / GridHeight), 0);
        }
        public Vector3Int ToIndex(Vector3Int pos)
        {
            return ToIndex(pos.x, pos.y);
        }
    }
}