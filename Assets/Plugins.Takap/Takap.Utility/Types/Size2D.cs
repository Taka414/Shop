//
// (C) 2022 Takap.
//

using System;
using Sirenix.OdinInspector;
using UnityEngine;

#pragma warning disable

namespace Takap.Utility
{
    /// <summary>
    /// 縦・横を表すサイズを表します。
    /// </summary>
    [Serializable, InlineProperty]
    public struct Size2DI
    {
        public static readonly Size2DI Zero = new Size2DI(0, 0);

        [SerializeField, LabelWidth(60)] int width;
        [SerializeField, LabelWidth(60)] int height;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public Size2DI(int w, int h)
        {
            width = w;
            height = h;
        }

        public static implicit operator Vector2Int(Size2DI value) => new Vector2Int(value.width, value.height);
        public static implicit operator Size2DI(Vector2Int value) => new Size2DI(value.x, value.y);
    }

    /// <summary>
    /// 縦・横を表すサイズを表します。
    /// </summary>
    [Serializable, InlineProperty]
    public struct Size2DF
    {
        public static readonly Size2DF Zero = new Size2DF(0, 0);

        [SerializeField, LabelWidth(60)] float width;
        [SerializeField, LabelWidth(60)] float height;

        public float Width { get => width; set => width = value; }
        public float Height { get => height; set => height = value; }

        public Size2DF(float w, float h)
        {
            width = w;
            height = h;
        }

        public static implicit operator Vector2(Size2DF value) => new Vector2(value.width, value.height);
        public static implicit operator Size2DF(Vector2 value) => new Size2DF(value.x, value.y);
    }
}