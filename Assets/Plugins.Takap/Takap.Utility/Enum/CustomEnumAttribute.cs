//  
// (C) 2023 Takap.
// 

using System;
using UnityEngine;

namespace Takap.Utility
{
    // カスタムEnum型を扱うためのクラス
    //
    // 使い方:
    // https://takap-tech.com/entry/2023/08/08/232323
    //  → 多分使わないけどまとめておいた
    public class CustomEnumAttribute : PropertyAttribute
    {
        public readonly Type Type;
        
        public CustomEnumAttribute(Type enumType) => Type = enumType;

        // 属性に指定されている型がenumかどうかを取得する
        // true: enumである / false: enumでない
        public bool IsEnum => Type != null && Type.IsEnum;
    }
}