//
// (C) 2022 Takap.
//

using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// 重み付きランダム抽選の要素の条件を表します。
    /// </summary>
    [System.Serializable]
    public readonly struct WeightedLotteryItem<T> : System.IEquatable<WeightedLotteryItem<T>>
    {
        public readonly T Value;
        public readonly float Weight;

        // 自分自身の型の情報(派生クラスを考慮)
        System.Type _type => typeof(WeightedLotteryItem<T>);

        public WeightedLotteryItem(T value, float weight)
        {
            Value = value;
            Weight = weight;
        }

        public static implicit operator WeightedLotteryItem<T>((T a, float b) pair) => new(pair.a, pair.b);

        public static bool operator ==(WeightedLotteryItem<T> left, WeightedLotteryItem<T> right)
        {
            return ReferenceEquals(left, right) || left.Equals(right);
        }
        public static bool operator !=(WeightedLotteryItem<T> left, WeightedLotteryItem<T> right)
        {
            return !(left == right);
        }

        // 同じ内容あれば同じ値を返す
        public override int GetHashCode() => System.HashCode.Combine(_type, Value, Weight);

        // IEquatable の実装
        public bool Equals(WeightedLotteryItem<T> other)
        {
            return ReferenceEquals(this, other) ||
                   _type == other._type &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value) &&
                   EqualityComparer<float>.Default.Equals(Weight, other.Weight);
        }

        // 同じ内容であればtrueを返す
        public override bool Equals(object obj) => Equals((WeightedLotteryItem<T>)obj);
    }
}