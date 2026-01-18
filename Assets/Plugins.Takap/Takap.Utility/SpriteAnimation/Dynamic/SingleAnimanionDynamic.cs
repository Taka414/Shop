//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 動的スライスを利用したスプライトアニメーション用のコンポーネント
    /// </summary>
    public class SingleAnimanionDynamic : SingleAnimanionBase
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - -

        [SerializeField] DynamicSliceTexture _textureSource;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - -

        protected override int GetImageCount() => _textureSource.CellCount;

        protected override Sprite GetSprite(int index) => _textureSource.GetSprite(index);

        private void OnValidate()
        {
            if (_textureSource == null)
            {
                TryGetComponent(out SpriteRenderer sr);
                sr.sprite = null;
            }
            else
            {
                TryGetComponent(out SpriteRenderer sr);
                if (sr.sprite == null)
                {
                    sr.sprite = _textureSource.GetSprite(0);
                }
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 画像を差し替えます。
        /// </summary>
        public override void OnFrameChange(Sprite sp)
        {
            Selector.ChangeImage(sp, out Sprite oldSp);
            if (oldSp)
            {
                Destroy(oldSp); // 動的に生成したSpriteは明示的に破棄しないとリークする
            }
        }

        /// <summary>
        /// [デバッグ用] 強制的に初期表示用の画像を更新します。
        /// </summary>
        [Button]
        public void RefreshImage()
        {
            if (_textureSource == null)
            {
                TryGetComponent(out SpriteRenderer sr);
                sr.sprite = null;
            }
            else
            {
                TryGetComponent(out SpriteRenderer sr);
                sr.sprite = _textureSource.GetSprite(0);
            }
        }
    }
}