//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// TimeSpan に関係する拡張メソッドを定義します。
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// 値を秒としてTimeSpanを取得します。
        /// </summary>
        public static TimeSpan Seconds(this float value) => TimeSpan.FromSeconds(value);
    }
}
