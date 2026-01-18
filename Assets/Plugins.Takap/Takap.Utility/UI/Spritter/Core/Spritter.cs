//
// (C) 2022 Takap.
//

using Takap.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap.UI
{
    /// <summary>
    /// 上下もしくは左右にドラッグできるサイズ調整用の境界線の基底クラスを表します。
    /// </summary>
    /// <remarks>
    /// 境界線をドラッグすることで左右もしくは上下に設定したパネルサイズを任意の大きさに変更できます。
    /// </remarks>
    [RequireComponent(typeof(Image))]
    public abstract class Spritter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 左右に移動可能を表すアイコン
        // ** 指定するテクスチャーはTextureType = Cursorに設定しないと警告がたくさん出るので注意
        [SerializeField] Texture2D _cursor;
        [SerializeField] protected Camera _cam;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 自分のキャッシュ
        Image _image;
        protected RectTransform _rect;
        // 親のキャンバス
        protected Canvas _canvas;
        CanvasScaler _canvasScaler;

        // ポインターが自分の範囲内かどうか
        // true: 範囲内 / false: それ以外
        bool _isinPointer;
        // ドラッグ中かどうか
        // true: ドラッグ中 / false: それ以外
        bool _pressed;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public Camera Camera { get => _cam; set => _cam = value; }

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _image);
            _rect = transform as RectTransform;

            _canvas = _image.canvas;
            _canvas.SetComponent(ref _canvasScaler);

            if (!_cam) _cam = Camera.main;
        }

        // ドラッグ可能 or ドラッグ中はカーソルを変更する
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _isinPointer = true;
            SetDragCursor();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _isinPointer = false;
            if (!_pressed) SetNormalCursor();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
            SetDragCursor();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            if (!_isinPointer) SetNormalCursor();
        }

        // ドラッグ中の左右パネルサイズを変更
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (_canvas.renderMode == RenderMode.WorldSpace)
            {
                Log.Warn($"Not support {RenderMode.WorldSpace}."); // 面倒見切れないのでサポート対象外
                return;
            }

            // 実際の画面サイズとCanvasの指定サイズが違うとスクロール量が不一致するのを訂正する
            var scaledDelta = eventData.delta;
            float x = _canvasScaler.referenceResolution.x / Screen.width;
            float y = _canvasScaler.referenceResolution.y / Screen.height;
            scaledDelta.x *= x;
            scaledDelta.y *= y;

            this.Core(scaledDelta);
        }

        protected abstract void Core(Vector2 scaledDelta);

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        protected void SetDragCursor()
        {
            if (!_cursor)
            {
                return;
            }
            Cursor.SetCursor(_cursor, new Vector2(_cursor.width / 2, _cursor.height / 2), CursorMode.Auto);
        }

        protected void SetNormalCursor()
        {
            if (!_cursor)
            {
                return;
            }
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
