//
// (c) 2022 Takap.
//

using System;
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
    /// <remarks>
    /// <para>1. <see cref="BitmapFont"/> との違い、一度作成した子要素はキャッシュして使いまわすようにしている。</para>
    /// <para>2. ゲーム中に動的にオブジェクトを生成する用途を想定しているんで静的な文字列表示には使用しない。</para>
    /// </remarks>
    public class BitmapFontFullCache : MonoBehaviour
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
        [SerializeField, ReadOnly] string _text = "";
        // 表示する色
        [SerializeField] Color _color = Color.white;
        // 文字に適用するマテリアル
        [SerializeField] Material _textMaterial;
        // 文字の大きさ
        [SerializeField] float _pixelsPerUnit = 100;
        // 最初に確保する子要素の数
        [SerializeField] int _defaultChildCount = 4;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        Transform _transformSelf;
        
        // SetSortingLayerで最後に指定された名前
        string _previousLayerName;
        
        // SetOrderInLayerで最後に指定された名前
        int _previousSoringOrder = -1;


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
        [ShowInInspector, ReadOnly]
        List<BitmapFontElement> _childs;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        void Awake()
        {
            //Log.Trace("Awake BitmapFont_v2", this);
            //Log.Trace($"FontName={_fontSource.name}");

            _transformSelf = transform;

            SetupChildInfo();

            if (!_fontSource)
            {
                Log.Warn("Not set fontSource.", this);
                return;
            }

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

            if (!UnityEditor.PrefabUtility.IsPartOfNonAssetPrefabInstance(this))
            {
                // プレハブ化してインスペクターに設定している状態でシーンを開くと
                // 変なところにゴミが生成されるのを防ぐための判定分
                //  → プレハブの時はNG/インスタンス化したプレハブの時はOKと判定している
                return;
            }

            SetupChildInfo();

            if (!_fontSource)
            {
                Log.Warn("Not set fontSource.", this);
                return;
            }

            SetText(_text);
            UpdateScaleChilds();
            UpdateAlignment();
            UpdateColorChilds();
        }
#endif

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        [Button]
        public void Reset()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                return;
            }
            _childs?.Clear();
            _childs = null;
            SetupChildInfo();
#endif
        }

        /// <summary>
        /// 指定したテキストで表示を更新します。
        /// </summary>
        [Button]
        public void SetText(string text)
        {
            if (text == null)
            {
                text = "";
            }

            //Log.Debug($"SetTest\r\n{Environment.StackTrace}", this);

            DisableChilds(text);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            //if (!NeedsUpdate(text))
            //{
            //    //Log.Trace("SetText skiped.");
            //    return;
            //}

            //UpdateNumberOfChilds(text.Length);

            //Log.Trace($"UseCache={UseCache()}");

            for (int i = 0; i < text.Length; i++)
            {
                if (i >= _childs.Count())
                {
                    var elem = CreateBitmapFontItem();
                    elem.name = $"Text_{i:D3}";
                    _childs.Add(elem);
                }

                string cStr = text[i].ToString();
                BitmapFontElement item = _childs[i];
                if (item.Text == cStr)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        item.gameObject.SetActive(true);
                    }
                    continue;
                }
                else
                {
                    item.SetText(cStr);
                    item.gameObject.SetActive(true);
                }

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
        public IEnumerable<BitmapFontElement> GetChildElements(bool isReverse = false)
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

        /// <summary>
        /// 生成済みの子要素に SortingLayer を設定します。
        /// </summary>
        public void SetSortingLayer(string layerName)
        {
            if (_childs == null)
            {
                Log.Warn("Not init childs.", this);
                return;
            }
            if (_previousLayerName == layerName)
            {
                return; // 何回も同じ値で初期化しない
            }
            _previousLayerName = layerName;

            foreach (var child in _childs)
            {
                child.SpriteRenderer.sortingLayerName = layerName;
            }
        }

        /// <summary>
        /// 生成済みの子要素に Order in Layer を設定します。
        /// </summary>
        public void SetOrderInLayer(int value)
        {
            if (_childs == null)
            {
                Log.Warn("Not init childs.", this);
                return;
            }
            if (_previousSoringOrder == value)
            {
                return; // 何回も同じ値で初期化しない
            }
            _previousSoringOrder = value;

            foreach (var child in _childs)
            {
                child.SpriteRenderer.sortingOrder = value;
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

            // 子要素の中で無効な物は削除してキャッシュを構築する
            foreach (GameObject childGameObject in this.GetChilds())
            {
                var elem = childGameObject.GetComponent<BitmapFontElement>();
                if (!elem)
                {
                    ObjectUtil.SafeDestroyWithEditor(elem);
                }
                else
                {
                    _childs.Add(elem);
                    InitBitmapFontItem(elem);
                }
            }

            // あらかじめプールしておくオブジェクトの生成
            if (_childs.Count < _defaultChildCount)
            {
                for (int i = _childs.Count; i < _defaultChildCount; i++)
                {
                    var item = CreateBitmapFontItem();
                    item.gameObject.SetActive(false);
                    item.Transform.SetLocalPosXY(0, 0);
                    item.name = $"Text_{i:D3}";
                }
            }

            // 初期値の個数より多い子要素がある場合削除する
            for (int i = _childs.Count; i > _defaultChildCount; i--)
            {
                ObjectUtil.SafeDestroyWithEditor(_childs[i - 1].gameObject);
                _childs.RemoveLast();
            }
        }

        /// <summary>
        /// 子要素のキャッシュを生成するときの初期化処理を行います。
        /// </summary>
        protected virtual void InitBitmapFontItem(BitmapFontElement bi)
        {
            // nop
        }

        /// <summary>
        /// 子要素を作成します。
        /// </summary>
        BitmapFontElement CreateBitmapFontItem()
        {
            if (!transform)
            {
                Log.Debug("Unknwon state for transform", this);
                return null;
            }

            GameObject go = new();
            go.SetParent(this);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            if (_textMaterial)
            {
                sr.material = _textMaterial;
            }

            BitmapFontElement bi = go.AddComponent<BitmapFontElement>();
            bi.Init(sr);

            CreateBitmapFontItem(bi);

            return bi;
        }

        /// <summary>
        /// 生成後の初期化処理を行います。
        /// </summary>
        protected virtual void CreateBitmapFontItem(BitmapFontElement bi)
        {
            // nop
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

            //if (_childs.Count != text.Length)
            //{
            //    return true; // 長さが違えば更新する
            //}

            for (int i = 0; i < text.Length; i++)
            {
                if (!_childs[i].gameObject.activeSelf)
                {
                    return true; // 非表示になっている
                }

                if (text[i].ToString() != _childs[i].Text)
                {
                    return true; // 内容の文字列が違う
                }
            }

            return false; // 更新の必要なし
        }

        /// <summary>
        /// 子要素のブジェクトを全て削除します。
        /// </summary>
        void DisableChilds(string text)
        {
            int i = 0;
            foreach (GameObject item in this.GetChilds())
            {
                //ObjectUtil.SafeDestroy(item); // 無関係なオブジェクトも全て削除
                item.SetActive(i++ < text.Length);
            }
            //_childs.Clear();
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
                if (!_childs[i].gameObject.activeSelf)
                {
                    continue;
                }

                BitmapFontElement item = _childs[i];
                if (!item.gameObject.activeSelf)
                {
                    continue;
                }
                Transform trans = item.Transform;

                if (!_transformSelf)
                {
                    _transformSelf = transform;
                }

                //sizeList[i] = item.Bounds.size.x;
                sizeList[i] = item.Bounds.size.x / _transformSelf.GetLocalScaleX();
                //Log.Trace($"Size[i]={item.Bounds.size.x}, LocalScale={trans.GetLocalScale()}");

                if (i == 0)
                {
                    float x = GetBeginPos();
                    trans.SetLocalPosXY(x, 0);
                    previousXPos = x;
                }
                else
                {
                    float x = previousXPos + sizeList[i - 1] / 2.0f + sizeList[i] / 2.0f + _kerning;
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
                    return (-(GetTotalWidth() / 2f)) + _childs[0].Bounds.size.x / 2f;
                }
                case BitmapTextAlingment.Left:
                {
                    return _childs[0].Bounds.size.x / 2f;
                }
                case BitmapTextAlingment.Right:
                {
                    return -GetTotalWidth() + _childs[_childs.Count - 1].Bounds.size.x / 2f;
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
            int itemCount = 0;
            float w = 0;
            for (int i = 0; i < _childs.Count; i++)
            {
                if (!_childs[i].gameObject.activeSelf)
                {
                    continue;
                }
                w += _childs[i].Bounds.size.x;
                itemCount++;
            }

            float kerningW = 0;
            if (_childs.Count >= 2 && _kerning != 0)
            {
                kerningW = _kerning * (itemCount - 1);
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
                if (!item.gameObject.activeSelf)
                {
                    continue;
                }
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
                if (!item.gameObject.activeSelf)
                {
                    continue;
                }
                item.Color = _color;
            }
        }
    }
}