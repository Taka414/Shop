// 
// (C) 2022 Takap.
// 

using System;

namespace Takap.Utility
{
    /// <summary>
    /// 参照渡しのパラメーターで受け取り側が変更してはいけないことを表す。
    /// 付与するだけで特にシステム上の意味はありません。
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false, AllowMultiple = true)]
    public sealed class ConstAttribute : Attribute
    {
    }
}
