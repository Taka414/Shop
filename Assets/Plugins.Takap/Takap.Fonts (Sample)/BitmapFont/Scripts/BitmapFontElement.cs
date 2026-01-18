//
// (C) 2021 Takap.
//

using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
#endif

namespace Takap.Utility.Fonts
{
    /// <summary>
    /// ビットマップフォントの子要素を表します。
    /// </summary>
    public class BitmapFontElement : MonoBehaviour
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField, ReadOnly] SpriteRenderer _sr;
        [SerializeField, ReadOnly] Transform _transform;
        [SerializeField, ReadOnly] string _text = "";
        [SerializeField] bool _isDestorySprite;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 表示する画像の文字を取得します。
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// キャッシュされた <see cref="Transform"/> を取得します。
        /// </summary>
        public Transform Transform => _transform;

        /// <summary>
        /// キャッシュされた <see cref="SpriteRenderer"/> を取得します。
        /// </summary>
        public SpriteRenderer SpriteRenderer => _sr;

        /// <summary>
        /// 画像の大きさを取得します。
        /// </summary>
        public Bounds Bounds => _sr.bounds;

        /// <summary>
        /// 画像の色を取得します。
        /// </summary>
        public Color Color { get => _sr.color; set => _sr.color = value; }

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            if (!_sr)
            {
                this.SetComponent(ref _sr);
                _transform = _sr.transform;
            }
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        public void Init(SpriteRenderer sr)
        {
            _transform = sr.transform;
            _sr = sr;
        }

        /// <summary>
        /// 画像に対応する文字を設定します。
        /// </summary>
        public void SetText(string text)
        {
            _text = text;
        }

        /// <summary>
        /// 表示する画像を設定します。
        /// </summary>
        public void SetSprite(Sprite sp, bool isDestorySprite)
        {
            if (!sp)
            {
                return;
            }

            Setup();

            if (_isDestorySprite)
            {
                Sprite spCurrent = _sr.sprite;
                if (spCurrent)
                {
                    //Log.Trace($"Destory Sprite={spCurrent.name}", this);
                    ObjectUtil.SafeDestroyWithEditor(spCurrent);
                }
            }
            _isDestorySprite = isDestorySprite;
            _sr.sprite = sp;

            this.SetDirty();
        }
    }
}