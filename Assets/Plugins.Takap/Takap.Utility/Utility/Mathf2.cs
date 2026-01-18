//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// よくある計算を提供します。
    /// </summary>
    public static class Mathf2
    {
        /// <summary>
        /// 0 ～ b の間を t で線形補間します。0 - 1.0外の範囲も応答します。
        /// </summary>
        public static float LerpUnclamped(float max, float percent)
        {
            return LerpUnclamped(0, max, percent);
        }

        /// <summary>
        /// a と b の間を t で線形補間します。0 - 1.0外の範囲も応答します。
        /// </summary>
        public static float LerpUnclamped(float min, float max, float percent)
        {
            return min + (max - min) * percent;
        }

        /// <summary>
        /// 指定した値が min - max の範囲内でどれくらいの割合かを取得します。
        /// </summary>
        public static float InverseLerpUnclamped(float max, float value) => InverseLerpUnclamped(0, max, value);

        /// <summary>
        /// 指定した値が min - max の範囲内でどれくらいの割合かを取得します。
        /// </summary>
        public static float InverseLerpUnclamped(float min, float max, float value)
        {
            // 
            // e.g.
            // 0   - 100 で 50 だと 0.5
            // 10  - 110 で 60 でも 0.5
            // 120 - 100 は 1.2 (上限がない)
            //

            if (min > max)
            {
                throw new ArgumentOutOfRangeException($"min={min} > max={max}");
            }

            return (value - min) / (max - min);
        }

        /// <summary>
        /// 新しい範囲で旧範囲内と同じ割合の数値を取得します。
        /// </summary>
        public static float ReRange(float min, float max, float value, float newMin, float newMax)
        {
            // 
            //
            // 以下のように範囲内の同じ割合に変換する
            //
            // e.g.
            // 0 - 100, 50  -> 200 - 300, 250
            //

            float percent = InverseLerpUnclamped(min, max, value);
            return LerpUnclamped(newMin, newMax, percent);
        }
    }
}