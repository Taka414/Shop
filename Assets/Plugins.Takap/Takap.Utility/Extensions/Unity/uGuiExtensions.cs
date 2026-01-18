//
// (C) 2022 Takap.
//

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// 画面とか座標の
    /// </summary>
    public static class uGuiExtensions
    {
        /// <summary>
        /// <see cref="PointerEventData"/> からワールド座標を取得します。
        /// </summary>
        public static Vector3 GetWorldPoint(this PointerEventData self, Canvas canvas)
        {
            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                {
                    RectTransform rect = self.pointerCurrentRaycast.gameObject.transform as RectTransform;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, self.position, canvas.worldCamera, out Vector3 worldPoint);
                    return worldPoint;
                }
                case RenderMode.ScreenSpaceCamera:
                {
                    return self.pointerCurrentRaycast.worldPosition;
                }

                case RenderMode.WorldSpace:
                default: throw new System.NotSupportedException($"このモードはサポートしていません。Mode={canvas.renderMode}");
            }
        }

        /// <summary>
        /// <see cref="PointerEventData"/> からUIローカル位置を取得します。
        /// </summary>
        public static Vector2 GetLocalPoint(this PointerEventData self, Graphic target)
        {
            Vector2 screenPoint = self.position;
            return uGuiUtil.GetLocalPoint(screenPoint, target);
            //Camera cam = target.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : target.canvas.worldCamera;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(target.rectTransform, screenPoint, cam, out Vector2 localPoint);
            //return localPoint;
        }

        /// <summary>
        /// <see cref="PointerEventData"/> からUIローカル位置を取得します。
        /// </summary>
        public static Vector2 GetLocalPoint(this PointerEventData self, RectTransform target, Canvas canvas)
        {
            Vector2 screenPoint = self.position;
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(target, screenPoint, cam, out Vector2 localPoint);
            return localPoint;
        }

        /// <summary>
        /// 指定したUI要素上でワールド座標を取得します。
        /// </summary>
        public static Vector3 GetWorldPoint(this Graphic self, in Vector2 screenPos)
        {
            if (self.canvas.renderMode == RenderMode.WorldSpace)
            {
                throw new System.NotSupportedException($"このモードはサポートしていません。Mode={self.canvas.renderMode}");
            }

            Camera cam = self.canvas.renderMode != RenderMode.ScreenSpaceOverlay ? null : self.canvas.worldCamera;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(self.rectTransform, screenPos, cam, out Vector3 worldPoint);
            return worldPoint;
        }

        /// <summary>
        /// ワールド座標をスクリーン座標に変換します。
        /// </summary>
        public static Vector2 GetScreenPoint(this Vector3 worldPoint, Canvas canvas)
        {
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
        }

        /// <summary>
        /// ワールド座標をUIのローカル座標に変換します。
        /// </summary>
        public static Vector2 GetLocalPoint(this Graphic self, in Vector3 worldPoint)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(self.canvas.worldCamera, worldPoint);

            Camera cam = self.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : self.canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(self.rectTransform, screenPoint, cam, out Vector2 localPoint);

            return localPoint;
        }

        /// <summary>
        /// ワールド座標をUIのローカル座標に変換します。
        /// </summary>
        public static Vector2 GetLocalPoint(this RectTransform self, Canvas canvas, in Vector3 worldPoint)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPoint);

            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(self, screenPoint, cam, out Vector2 localPoint);

            return localPoint;
        }

        /// <summary>
        /// ワールド座標をUIのローカル座標に変換します。
        /// </summary>
        public static Vector2 GetLocalPoint(this in Vector3 worldPoint, RectTransform targetRect, Canvas canvas)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPoint);

            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, screenPoint, cam, out Vector2 localPoint);

            return localPoint;
        }

        // - - - - - - - - - -

        // 動作未確認
        public static Vector2 ToUiLocalPos(this PointerEventData self, Component component, Canvas canvas = null)
        {
            return ToUiLocalPos(self, component.gameObject, canvas);
        }
        // 動作未確認
        public static Vector2 ToUiLocalPos(this PointerEventData self, GameObject gameObject, Canvas canvas = null)
        {
            //Vector2 screenPoint = self.position;
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.transform as RectTransform, self.position, cam, out Vector2 localPoint);
            return localPoint;
        }

        /// <summary>
        /// UI上のローカル座標に変換します。
        /// </summary>
        public static Vector2 GetUiLocalPos(this Transform self, Component component, Canvas canvas = null)
        {
            return GetUiLocalPos(self, component.gameObject, canvas);
        }
        public static Vector2 GetUiLocalPos(this Transform self, GameObject gameObject, Canvas canvas = null)
        {
            return GetUiLocalPos(self.position, gameObject, canvas);
        }
        public static Vector2 GetUiLocalPos(this in Vector3 worldPos, GameObject gameObject, Canvas canvas = null)
        {
            var rect = gameObject.transform.parent as RectTransform;
            if (!rect)
            {
                throw new ArgumentException("UIオブジェクトでないものはparentに指定できません。");
            }

            // 指定されてなければ取得する(若干効率が悪くなる)
            Canvas tempCanvas = canvas ? canvas : gameObject.GetComponentInParent<Canvas>();
            if (!tempCanvas)
            {
                throw new ArgumentException("指定された要素からCanvasが取得できませんでした。");
            }

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(tempCanvas.worldCamera, worldPos/*<-WorldPosition*/);
            Camera cam = tempCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : tempCanvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, cam, out Vector2 result);
            return result;
        }
    }

    public static class uGuiUtil
    {
        /// <summary>
        /// <see cref="PointerEventData"/> からUIローカル位置を取得します。
        /// </summary>
        public static Vector2 GetLocalPoint(in Vector2 screenPoint, Graphic target)
        {
            //Vector2 screenPoint = self.position;
            Camera cam = target.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : target.canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(target.rectTransform, screenPoint, cam, out Vector2 localPoint);
            return localPoint;
        }
    }
}