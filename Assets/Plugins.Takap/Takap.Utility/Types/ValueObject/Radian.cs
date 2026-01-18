//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// ラジアン角を表します。
    /// </summary>
    public readonly struct Radian : IEquatable<Radian>
    {
        readonly float _value;

        /// <summary>
        /// このラジアン角に対応する角度を取得します。
        /// </summary>
        public Degree Degree => _value * Mathf.Rad2Deg;

        /// <summary>
        /// このオブジェクトの値を取得します。
        /// </summary>
        public float Value => _value;

        public Radian(float value) => this._value = value;

        public static implicit operator float(Radian value) => value._value;
        public static implicit operator Radian(float value) => new Radian(value);

        public bool Equals(Radian other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is Radian _obj && Equals(_obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();

        public static bool operator ==(in Radian x, in Radian y) => x._value.Equals(y._value);
        public static bool operator !=(in Radian x, in Radian y) => !x._value.Equals(y._value);

        public Vector2 GetVector(float rate = 1.0f) => MathfUtil.GetVector(this, rate);
    }
}
