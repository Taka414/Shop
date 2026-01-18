// 
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// フィールド、プロパティ、パラメータの設定値が変更禁止な事を表します。
    /// 特にリリースした後に変更してはいけない定数などに付与します。
    /// 目印として付与するだけでシステム上の意味は特にありません。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class DoNotChangeAttribute : Attribute
    {
        /// <summary>
        /// 既定の初期値でオブジェクトを生成します。
        /// </summary>
        public DoNotChangeAttribute()
        {
        }

        /// <summary>
        /// 禁止の理由や対象を説明する文字列を指定してオブジェクトを生成します。
        /// </summary>
        public DoNotChangeAttribute(string message)
        {
        }
    }
}
