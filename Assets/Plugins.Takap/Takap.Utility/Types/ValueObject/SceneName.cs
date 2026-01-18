//
// (C) 2022 Takap.
//

using System;

namespace Takap.Utility
{
    /// <summary>
    /// ゲームで使用するシーン名を表します。
    /// </summary>
    public readonly partial struct SceneName : IEquatable<SceneName>
    {
        readonly string _value;

        /// <summary>
        /// このオブジェクトの値を取得します。
        /// </summary>
        public string Value => _value;

        public SceneName(string value) => _value = value;

        public static implicit operator string(SceneName value) => value._value;
        public static implicit operator SceneName(string value) => new SceneName(value);

        public bool Equals(SceneName other) => _value.Equals(other._value);

        public override bool Equals(object obj) => obj is SceneName _obj && Equals(_obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();

        public static bool operator ==(in SceneName x, in SceneName y) => x._value.Equals(y._value);
        public static bool operator !=(in SceneName x, in SceneName y) => !x._value.Equals(y._value);
    }
}
