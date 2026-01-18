//
// (C) 2022 Takap.
//

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 上下左右を表します。
    /// </summary>
    [Serializable]
    public /*readonly*/ struct Thickness : IEquatable<Thickness>
    {
        // JsonUtility でシリアライズする事を考慮

        // Fields
        //public readonly float Left;
        //public readonly float Top;
        //public readonly float Right;
        //public readonly float Bottom;
        [HorizontalGroup("grp1", LabelWidth = 16), LabelText("L")]
        [SerializeField] private float _left;
        [HorizontalGroup("grp1", marginLeft: 4), LabelText("T")]
        [SerializeField] private float _top;
        [HorizontalGroup("grp1", marginLeft: 4), LabelText("R")]
        [SerializeField] private float _right;
        [HorizontalGroup("grp1", marginLeft: 4), LabelText("B")]
        [SerializeField] private float _bottom;

        // Props
        public float Left => _left;
        public float Top => _top;
        public float Right => _right;
        public float Bottom => _bottom;
        public float LR => _left + _right;
        public float TB => _top + _bottom;

        public Thickness(float l, float t, float r, float b)
        {
            _left = l;
            _top = t;
            _right = r;
            _bottom = b;
        }

        // 演算子のオーバーライド
        public static bool operator ==(in Thickness a, in Thickness b) => Equals(a, b);
        public static bool operator !=(in Thickness a, in Thickness b) => !Equals(a, b);

        // 4つの組み合わせからHSVオブジェクトを作成する
        public static implicit operator Thickness((float l, float t, float r, float b) thickness)
        {
            return new Thickness(thickness.l, thickness.t, thickness.r, thickness.b);
        }

        // 等値比較演算子の実装
        public readonly override bool Equals(object obj) => (obj is Thickness _obj) && Equals(_obj);

        // IEquatable<T> の implement
        public readonly bool Equals(Thickness other)
        {
            // 個別に記述する
            return ReferenceEquals(this, other) ||
                   _left == other._left &&
                   _top == other._top &&
                   _right == other._right &&
                   _bottom == other._bottom;
        }

        public readonly override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _left.GetHashCode();
                hashCode = (hashCode * 397) ^ _top.GetHashCode();
                hashCode = (hashCode * 397) ^ _right.GetHashCode();
                hashCode = (hashCode * 397) ^ _bottom.GetHashCode();
                return hashCode;
            }
        }
    }

    /// <summary>
    /// <see cref="Thickness"/> を利用しやすいような拡張メソッドを定義します。
    /// </summary>
    public static class ThicknessExtension
    {
        /// <summary>
        /// このオブジェクトを <see cref="RectTransform"/> を仮定して 
        /// <see cref="GetLocalSize(RectTransform)"/> に処理を転送します。
        /// </summary>
        public static Thickness GetLocalSize(this Transform t)
        {
            if (t is RectTransform rt)
            {
                return GetLocalSize(rt);
            }
            throw new InvalidOperationException($"t is not RectTransform.");
        }

        /// <summary>
        /// このオブジェクトの中央を(アンカーなどの設定に関わらず固定で)ゼロとしてローカルサイズを取得します。
        /// </summary>
        /// <remarks>
        /// RectTransform.localPostion と対応関係がある数値になる。
        /// </remarks>
        public static Thickness GetLocalSize(this RectTransform rt)
        {
            Rect rect = rt.rect;
            float widthHarf = rect.x / 2f;
            float hightHarf = rect.y / 2f;
            return new Thickness(-widthHarf, hightHarf, widthHarf, -hightHarf);
        }
    }
}