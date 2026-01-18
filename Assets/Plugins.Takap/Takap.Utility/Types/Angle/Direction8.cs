//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 8方向を表します。
    /// </summary>
    public enum Direction8
    {
        // 
        // 各方向の割り当て:
        //
        // UpLeft   | Up   |  Up Right
        // ----------------------------
        // Left     | x    | Right
        // ----------------------------
        // DownLeft | Down | DownRight
        // 

        /// <summary>未設定</summary>
        None = 0,
        /// <summary>上</summary>
        Up,
        /// <summary>下</summary>
        Down,
        /// <summary>左</summary>
        Left,
        /// <summary>右</summary>
        Right,
        /// <summary>左上</summary>
        UpLeft,
        /// <summary>右上</summary>
        UpRight,
        /// <summary>左下</summary>
        DownLeft,
        /// <summary>右下</summary>
        DownRight,
    }
}
