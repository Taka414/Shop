//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using Takap.Utility;
using UnityEngine;

namespace Takap.Samples.SpriteSheet
{
    /// <summary>
    /// <see cref="DynamicSliceTexture"/> のテスト用コンポーネント
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DynamicSliceTextureSourceTest : MonoBehaviour
    {
        [SerializeField, LabelText("表示テクスチャー(スライス)")] DynamicSliceTexture _textureSource;

        private SpriteRenderer sr;

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
                    _textureSource.ChangeSprite(sr, 0);
                }
            }
        }

        private void Awake()
        {
            this.SetComponent(ref sr);
        }

        [Button]
        public void ChangeFrame(int i)
        {
            sr.sprite = _textureSource.GetSprite(i);
        }
    }
}