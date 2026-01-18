//
// (C) 2023 Takap.
//

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL || UNITY_WEBPLAYER
#undef IS_MOBILE
#else
#define IS_MOBILE // モバイル(=タッチ用の環境)の時だけ宣言する
#endif

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// モバイル・PC両対応の画面のタッチを検出・通知するクラス
    /// </summary>
    public class TouchManager : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // Moveイベントを発生させるかどうかのフラグ
        // true: Moveイベントが発生する / false: 発生しない
        [SerializeField] bool _useMoveEvent = true;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // タッチ開始位置
        Vector2 _startPos;
        // 直前のタッチ位置
        Vector2 _previousPoint;
        

        // 無効な位置
        static readonly Vector2 INVALID = new Vector2(-9999, -9999);

        //
        // Props & Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 画面が推された時に発生します。
        /// </summary>
        public R3.Observable<TouchInfo> PointerDown => _pointerDown;
        private readonly R3.Subject<TouchInfo> _pointerDown = new();

        /// <summary>
        /// 画面が離された時に発生します。
        /// </summary>
        public R3.Observable<TouchInfo> PointerUp => _pointerUp;
        private readonly R3.Subject<TouchInfo> _pointerUp = new();

        /// <summary>
        /// 画面をドラッグ or スワイプ中に発生します。
        /// </summary>
        /// <remarks>
        /// 押されている最中のみ発生する。
        /// IPointerMoveHandler.OnPointerMove とは動きが違う。
        ///  → ポインターが画面上をうろうろしてもイベントは発生しない
        /// </remarks>
        public R3.Observable<TouchInfo> PointerMove => _pointerMove;
        private readonly R3.Subject<TouchInfo> _pointerMove = new();

        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Update()
        {
            var state = GetPointerState();
            switch (state)
            {
                case TouchState.Down:
                {
                    Vector2 curret = GetPosition();
                    _startPos = curret;
                    _previousPoint = curret;
                    _pointerDown.OnNext(new TouchInfo(curret, Vector2.zero, curret));
                    break;
                }
                case TouchState.Move:
                {
                    if (!_useMoveEvent) return;

                    Vector2 curret = GetPosition();
                    if (curret == _previousPoint) return; // 動いた時だけ発生する
                    Vector2 delta = curret- _previousPoint;
                    _previousPoint = curret;
                    _pointerMove.OnNext(new TouchInfo(curret, delta, _startPos));
                    break;
                }
                case TouchState.Up:
                {
                    Vector2 curret = GetPosition();
                    Vector2 delta = curret - _previousPoint;
                    _previousPoint = INVALID;
                    _pointerUp.OnNext(new TouchInfo(curret, delta, _startPos));
                    break;
                }
            }
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在の操作状態を取得します。
        /// </summary>
        private TouchState GetPointerState()
        {
#if IS_MOBILE
            if (Input.touchCount == 0)
            {
                return TouchState.None;
            }

            return Input.GetTouch(0).phase switch
            {
                TouchPhase.Began => TouchState.Down,
                TouchPhase.Moved or TouchPhase.Stationary => TouchState.Move,
                TouchPhase.Canceled or TouchPhase.Ended => TouchState.Up,
                _ => TouchState.None,
            };
#else
            if (Input.GetMouseButtonDown(0))
            {
                return TouchState.Down;
            }
            else if (Input.GetMouseButton(0))
            {
                return TouchState.Move;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return TouchState.Up;
            }
            return TouchState.None;
#endif
        }

        /// <summary>
        /// 現在の操作位置を取得します。
        /// </summary>
        private Vector2 GetPosition()
        {
#if IS_MOBILE
            return Input.GetTouch(0).position;
#else
            return GetPointerState() == TouchState.None ? Vector2.zero : (Vector2)Input.mousePosition;
#endif
        }

        //
        // InnerTypes
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// タッチ状態を表します。
        /// </summary>
        public enum TouchState
        {
            /// <summary>タッチなし</summary>
            None,
            /// <summary>タッチ開始</summary>
            Down,
            /// <summary>タッチ中</summary>
            Move,
            /// <summary>タッチ終了</summary>
            Up,
        }
    }

    /// <summary>
    /// タッチ情報
    /// </summary>
    public readonly struct TouchInfo
    {
        /// <summary>
        /// タッチされたスクリーン座標を取得します。
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        /// 前回のイベントからの移動量を取得します。
        /// </summary>
        public readonly Vector2 Delta;

        /// <summary>
        /// 開始位置のスクリーン座標を取得します。
        /// </summary>
        public readonly Vector2 PressPosition;

        public TouchInfo(Vector2 screenPoint, Vector2 delta, Vector2 pressPosition)
        {
            Position = screenPoint;
            Delta = delta;
            PressPosition = pressPosition;
        }
    }

    public static class TouchInfoExtensions
    {
        public static Vector2 GetPositionViewPort(this TouchInfo self)
        {
            return new Vector2(self.Position.x / Screen.width, self.Position.y / Screen.height);
        }
        public static Vector2 GetDeltaViewPort(this TouchInfo self)
        {
            return new Vector2(self.Delta.x / Screen.width, self.Delta.y / Screen.height);
        }
        public static Vector2 GetPressPositionViewPort(this TouchInfo self)
        {
            return new Vector2(self.PressPosition.x / Screen.width, self.PressPosition.y / Screen.height);
        }
    }
}
