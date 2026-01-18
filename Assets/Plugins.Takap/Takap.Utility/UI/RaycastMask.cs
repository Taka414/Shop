//
// (C) 2022 Takap.
//

using UnityEngine;
using UnityEngine.UI;

// 
// 画像の透明な部分を無視したタッチ判定を出すためのスクリプト
//
// 参考:
// http://ryu955.hatenablog.com/entry/2018/02/20/002207
// 

namespace Takap.Utility
{
    /// <summary>
    /// 透明な部分はタッチできないようにすコンポーネント
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class RaycastMask : MonoBehaviour, ICanvasRaycastFilter
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private Image _img;
        private Sprite _sp;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Start()
        {
            _img = GetComponent<Image>();
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            _sp = _img.sprite;

            var rectTransform = transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, sp, eventCamera, out Vector2 localPositionPivotRelative);

            // 左下が0の座標に変換
            var localPosition =
                new Vector2(localPositionPivotRelative.x + rectTransform.pivot.x * rectTransform.rect.width,
                    localPositionPivotRelative.y + rectTransform.pivot.y * rectTransform.rect.height);

            Rect spriteRect = _sp.textureRect;
            Rect maskRect = rectTransform.rect;

            int x;
            int y;

            // convert to texture space
            switch (_img.type)
            {
                case Image.Type.Sliced:
                {
                    Vector4 border = _sp.border;
                    // x slicing
                    if (localPosition.x < border.x)
                    {
                        x = Mathf.FloorToInt(spriteRect.x + localPosition.x);
                    }
                    else if (localPosition.x > maskRect.width - border.z)
                    {
                        x = Mathf.FloorToInt(spriteRect.x + spriteRect.width - (maskRect.width - localPosition.x));
                    }
                    else
                    {
                        x = Mathf.FloorToInt(spriteRect.x + border.x +
                                             ((localPosition.x - border.x) /
                                             (maskRect.width - border.x - border.z)) *
                                             (spriteRect.width - border.x - border.z));
                    }
                    // y slicing
                    if (localPosition.y < border.y)
                    {
                        y = Mathf.FloorToInt(spriteRect.y + localPosition.y);
                    }
                    else if (localPosition.y > maskRect.height - border.w)
                    {
                        y = Mathf.FloorToInt(spriteRect.y + spriteRect.height - (maskRect.height - localPosition.y));
                    }
                    else
                    {
                        y = Mathf.FloorToInt(spriteRect.y + border.y +
                                             ((localPosition.y - border.y) /
                                             (maskRect.height - border.y - border.w)) *
                                             (spriteRect.height - border.y - border.w));
                    }
                    break;
                }
                case Image.Type.Simple:
                default:
                {
                    // conversion to uniform UV space
                    x = Mathf.FloorToInt(spriteRect.x + spriteRect.width * localPosition.x / maskRect.width);
                    y = Mathf.FloorToInt(spriteRect.y + spriteRect.height * localPosition.y / maskRect.height);
                    break;
                }
            }

            // destroy component if texture import settings are wrong
            try
            {
                return _sp.texture.GetPixel(x, y).a > 0;
            }
            catch (UnityException)
            {
                Debug.LogError("Mask texture not readable, set your sprite to Texture Type 'Advanced' and check 'Read/Write Enabled'");
                Destroy(this);
                return false;
            }
        }
    }
}