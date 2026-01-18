//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

#pragma warning disable

namespace Takap.Utility
{
    /// <summary>
    /// 上下限のある範囲を表します。
    /// </summary>
    [Serializable]
    public struct RangeI
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        public int Min { get => _min; set => _min = value; }
        public int Max { get => _max; set => _max = value; }

        public RangeI(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public static implicit operator Vector2Int(RangeI value) => new Vector2Int(value.Min, value.Max);
        public static implicit operator RangeI(Vector2Int value) => new RangeI(value.x, value.y);

        public float Range() => UniRandom.Range(_min, _max);
    }

    /// <summary>
    /// 上下限のある範囲を表します。
    /// </summary>
    [Serializable]
    public struct RangeF
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min { get => _min; set => _min = value; }
        public float Max { get => _max; set => _max = value; }

        public RangeF(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public static implicit operator Vector2(RangeF value) => new Vector2(value._min, value._max);
        public static implicit operator RangeF(Vector2 value) => new RangeF(value.x, value.y);

        public float Range() => UniRandom.Range(_min, _max);
    }
}