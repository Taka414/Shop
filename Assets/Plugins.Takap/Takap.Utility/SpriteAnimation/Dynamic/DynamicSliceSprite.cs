//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 動的に切り出した画像を安全に運用するためのクラス
    /// </summary>
    //[RequireComponent(typeof(SpriteRenderer))]
    public class DynamicSliceSprite : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // もとになる画像
        [SerializeField] DynamicSliceTexture _textureSource;
        // 表示する画像の番号
        [SerializeField, MinValue(0)] int _imageNo;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        ISpriteSelector _selector;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -



        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _selector);
            //this.selector = SpriteSelector.GetInstance(this);
        }

        private void OnValidate()
        {
            if (_textureSource == null)
            {
                return;
            }
            if (_imageNo < 0)
            {
                return;
            }
            if (_imageNo > _textureSource.CellCount)
            {
                return; // 最大を超えている場合何もしない
            }
            if (!_textureSource.HasImage(_imageNo))
            {
                return;
            }

            ISpriteSelector selector = null;
            this.SetComponent(ref selector);

            selector.Sprite = _textureSource.GetSprite(_imageNo);
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void ChangeTextureSource(DynamicSliceTexture textureSource, int index = 0)
        {
            _textureSource = textureSource;
            _textureSource.ChangeSprite(_selector, index);
        }

        //
        // Othor Methods
        // - - - - - - - - - - - - - - - - - - - -


    }
}