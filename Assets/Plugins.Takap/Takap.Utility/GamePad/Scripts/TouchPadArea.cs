//
// (C) 2022 Takap.
//

using R3;
using Takap.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap
{
    /// <summary>
    /// タッチパッド用のエリアを表します。
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class TouchPadArea : UIMonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] bool _eventEnabled;

        [SerializeField] TouchPad _touchPad;

        //
        // Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// エリア内でパッド操作が行われた時に発生します。
        /// </summary>
        public Observable<PadAction> PadActionPerFrame => _padActionPerFrame;
        private readonly Subject<PadAction> _padActionPerFrame = new();

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // このエリアを表すイメージ
        Image _image;
        // 押されているかどうかを表すフラグ
        // true: 押されている / false: それ以外
        bool _isPressed;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public bool EventEnabled
        {
            get => _eventEnabled;
            set => _eventEnabled = value;
        }

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            if (_touchPad == null)
            {
                Log.Warn("Not set TouchPad.");
                return;
            }

            this.SetComponent(ref _image);
            _touchPad.PadActionPerFrame.Subscribe(p => _padActionPerFrame.OnNext(p));
        }

        public void OnPointerDown(PointerEventData e)
        {
            if (!_eventEnabled)
            {
                return;
            }
            _isPressed = true;

            if (_touchPad)
            {
                _touchPad.Begin(new TouchAreaInfo(e, _image));
            }
        }

        public void OnPointerUp(PointerEventData e)
        {
            if (!_eventEnabled)
            {
                return;
            }
            _isPressed = false;

            if (_touchPad)
            {
                _touchPad.End(new TouchAreaInfo(e, _image));
            }
        }

        public void OnPointerMove(PointerEventData e)
        {
            if (!_eventEnabled || !_isPressed)
            {
                return;
            }

            if (_touchPad)
            {
                _touchPad.Move(new TouchAreaInfo(e, _image));
            }
        }
    }

    /// <summary>
    /// エリア → パッドに渡すパラメータクラス
    /// </summary>
    public readonly struct TouchAreaInfo
    {
        private readonly PointerEventData _eventData;

        private readonly Image _touchAreaImage;

        public TouchAreaInfo(PointerEventData eventData, Image touchAreaImage)
        {
            _eventData = eventData;
            _touchAreaImage = touchAreaImage;
        }

        public Vector2 ScreenPos => _eventData.position;

        public Vector2 LocalPos => _eventData.ToLocalPos(_touchAreaImage);
    }

    /// <summary>
    /// パッドの操作タイプ
    /// </summary>
    public enum PadActionType
    {
        Start,
        Move,
        End,
    }

    /// <summary>
    /// パッド → エリアに渡す操作内容を表すパラーメータークラス
    /// </summary>
    public readonly struct PadAction
    {
        public readonly float Amount;

        public readonly Radian Rad;

        public Degree Deg => Rad * Mathf.Rad2Deg;

        public readonly PadActionType ActionType;

        public PadAction(float amount, PadActionType type, float rad)
        {
            Amount = amount;
            ActionType = type;
            Rad = rad;
        }
    }
}
