// 
// (C) 2022 Takap.
// 

using System;

namespace Takap.Utility
{
    /// <summary>
    /// アニメーションクリップから呼び出されるイベントを表します。
    /// クリップからのメソッド名での呼び出しがあるため名前の変更には注意しましょう。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class CallFromAnimationClipAttribute : Attribute
    {
    }
}