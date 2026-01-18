//
// (C) 2022 Takap.
//

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Graphic"/> の動作を拡張します。
    /// </summary>
    public static class GraphicExtensions
    {
        //
        // Color
        // - - - - - - - - - - - - - - - - - - - -

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this Graphic self, in Color c)
        {
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this Graphic self, float r, float g, float b)
        {
            Color c = self.color;
            c.r = r;
            c.g = g;
            c.b = b;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColor(this Graphic self, float r, float g, float b, float a)
        {
            Color c = self.color;
            c.r = r;
            c.g = g;
            c.b = b;
            c.a = a;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorR(this Graphic self, float r)
        {
            Color c = self.color;
            c.r = r;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorG(this Graphic self, float g)
        {
            Color c = self.color;
            c.g = g;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorB(this Graphic self, float b)
        {
            Color c = self.color;
            c.b = b;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetColorA(this Graphic self, float a)
        {
            Color c = self.color;
            c.a = a;
            self.color = c;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAlpha(this Graphic self, float a)
        {
            Color c = self.color;
            c.a = a;
            self.color = c;
        }
    }
}