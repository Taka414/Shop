//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// 方向に関係する汎用操作を提供するクラス 
    /// </summary>
    public static class AngleUtil
    {
        /// <summary>
        /// 4方向を8方向に変換する(TopView -> QuarterView)
        /// </summary>
        public static Direction8 ToAngle8(Direction4 angle)
        {
            switch (angle)
            {
                case Direction4.Up: return Direction8.Up;
                case Direction4.Down: return Direction8.Down;
                case Direction4.Left: return Direction8.Left;
                case Direction4.Right: return Direction8.Right;
                case Direction4.None: return Direction8.None;
                default: return Direction8.None;
            };
        }

        /// <summary>
        /// 8方向を4方向に変換する
        /// </summary>
        public static Direction4 ToAngle4(Direction8 angle)
        {
            switch (angle)
            {
                case Direction8.Up: return Direction4.Up;
                case Direction8.Down: return Direction4.Down;
                case Direction8.Left: return Direction4.Left;
                case Direction8.Right: return Direction4.Right;
                case Direction8.None: return Direction4.None;
                // それ以外は対応していない
                default: return Direction4.None;
            }
        }

        /// <summary>
        /// 指定した向きの反対を取得します。
        /// </summary>
        public static Direction4 Reverse(Direction4 angle)
        {
            switch (angle)
            {
                case Direction4.None: return Direction4.None;
                case Direction4.Up: return Direction4.Down;
                case Direction4.Down: return Direction4.Up;
                case Direction4.Left: return Direction4.Right;
                case Direction4.Right: return Direction4.Left;
            }

            throw new NotSupportedException($"未サポートの変換です。{angle}");
        }

        /// <summary>
        /// 指定したラジアンをAngle4に変換します。
        /// </summary>
        public static Direction4 ToAngle4(in Radian rad)
        {
            return ToAngle4((float)rad);
        }

        /// <summary>
        /// 指定したラジアンをAngle4に変換します。
        /// </summary>
        public static Direction4 ToAngle4(float rad)
        {
            float deg = MathfUtil.ToDeg(rad);

            if (deg >= -45.0f && deg < 45.0f)
            {
                return Direction4.Right;
            }
            else if (deg >= 45.0f && deg < 135.0f || deg > -315.0f && deg < -225.0f)
            {
                return Direction4.Up;
            }
            else if (deg >= 135.0f && deg < 225.0f || deg >= -225.0f && deg < -135.0f)
            {
                return Direction4.Left;
            }
            else if (deg > 225.0f && deg < 315 || deg >= -135.0f && deg < -45.0f)
            {
                return Direction4.Down;
            }
            else
            {
                return Direction4.None;
            }
        }

        /// <summary>
        /// 反対方向を取得します。
        /// </summary>       
        public static Direction2 Reverse(Direction2 angle)
        {
            switch (angle)
            {
                case Direction2.Left: return Direction2.Right;
                case Direction2.Right: return Direction2.Left;

                case Direction2.None:
                default: return angle;
            }
        }

        /// <summary>
        /// 角度から方向を取得します。
        /// </summary>
        public static Direction2 ToAngle2(float degree)
        {
            float nd = UnityEngine.Mathf.Repeat(degree, 360);
            if (degree > -90.0f && degree <= 90.0f || degree >= 270.0f && degree <= 360.0f)
            {
                return Direction2.Right;
            }
            else
            {
                return Direction2.Left;
            }
        }
    }
}
