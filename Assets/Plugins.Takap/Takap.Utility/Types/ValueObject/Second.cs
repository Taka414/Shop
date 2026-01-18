//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// 秒を表します。
    /// </summary>
    public readonly struct Second : IEquatable<Second>
    {
        readonly float _value;

        /// <summary>
        /// このオブジェクトの値を取得します。
        /// </summary>
        public float Value => _value;

        /// <summary>
        /// このオブジェクトの値を <see cref="TimeSpan"/> として取得する。
        /// </summary>
        public TimeSpan TimeSpan => TimeSpan.FromSeconds(_value);

        public Second(float value) => this._value = value;

        public static implicit operator float(Second value) => value._value;
        public static implicit operator Second(float value) => new Second(value);

        public bool Equals(Second other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is Second _obj && Equals(_obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();

        public static bool operator ==(in Second a, in Second b) => a._value.Equals(b._value);
        public static bool operator !=(in Second a, in Second b) => !a._value.Equals(b._value);

        public static Second operator +(in Second a, in Second b) => new Second(a._value + b._value);
        public static Second operator +(float a, in Second b) => new Second(a + b._value);
        public static Second operator +(in Second a, float b) => new Second(a._value + b);

        public static Second operator -(in Second a, in Second b) => new Second(a._value - b._value);
        public static Second operator -(float a, in Second b) => new Second(a + b._value);
        public static Second operator -(in Second a, float b) => new Second(a._value + b);

        public static Second operator *(in Second a, in Second b) => new Second(a._value * b._value);
        public static Second operator *(float a, in Second b) => new Second(a * b._value);
        public static Second operator *(in Second a, float b) => new Second(a._value * b);

        public static Second operator /(in Second a, in Second b) => new Second(a._value / b._value);
        public static Second operator /(float a, in Second b) => new Second(a / b._value);
        public static Second operator /(in Second a, float b) => new Second(a._value / b);

        public static bool operator >(in Second a, in Second b) => a._value > b._value;
        public static bool operator >(float a, in Second b) => a > b._value;
        public static bool operator >(in Second a, float b) => a._value > b;

        public static bool operator <(in Second a, in Second b) => a._value < b._value;
        public static bool operator <(float a, in Second b) => a < b._value;
        public static bool operator <(in Second a, float b) => a._value < b;

        public static bool operator >=(in Second a, in Second b) => a._value >= b._value;
        public static bool operator >=(float a, in Second b) => a >= b._value;
        public static bool operator >=(in Second a, float b) => a._value >= b;

        public static bool operator <=(in Second a, in Second b) => a._value <= b._value;
        public static bool operator <=(float a, in Second b) => a <= b._value;
        public static bool operator <=(in Second a, float b) => a._value <= b;
    }
}
