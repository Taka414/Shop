//
// (C) 2022 Takap.
//
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 色に関係する汎用機能を提供します。
    /// </summary>
    public class ColorUtil
    {
        /// <summary>
        /// 一般的なHP表現色(=最大値が緑->最小値が赤)を取得します。
        /// </summary>
        public static Color GetHelthColor(float maxValue, float currentValue)
        {
            //
            // HSV のカラーサークルが以下の性質を利用して
            // 緑 → 赤のグラデーションを取得する
            // H = 0   -> 赤
            // H = 120 -> 緑
            //        

            if (currentValue < 0)
            {
                currentValue = 0;
            }
            else if (currentValue > maxValue)
            {
                currentValue = maxValue;
            }

            // Unity の色指定の範囲:
            // H, S, V = 0 ～ 1.0
            return Color.HSVToRGB(120.0f * (currentValue / maxValue) / 360.0f, 1.0f, 1.0f);
        }
    }
}