//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 角度を表します。
    /// </summary>
    public readonly struct Degree : IEquatable<Degree>
    {
        readonly float _value;

        /// <summary>
        /// このオブジェクトの値を取得します。
        /// </summary>
        public float Value => _value;

        /// <summary>
        /// この角度に対応するラジアン角を取得します。
        /// </summary>
        public Radian Radian => _value * Mathf.Deg2Rad;

        public Degree(float value) => _value = value;

        public static implicit operator float(Degree value) => value._value;
        public static implicit operator Degree(float value) => new Degree(value);

        public bool Equals(Degree other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is Degree _obj && Equals(_obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();

        public static bool operator ==(in Degree x, in Degree y) => x._value.Equals(y._value);
        public static bool operator !=(in Degree x, in Degree y) => !x._value.Equals(y._value);

        public static Degree operator +(in Degree x, in Degree y) => new Degree(x._value + y.Value);
        public static Degree operator +(float x, in Degree y) => new Degree(x + y.Value);
        public static Degree operator +(in Degree x, float y) => new Degree(x._value + y);
        public static Degree operator -(in Degree x, in Degree y) => new Degree(x._value - y.Value);
        public static Degree operator -(float x, in Degree y) => new Degree(x - y.Value);
        public static Degree operator -(in Degree x, float y) => new Degree(x._value - y);

        public Vector2 GetVector(float rate = 1.0f) => MathfUtil.GetVector(this, rate);
    }
}
