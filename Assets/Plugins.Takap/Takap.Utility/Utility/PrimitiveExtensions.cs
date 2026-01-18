//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// C# のプリミティブ型を拡張します。
    /// </summary>
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// 指定した数より大きい一番近い2のべき乗を取得します。
        /// </summary>
        public static int GetNearPow2(this int n)
        {
            if (n <= 0)
            {
                return 0;
            }

            var k = System.Math.Ceiling(System.Math.Log(n, 2));
            return (int)System.Math.Pow(2.0, k);
        }
    }
}