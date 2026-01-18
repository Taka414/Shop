//
// (C) 2022 Takap.
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="RectTransformExtensions"/> の機能を拡張します。
    /// </summary>
    public static class RectTransformExtensions
    {
        // 取得(sizeDelta)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetSizeDelta(this RectTransform self)
        {
            return self.sizeDelta;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetWidthDelta(this RectTransform self)
        {
            return self.sizeDelta.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetHeightDelta(this RectTransform self)
        {
            return self.sizeDelta.y;
        }

        // 取得(rect)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetSizeRect(this RectTransform self)
        {
            return new Vector2(self.rect.width, self.rect.height);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetWidthRect(this RectTransform self)
        {
            return self.rect.width;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetHeightRect(this RectTransform self)
        {
            return self.rect.height;
        }

        // 取得
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetWidth(this RectTransform self)
        {
            return self.sizeDelta.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetHeight(this RectTransform self)
        {
            return self.sizeDelta.y;
        }

        // 設定
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(this RectTransform self, in Vector2 size)
        {
            self.sizeDelta = size;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(this RectTransform self, float width, float height)
        {
            Vector2 s = self.sizeDelta;
            s.x = width;
            s.y = height;
            self.sizeDelta = s;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetWidth(this RectTransform self, float width)
        {
            Vector2 s = self.sizeDelta;
            s.x = width;
            self.sizeDelta = s;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetHeight(this RectTransform self, float height)
        {
            Vector2 s = self.sizeDelta;
            s.y = height;
            self.sizeDelta = s;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetHeight(this RectTransform self, in Size2DF size)
        {
            Vector2 s = self.sizeDelta;
            s.x = size.Width;
            s.y = size.Height;
            self.sizeDelta = s;
        }

        /// <summary>
        /// <paramref name="self"/> のサイズを親要素の大きさに合わせます。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FitParent(this RectTransform self)
        {
            RectUtil.FitParent(self);
        }

        /// <summary>
        /// <paramref name="self"/> のサイズを指定した要素の大きさに合わせます。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fit(this RectTransform self, RectTransform target)
        {
            self.SetSize(target.anchoredPosition);
        }

        /// <summary>
        /// アンカーポイントを設定します。
        /// </summary>
        public static void SetAncPos(this RectTransform self, float x, float y)
        {
            Vector2 pos = self.anchoredPosition;
            pos.x = x;
            pos.y = y;
            self.anchoredPosition = pos;
        }

        // ピボット

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPivot(this RectTransform self, in Vector2 pos)
        {
            self.pivot = pos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPivot(this RectTransform self, float x, float y)
        {
            Vector2 pv = self.pivot;
            pv.Set(x, y);
            self.pivot = pv;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPivotX(this RectTransform self, float x)
        {
            Vector2 pv = self.pivot;
            pv.x = x;
            self.pivot = pv;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPivotY(this RectTransform self, float y)
        {
            Vector2 pv = self.pivot;
            pv.y = y;
            self.pivot = pv;
        }

        // アンカー位置

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetAnchoredPos(this RectTransform self)
        {
            return self.anchoredPosition;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAnchoredPosX(this RectTransform self)
        {
            return self.anchoredPosition.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAnchoredPosY(this RectTransform self)
        {
            return self.anchoredPosition.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAnchoredPosZ(this RectTransform self)
        {
            return self.anchoredPosition3D.z;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPos(this RectTransform self, in Vector2 pos)
        {
            self.anchoredPosition = pos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPos(this RectTransform self, float x, float y)
        {
            Vector2 ancPos = self.anchoredPosition;
            ancPos.Set(x, y);
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPosX(this RectTransform self, float x)
        {
            Vector2 ancPos = self.anchoredPosition;
            ancPos.x = x;
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPosY(this RectTransform self, float y)
        {
            Vector2 ancPos = self.anchoredPosition;
            ancPos.y = y;
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAnchoredPosZ(this RectTransform self, float z)
        {
            Vector3 ancPos = self.anchoredPosition3D;
            ancPos.z = z;
            self.anchoredPosition3D = ancPos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddAnchoredPosX(this RectTransform self, float x)
        {
            Vector2 ancPos = self.anchoredPosition;
            ancPos.x += x;
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddAnchoredPosY(this RectTransform self, float y)
        {
            Vector2 ancPos = self.anchoredPosition;
            ancPos.y += y;
            self.anchoredPosition = ancPos;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddAnchoredPosZ(this RectTransform self, float z)
        {
            Vector3 ancPos = self.anchoredPosition3D;
            ancPos.z += z;
            self.anchoredPosition3D = ancPos;
        }

        // アンカーが Strech の時のサイズの指定

        public static void SetAnchoredTop(this RectTransform self, float value)
        {
            Vector2 v = self.offsetMax;
            v.y = -value;
            self.offsetMax = v;
        }
        public static void SetAnchoredBottom(this RectTransform self, float value)
        {
            Vector2 v = self.offsetMin;
            v.y = value;
            self.offsetMin = v;
        }
        public static void SetAnchoredLeft(this RectTransform self, float value)
        {
            Vector2 v = self.offsetMin;
            v.x = -value;
            self.offsetMin = v;
        }
        public static void SetAnchoredRight(this RectTransform self, float value)
        {
            Vector2 v = self.offsetMax;
            v.x = value;
            self.offsetMax = v;
        }
    }
}
