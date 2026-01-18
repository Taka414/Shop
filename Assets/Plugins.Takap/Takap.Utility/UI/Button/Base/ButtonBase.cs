//
// (C) 2022 Takap.
//

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Takap.Utility
{
    /// <summary>
    /// UIのボタンの基底クラスを表します。
    /// </summary>
    [ExecuteAlways]
    public abstract class ButtonBase : UIMonoBehaviour, IStandardButtonEvents
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 操作を受け付けるかどうか
        // true: 受け付ける / false: ない
        [SerializeField] bool _interactable = true;
        // 押された時のイベント通知
        [SerializeField] ButtonEvent _click;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public const bool Normal = false;
        public const bool Pressed = true;

        /// <summary>
        /// ボタンが押せるかどうかを設定または取得します。
        /// <para>true: 押せる(規定値) / false: 無効</para>
        /// </summary>
        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable == value) return;

                _interactable = value;
                OnChangeButtonInteractable(value);
            }
        }

        /// <summary>
        /// クリックされた時に発生するUnityEventを取得します。
        /// </summary>
        public ButtonEvent Click => _click;

        //
        // External Methods
        // - - - - - - - - - - - - - - - - - - - -

        protected virtual void PointerDown(PointerEventData e) { }
        protected virtual void PointerUp(PointerEventData e) { }
        protected virtual void PointerExit(PointerEventData e) { }
        protected virtual void PointerEnter(PointerEventData e) { }
        protected virtual void OnChangeButtonInteractable(bool interactable) { }
        protected virtual void Clicked() => _click?.Invoke(this);

        //
        // Unity Events
        // - - - - - - - - - - - - - - - - - - - -

        protected abstract void Awake();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            UnityEditor.EditorApplication.update += __OnValidate; // こうやって書くのがお約束
        }

        protected virtual void __OnValidate()
        {
            UnityEditor.EditorApplication.update -= __OnValidate;
            if (this == null)
            {
                return;
            }

            Awake();
            OnChangeButtonInteractable(_interactable);
        }
#endif

        //
        // IStandardButtonEvents impl
        // - - - - - - - - - - - - - - - - - - - -

        bool _isPressed;
        bool _isEnter;

        private void OnEnable()
        {
            _isPressed = false;
            _isEnter = false;
        }

        public void OnPointerDown(PointerEventData e)
        {
            if (!_interactable) return;

            _isPressed = true;
            
            PointerDown(e);
        }

        public void OnPointerUp(PointerEventData e)
        {
            if (!_interactable) return;

            _isPressed = false;
            
            PointerUp(e);

            if (_isEnter)
            {
                Clicked();
            }
        }

        public void OnPointerEnter(PointerEventData e)
        {
            if (!_interactable) return;

            _isEnter = true;

            if (_isPressed)
            {
                PointerEnter(e);
            }
        }

        public void OnPointerExit(PointerEventData e)
        {
            if (!_interactable) return;

            _isEnter = false;

            if (_isPressed)
            {
                PointerExit(e);
            }
        }
    }

    [Serializable]
    public class ButtonEvent : UnityEvent<ButtonBase> { }

    /// <summary>
    /// ボタン用のイベント通知を行う事を表します。
    /// </summary>
    public interface IStandardButtonEvents : IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler { }
}