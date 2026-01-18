// 
// (C) 2022 Takap.
// 

using System;

namespace Takap.Utility
{
    /// <summary>
    /// イベントからから呼び出される処理を表します。
    /// インスペクター上からの設定があるため名前の変更には注意しましょう。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class CallFromUnityEventAttribute : Attribute
    {
        public CallFromUnityEventAttribute()
        {
            // nop
        }

        public CallFromUnityEventAttribute(string message)
        {
            // nop
        }
    }
}