// 
// (C) 2022 Takap.
// 

using UnityEngine;

namespace Takap.Utility.Algorithm
{
    public class GridSource : ICanMoveGrid
    {
        /// <summary>
        /// このグリッドが移動可能かどうかを設定または取得します。
        /// true : 移動可能 / false : 不可
        /// </summary>
        public bool CanMove { get; set; }

        /// <summary>
        /// このグリッドのX方向のインデックスを設定または取得します。
        /// </summary>
        public int XIndex { get; set; }

        /// <summary>
        /// このグリッドのY方向のインデックスを設定または取得します。
        /// </summary>
        public int YIndex { get; set; }

        /// <summary>
        /// このグリッドの位置を表します。
        /// </summary>
        public Vector2 Position { get; set; }

        public override string ToString() => $"xi={XIndex}, yi={YIndex}, ({Position})";
    }
}