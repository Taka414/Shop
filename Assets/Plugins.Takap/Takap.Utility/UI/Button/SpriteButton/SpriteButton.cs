//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap.Utility
{

    /// <summary>
    /// 1枚のボタン画像で押したりする表現をするボタンを表します。
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FlipImage))]
    public class SpriteButton : ButtonBase
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // ボタンの設定
        [SerializeField, InlineEditor(Expanded = true)] SpriteButtonParam _param;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // [デバッグ用] ボタン押したときのサンプル表示
        // true: 押したときの表示 / false: 通常表示
        [ShowInInspector, LabelText("Pressed (for Debug)")] bool _pressed;

        // このオブオブジェクト
        Image _image;
        FlipImage _flip;
        // 子要素のオブジェクト
        RectTransform _childRect;
        TextMeshProUGUI _childText;

        // テキストに押したときの効果を適用したかどうか
        // true: 適用した / false: してない
        bool _textPressed;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        protected override void Awake()
        {
            this.SetComponent(ref _image);
            this.SetComponent(ref _flip);
            this.SetChildComponent(ref _childRect);
            this.SetChildComponent(ref _childText);
        }

        // デバッグ中にScriptableObjectの変更をプレビューしたいだけなので実機では動かさない
#if UNITY_EDITOR
        private void Update()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                _UpdateButton(_pressed);
            }
        }
#endif

        protected override void PointerDown(PointerEventData e)
        {
            _pressed = true;
            _UpdateButton(Pressed);
        }

        protected override void PointerUp(PointerEventData e)
        {
            _pressed = false;
            _UpdateButton(Normal);
        }

        protected override void PointerExit(PointerEventData e)
        {
            _pressed = false;
            _UpdateButton(Normal);
        }

        protected override void PointerEnter(PointerEventData e)
        {
            _pressed = true;
            _UpdateButton(Pressed);
        }

        protected override void OnChangeButtonInteractable(bool interactable)
        {
            _UpdateButton(Normal);
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        // 初期設定
        private void _UpdateButton(bool btnPressed)
        {
            if (!_param) // 未設定の時のデフォルト表示
            {
                SetDefault();
                return;
            }

            if (Interactable)
            {
                if (!btnPressed)
                {
                    SetNormalColor();
                    SetNormalText();
                }
                else
                {
                    SetPressColor();
                    SetPressText();
                }
            }
            else
            {
                if (!_param.DisableParam)
                {
                    return;
                }

                _image.sprite = _param.DisableParam.Sprite;
                _image.color = _param.DisableParam.Color;

                if (_childText)
                {
                    _childText.color = _param.DisableParam.Color;
                }
            }
        }

        // 設定が配置されてないときのデフォルト値設定
        private void SetDefault()
        {
            _image.color = Color.white;
            _image.sprite = null;
            _flip.SetFlipXY(false);
            
            if (_childText)
            {
                _childText.color = Color.white;
            }
        }

        // ボタンの通常表示
        private void SetNormalColor()
        {
            _image.sprite = _param.NormalSprite;
            _image.color = _param.NormalColor;
            _flip.SetFlipXY(false);

            if (!_childText) return;
            _childText.color = _param.NormalColor;
        }

        // ボタンが押されたとき
        private void SetPressColor()
        {
            _image.sprite = _param.NormalSprite;
            _image.color = _param.PressColor;
            _flip.SetFlipXY(true);

            if (!_childText) return;
            _childText.color = _param.NormalColor;
        }

        // 子要素のテキストの通常表示
        private void SetNormalText()
        {
            if (!_textPressed)
            {
                return;
            }
            _textPressed = false;

            if (_param.TextParam && _childRect)
            {
                Vector2 pos = _param.TextParam.Offset;
                _childRect.AddLocalPosXY(-pos.x, pos.y);
            }
        }

        // 子要素のテキストの押された時表示
        private void SetPressText()
        {
            if (_textPressed)
            {
                return;
            }
            _textPressed = true;

            if (_param.TextParam && _childRect)
            {
                Vector2 pos = _param.TextParam.Offset;
                _childRect.AddLocalPosXY(pos.x, -pos.y);
            }
        }
    }
}
