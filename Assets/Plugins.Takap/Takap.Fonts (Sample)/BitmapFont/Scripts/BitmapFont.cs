//
// (c) 2022 Takap.
//

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Takap.Utility;
using UnityEngine;

namespace Takap.Utility.Fonts
{
    /// <summary>
    /// <see cref="BitmapFontSetting"/> をもとにビットマップフォントを作成します。
    /// </summary>
    public class BitmapFont : MonoBehaviour
    {
        //
        // インゲーム(UI以外のゲーム中)で表示するテキストを表すクラス
        //

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="BitmapFont"/> のテキスト揃えを表します。
        /// </summary>
        public enum BitmapTextAlingment
        {
            /// <summary>中央揃え</summary>
            Center = 0,
            /// <summary>左揃え</summary>
            Left,
            /// <summary>右揃え</summary>
            Right,
        }

        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // 描画元のフォントデータ(動的切り出し)
        [SerializeField] BitmapFontSetting _fontSource;
        // 文字表示の倍率
        [SerializeField] float _scale = 1.0f;
        // カーニング
        [SerializeField] float _kerning;
        // 表示方法
        [SerializeField] BitmapTextAlingment _alingment = BitmapTextAlingment.Center;
        // 表示するテキスト
        [SerializeField] string _text = "";
        // 表示する色
        [SerializeField] Color _color = Color.white;
        // 文字に適用するマテリアル
        [SerializeField] Material _textMaterial;
        // 文字の大きさ
        [SerializeField] float _pixelsPerUnit = 100;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        BitmapFontCache _bitmapFontCache;
        [VContainer.Inject]
        public void Construct(BitmapFontCache bitmapFontCache)
        {
            Validator.SetValueIfThrowNull(ref _bitmapFontCache, bitmapFontCache);
        }

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このオブジェクトが表示してる文字列を設定または取得します。
        /// </summary>
        public string Text
        {
            get => _text;
            set => SetText(value);
        }

        /// <summary>
        /// アライメントを設定または取得します。
        /// </summary>
        public BitmapTextAlingment Alingment
        {
            get => _alingment;
            set
            {
                _alingment = value;
                UpdateAlignment();
            }
        }

        /// <summary>
        /// カーニングを設定または取得します
        /// </summary>
        public float Kerning
        {
            get => _kerning;
            set
            {
                _kerning = value;
                UpdateAlignment();
            }
        }

        /// <summary>
        /// 画像の色を設定または取得します。
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateColorChilds();
            }
        }

        // 子要素のキャッシュ
        List<BitmapFontElement> _childs;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        //public BitmapFont_v2()
        //{
        //    Log.Trace("ctor BitmapFont_v2", this);
        //}

        void Start()
        {
            //Log.Trace("Awake BitmapFont_v2", this);
            //Log.Trace($"FontName={_fontSource.name}");

            SetupChildInfo();
            SetText(_text);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += OnValidateCore;
        }

        void OnValidateCore()
        {
            if (this == null)
            {
                return;
            }

            SetupChildInfo();

            SetText(_text);
            UpdateScaleChilds();
            UpdateAlignment();
            UpdateColorChilds();
        }
#endif

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したテキストで表示を更新します。
        /// </summary>
        [Button]
        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                RemoveChilds();
                return;
            }

            if (!NeedsUpdate(text))
            {
                //Log.Trace("SetText skiped.");
                return;
            }

            UpdateNumberOfChilds(text.Length);

            //Log.Trace($"UseCache={UseCache()}");

            for (int i = 0; i < text.Length; i++)
            {
                string cStr = text[i].ToString();
                BitmapFontElement item = _childs[i];
                if (item.Text == cStr)
                {
                    continue;
                }

                item.SetText(cStr);
                item.name = "Text_" + i.ToString("D3") + "_" + cStr;
                item.SetText(cStr);

                if (UseCache())
                {
                    if (!_bitmapFontCache.TryGetSprite(_fontSource.name, cStr, _pixelsPerUnit, out Sprite sp))
                    {
                        sp = _fontSource.GetSprite(cStr, _pixelsPerUnit);
                        _bitmapFontCache.AddSprite(_fontSource.name, cStr, _pixelsPerUnit, sp);
                    }
                    item.SetSprite(sp, false);
                }
                else
                {
                    Sprite sp = _fontSource.GetSprite(cStr, _pixelsPerUnit);
                    item.SetSprite(sp, true);
                }
            }

            _text = text;
            UpdateAlignment();
            UpdateColorChilds();
        }

        /// <summary>
        /// 現在管理している文字のオブジェクトを全て取得します。
        /// </summary>
        public IEnumerable<BitmapFontElement> GetChildElements(bool isReverse)
        {
            int count = _childs.Count;
            if (!isReverse)
            {
                for (int i = 0; i < count; i++)
                {
                    yield return _childs[i];
                }
            }
            else
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    yield return _childs[i];
                }
            }
        }

        //
        // Non-Public methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 子要素の管理リストを初期化します。
        /// </summary>
        void SetupChildInfo()
        {
            if (_childs != null)
            {
                return;
            }
            _childs = new(); // 無ければ作って初期化

            //Log.Trace("InitChildInfo", this);

            _childs.Clear();
            foreach (BitmapFontElement item in this.GetChilds<BitmapFontElement>())
            {
                _childs.Add(item);
            }
        }

        /// <summary>
        /// 子要素を作成します。
        /// </summary>
        BitmapFontElement CreateBitmapFontItem()
        {
            GameObject go = new();
            go.SetParent(this);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            if (_textMaterial)
            {
                sr.material = _textMaterial;
            }

            BitmapFontElement bi = go.AddComponent<BitmapFontElement>();
            bi.Init(sr);
            return bi;
        }

        /// <summary>
        /// 指定した個数に子要素数を調整します。
        /// </summary>
        void UpdateNumberOfChilds(int count)
        {
            if (count == _childs.Count)
            {
                return;
            }
            else if (count > _childs.Count)
            {
                // 増やす
                int newItems = count - _childs.Count;
                for (int i = 0; i < newItems; i++)
                {
                    _childs.Add(CreateBitmapFontItem());
                }
            }
            else
            {
                // 減らす
                int dim = _childs.Count - count;
                for (int i = 0; i < dim; i++)
                {
                    BitmapFontElement item = _childs.RemoveLast();
                    ObjectUtil.SafeDestroyWithEditor(item.gameObject);
                }
            }
        }

        /// <summary>
        /// キャッシュを使用するかどうかを取得します。
        /// </summary>
        bool UseCache()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                return true; // 実行中はキャッシュを使用する
            }
            else
            {
                return false; // 編集中はキャッシュを使用しない
            }
#else
            return true; // 実機で実行中はキャッシュを使用する
#endif
        }

        /// <summary>
        /// 子要素の更新が必要かどうかを確認します。
        /// </summary>
        bool NeedsUpdate(string text)
        {
            // キャッシュが構成されている前提

            if (_childs.Count != text.Length)
            {
                return true; // 長さが違えば更新する
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].ToString() != _childs[i].Text)
                {
                    return true;
                }
            }

            return false; // 更新の必要なし
        }

        /// <summary>
        /// 子要素のブジェクトを全て削除します。
        /// </summary>
        void RemoveChilds()
        {
            foreach (GameObject item in this.GetChilds().ToArray())
            {
                ObjectUtil.SafeDestroyWithEditor(item); // 無関係なオブジェクトも全て削除
            }
            _childs.Clear();
        }

        /// <summary>
        /// アライメントを更新します。
        /// </summary>
        void UpdateAlignment()
        {
            if (_childs.Count == 0)
            {
                return;
            }

            float[] sizeList = new float[_text.Length];
            float previousXPos = 0;

            for (int i = 0; i < _childs.Count; i++)
            {
                BitmapFontElement item = _childs[i];
                Transform trans = item.Transform;

                sizeList[i] = item.Bounds.size.x;

                if (i == 0)
                {
                    float x = GetBeginPos();
                    trans.SetLocalPosXY(x, 0);
                    previousXPos = x;
                }
                else
                {
                    float x = previousXPos + (sizeList[i - 1] / 2.0f) + (sizeList[i] / 2.0f) + _kerning;
                    trans.SetLocalPosX(x);
                    previousXPos = x;
                }
            }
        }

        /// <summary>
        /// 文字の開始位置を取得します。
        /// </summary>
        float GetBeginPos()
        {
            switch (_alingment)
            {
                case BitmapTextAlingment.Center:
                {
                    return (-(GetTotalWidth() / 2f)) + (_childs[0].Bounds.size.x / 2f);
                }
                case BitmapTextAlingment.Left:
                {
                    return _childs[0].Bounds.size.x / 2f;
                }
                case BitmapTextAlingment.Right:
                {
                    return -GetTotalWidth() + (_childs[_childs.Count - 1].Bounds.size.x / 2f);
                }
                default:
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 子要素の全体幅を取得します。
        /// </summary>
        float GetTotalWidth()
        {
            float w = 0;
            for (int i = 0; i < _childs.Count; i++)
            {
                w += _childs[i].Bounds.size.x;
            }

            float kerningW = 0;
            if (_childs.Count >= 2 && _kerning != 0)
            {
                kerningW = _kerning * (_childs.Count - 1);
            }
            return w + kerningW;
        }

        /// <summary>
        /// 子要素のスケールを変更します。
        /// </summary>
        void UpdateScaleChilds()
        {
            foreach (BitmapFontElement item in _childs)
            {
                Transform tr = item.Transform;
                tr.SetLocalScaleXY(_scale);
            }

            UpdateAlignment();
        }

        /// <summary>
        /// 子要素の色を変更します。
        /// </summary>
        void UpdateColorChilds()
        {
            foreach (BitmapFontElement item in _childs)
            {
                item.Color = _color;
            }
        }
    }
}