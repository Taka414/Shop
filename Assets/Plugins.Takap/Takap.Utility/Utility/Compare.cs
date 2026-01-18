//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// 比較に関係する機能を提供します。
    /// </summary>
    public static class Compare
    {
        /// <summary>
        /// <see cref="Comparison{T}"/> でfloat型の使用するときのヘルパーメソッド
        /// </summary>
        public static int Float(float a, float b)
        {
            float c = a - b;
            if (c == 0) { return 0; }
            else if (c > 0) { return 1; }
            return -1;
        }
    }
}