//
// (C) 2022 Takap.
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// HSVのValueObject
    /// </summary>
    public readonly struct ColorHsv : IEquatable<ColorHsv>
    {
        // Fields
        public readonly float H; // H : 0-360
        public readonly float S; // S : 0-100
        public readonly float V; // V : 0-100
        public readonly float A; // A : 0-100

        // Constructors
        public ColorHsv(float h, float s, float v)
        {
            H = Mathf.Clamp(h, 0, 360);
            S = Mathf.Clamp(s, 0, 100);
            V = Mathf.Clamp(v, 0, 100);
            A = 100; // default is max
        }
        public ColorHsv(float h, float s, float v, float a) : this(h, s, v)
        {
            A = Mathf.Clamp(a, 0, 100);
        }

        // 演算子のオーバーライド
        public static bool operator ==(in ColorHsv a, in ColorHsv b) => Equals(a, b);
        public static bool operator !=(in ColorHsv a, in ColorHsv b) => !Equals(a, b);

        // 3つの組み合わせからHSVオブジェクトを作成する
        public static implicit operator ColorHsv((float h, float s, float v) hsv)
        {
            return new ColorHsv(hsv.h, hsv.s, hsv.v);
        }
        // 4つの組み合わせからHSVオブジェクトを作成する
        public static implicit operator ColorHsv((float h, float s, float v, float a) hsva)
        {
            return new ColorHsv(hsva.h, hsva.s, hsva.v, hsva.a);
        }
        public static implicit operator Color(in ColorHsv hsv)
        {
            return hsv.ToRgb();
        }

        // 等値比較演算子の実装
        public override bool Equals(object obj) => (obj is ColorHsv _obj) && Equals(_obj);

        // IEquatable<T> の implement
        public bool Equals(ColorHsv other)
        {
            // 個別に記述する
            return ReferenceEquals(this, other) ||
                   H == other.H &&
                   S == other.S &&
                   V == other.V &&
                   A == other.A;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = H.GetHashCode();
                hashCode = (hashCode * 397) ^ S.GetHashCode();
                hashCode = (hashCode * 397) ^ V.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
            }
        }

        // -- Utils ---

        /// <summary>
        /// RGB から HSV を作成します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorHsv FromColorRgb(in Color rgbColor)
        {
            Color.RGBToHSV(rgbColor, out float h, out float s, out float v);
            return new ColorHsv(h, s, v);
        }

        /// <summary>
        /// HSV オブジェクトから RGB オブジェクトを取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color ToRgb() => Color.HSVToRGB(H, S, V);

        /// <summary>
        /// 指定した値にHを変更します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv SetH(float newH)
        {
            float h = Mathf.Clamp(newH, 0, 360);
            return (h, S, V);
        }
        /// <summary>
        /// 指定した値をHに加算します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv AddH(float value)
        {
            float h = Mathf.Clamp(H + value, 0, 360);
            return (h, S, V);
        }

        /// <summary>
        /// 指定した値にSを変更します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv SetS(float newS)
        {
            float s = Mathf.Clamp(newS, 0, 100);
            return (H, s, V);
        }
        /// <summary>
        /// 指定した値をSに加算します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv AddS(float value)
        {
            float s = Mathf.Clamp(S + value, 0, 100);
            return (H, s, V);
        }

        /// <summary>
        /// 指定した値にVを変更します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv SetV(float newV)
        {
            float v = Mathf.Clamp(newV, 0, 100);
            return (H, S, v);
        }
        /// <summary>
        /// 指定した値をVに加算します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ColorHsv AddV(float value)
        {
            float v = Mathf.Clamp(V + value, 0, 100);
            return (H, S, v);
        }
    }

    /// <summary>
    /// <see cref="ColorHsv"/> 用の便利な拡張機能を定義します。
    /// </summary>
    public static class ColorHsvExtension
    {
        /// <summary>
        /// <see cref="ColorHsv"/> を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorHsv GetColorHsv(this Image img)
        {
            Color.RGBToHSV(img.color, out float h, out float s, out float v);
            return (h, s, v);
        }

        /// <summary>
        /// <see cref="ColorHsv"/> を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorHsv GetColorHsv(this SpriteRenderer sp)
        {
            Color.RGBToHSV(sp.color, out float h, out float s, out float v);
            return (h, s, v);
        }

        /// <summary>
        /// <see cref="ColorHsv"/> を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorHsv ToHsv(this in Color c)
        {
            Color.RGBToHSV(c, out float h, out float s, out float v);
            return (h, s, v, c.a);
        }
    }
}