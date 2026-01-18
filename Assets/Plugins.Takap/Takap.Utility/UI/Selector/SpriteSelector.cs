//
// (C) 2022 Takap.
//

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Image"/> と <see cref="Renderer"/> を同じように扱うためのクラス
    /// </summary>
    public static class SpriteSelector
    {
        /// <summary>
        /// <see cref="ISpriteSelector"/> を取得します。
        /// </summary>
        public static ISpriteSelector GetInstance(Component src) => GetInstance(src.gameObject);

        /// <summary>
        /// <see cref="ISpriteSelector"/> を取得します。
        /// </summary>
        public static ISpriteSelector GetInstance(GameObject src)
        {
            Image img = src.GetComponent<Image>();
            if (img)
            {
                return new ImageSelector(img);
            }

            SpriteRenderer sr = src.GetComponent<SpriteRenderer>();
            if (sr)
            {
                return new SpriteRendererSelector(sr);
            }

            throw new NotSupportedException("Require 'Image' or 'SpriteRenderer'.");
        }
    }

    /// <summary>
    /// <see cref="Image"/> と <see cref="Renderer"/> を同じように扱うためのインタフェース
    /// </summary>
    public interface ISpriteSelector
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="UnityEngine.Sprite"/> を設定または取得します。
        /// </summary>
        Sprite Sprite { get; set; }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        void ChangeImage(Sprite sp);

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        void ChangeImage(Sprite sp, out Sprite oldSprite);

        /// <summary>
        /// マテリアルをオブジェクトに設定します。
        /// </summary>
        void SetMaterial(Material material);

        /// <summary>
        /// Xを反転するかどうか設定します。
        /// </summary>
        void SetFlipX(bool flag);
        
        /// <summary>
        /// Yを反転するかどうか設定します。
        /// </summary>
        void SetFlipY(bool flag);
    }

    public class SpriteRendererSelector : ISpriteSelector
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private SpriteRenderer _sr;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="UnityEngine.Sprite"/> を取得します。
        /// </summary>
        public Sprite Sprite { get => _sr.sprite; set => _sr.sprite = value; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した要素でオブジェクトを生成します。
        /// </summary>
        public SpriteRendererSelector(SpriteRenderer sr) => _sr = sr;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        public void ChangeImage(Sprite sp)
        {
            _sr.sprite = sp;
        }

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        public void ChangeImage(Sprite sp, out Sprite oldSprite)
        {
            oldSprite = _sr.sprite;
            _sr.sprite = sp;
        }

        /// <summary>
        /// マテリアルをオブジェクトに設定します。
        /// </summary>
        public void SetMaterial(Material material)
        {
            _sr.material = material;
        }

        public void SetFlipX(bool flag)
        {
            _sr.flipX = flag;
        }

        public void SetFlipY(bool flag)
        {
            _sr.flipY = flag;
        }
    }

    public class ImageSelector : ISpriteSelector
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private Image image;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="UnityEngine.Sprite"/> を取得します。
        /// </summary>
        public Sprite Sprite { get => image.sprite; set => image.sprite = value; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した要素でオブジェクトを生成します。
        /// </summary>
        public ImageSelector(Image image) => this.image = image;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        public void ChangeImage(Sprite sp)
        {
            image.sprite = sp;
        }

        /// <summary>
        /// 指定した <see cref="UnityEngine.Sprite"/> で表示を更新します。
        /// </summary>
        public void ChangeImage(Sprite sp, out Sprite oldSprite)
        {
            oldSprite = image.sprite;
            image.sprite = sp;
        }

        /// <summary>
        /// マテリアルをオブジェクトに設定します。
        /// </summary>
        public void SetMaterial(Material material)
        {
            image.material = material;
        }

        public void SetFlipX(bool flag)
        {
            RectTransform rect = image.rectTransform;
            Quaternion rotation = rect.rotation;
            if (flag)
            {
                rotation.y = 0;
            }
            else
            {
                rotation.y = 180;
            }
            rect.rotation = rotation;
        }

        public void SetFlipY(bool flag)
        {
            RectTransform rect = image.rectTransform;
            Quaternion rotation = rect.rotation;
            if (flag)
            {
                rotation.x = 0;
            }
            else
            {
                rotation.x = 180;
            }
            rect.rotation = rotation;
        }
    }
}
