//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="SpriteRendererExtensions"/> クラスを拡張します。
    /// </summary>
    public static class SpriteRendererExtensions
    {
        //
        // Color
        // - - - - - - - - - - - - - - - - - - - -

        // Set

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this SpriteRenderer self, Color c)
        {
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this SpriteRenderer self, float r, float g, float b)
        {
            Color c = self.color;
            c.r = r;
            c.g = g;
            c.b = b;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this SpriteRenderer self, float r, float g, float b, float a)
        {
            Color c = self.color;
            c.r = r;
            c.g = g;
            c.b = b;
            c.a = a;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorR(this SpriteRenderer self, float r)
        {
            Color c = self.color;
            c.r = r;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorG(this SpriteRenderer self, float g)
        {
            Color c = self.color;
            c.g = g;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorB(this SpriteRenderer self, float b)
        {
            Color c = self.color;
            c.b = b;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorA(this SpriteRenderer self, float opacity)
        {
            Color c = self.color;
            c.a = opacity;
            self.color = c;
        }

        // Get

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetColorA(this SpriteRenderer self)
        {
            return self.color.a;
        }

        //
        // Size
        // - - - - - - - - - - - - - - - - - - - -

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetBoundsSize(this SpriteRenderer self)
        {
            return self.bounds.size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetBoundsSizeX(this SpriteRenderer self)
        {
            return self.bounds.size.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetBoundsSizeY(this SpriteRenderer self)
        {
            return self.bounds.size.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetBoundsSizeZ(this SpriteRenderer self)
        {
            return self.bounds.size.z;
        }

        // DrawMode == Sliced の時だけ有効のサイズ

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetSize(this SpriteRenderer self)
        {
            return self.size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSizeX(this SpriteRenderer self)
        {
            return self.size.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSizeY(this SpriteRenderer self)
        {
            return self.size.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SetSize(this SpriteRenderer self, in Vector2 size)
        {
            return self.size = size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(this SpriteRenderer self, float x, float y)
        {
            Vector2 vec2 = self.size;
            vec2.x = x;
            vec2.y = y;
            self.size = vec2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSizeX(this SpriteRenderer self, float x)
        {
            Vector2 vec2 = self.size;
            vec2.x = x;
            self.size = vec2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSizeY(this SpriteRenderer self, float y)
        {
            Vector2 vec2 = self.size;
            vec2.y = y;
            self.size = vec2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSizeXY(this SpriteRenderer self, float xy)
        {
            Vector2 vec2 = self.size;
            vec2.x = xy;
            vec2.y = xy;
            self.size = vec2;
        }

        /// <summary>
        /// 指定した画像の4隅の位置を取得します。
        /// </summary>
        public static IEnumerable<Vector2> Get4Corners(this SpriteRenderer sr)
        {
            Vector2 harfSize = sr.bounds.size / 2.0f;
            Vector2 currensPos = sr.transform.GetPos();
            yield return new Vector2(currensPos.x - harfSize.x, currensPos.y + harfSize.y); // tl
            yield return new Vector2(currensPos.x + harfSize.x, currensPos.y + harfSize.y); // tr
            yield return new Vector2(currensPos.x - harfSize.x, currensPos.y - harfSize.y); // bl
            yield return new Vector2(currensPos.x + harfSize.x, currensPos.y - harfSize.y); // br
        }
    }
}