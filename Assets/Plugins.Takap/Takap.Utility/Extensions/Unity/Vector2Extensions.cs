//
// (C) 2022 Takap.
//

using System;
using System.Runtime.CompilerServices;
using Takap.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Takap
{
    /// <summary>
    /// <see cref="Vector2"/> もしくは <see cref="Vector3"/>　の機能を拡張します。
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// スクリーン座標を指定したオブジェクトのローカル座標に変換します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToLocalPos(this in Vector2 self, Graphic g, RectTransform rectTransform = null)
        {
            rectTransform = rectTransform != null ? rectTransform : g.rectTransform;
            RenderMode mode = g.canvas.renderMode;
            if (mode == RenderMode.ScreenSpaceOverlay)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, self, null, out Vector2 result);
                return result;
            }
            else if (mode == RenderMode.ScreenSpaceCamera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, self, g.canvas.worldCamera, out Vector2 result);
                return result;
            }
            else
            {
                throw new NotSupportedException($"{RenderMode.WorldSpace} is not supported."); // しらん
            }
        }

        /// <summary>
        /// 指定した <see cref="Vector2"/ との角度をラジアンとして取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Radian GetRad(this in Vector2 from, in Vector2 to)
        {
            return MathfUtil.GetRad(from, to);
        }

        /// <summary>
        /// 指定した <see cref="Vector2"/> との角度を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Degree GetDeg(this in Vector2 from, in Vector2 to)
        {
            return MathfUtil.GetDeg(from, to);
        }

        /// <summary>
        /// 指定した <see cref="Vector2"/> との距離を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(this in Vector2 from, in Vector2 to)
        {
            return Vector2.Distance(from, to);
        }

        /// <summary>
        /// <see cref="Vector3"/> を <see cref="Vector2"/> に変換します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(in this Vector3 self) => self;

        /// <summary>
        /// <see cref="Vector2"/> を <see cref="Vector3"/> に変換します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToVector3(in this Vector2 self) => self;

        /// <summary>
        /// x - y の間のランダムな値を取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(in this Vector2 self)
        {
            return UnityEngine.Random.Range(self.x, self.y);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetX(this ref Vector2 self, float x) => self.Set(x, self.y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetY(this ref Vector2 self, float y) => self.Set(self.x, y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetX(this ref Vector3 self, float x) => self.Set(x, self.y, self.z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetY(this ref Vector3 self, float y) => self.Set(self.x, y, self.z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetZ(this ref Vector3 self, float z) => self.Set(self.x, self.y, z);
    }
}
