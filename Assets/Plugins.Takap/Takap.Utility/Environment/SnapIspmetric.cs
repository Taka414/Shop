//
// (C) 2020 Takap
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// クォータービュー用のタイルのスナップ機能を提供します。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Renderer))]
    public class SnapIspmetric : MonoBehaviour
    {
        //
        // 使い方:
        // ゲームオブジェクトにアタッチしてグリッドサイズを指定する
        //

        //
        // 参考:
        // https://qiita.com/keroxp/items/97d375786617c9eca783
        //

        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // 仮想グリッドの原点
        [SerializeField] private Vector3 _center;
        // 仮想グリッドのセルの大きさ (単位: 1Unity）
        [SerializeField] private Vector2 _cellSize = new Vector3(1, 1);
        // このオブジェクトの仮想グリッド上での格子の横幅
        [SerializeField] private int _width = 1;
        // このオブジェクトの仮想グリッド上での格子の縦幅
        [SerializeField] private int _height = 1;
        // 格子のオフセット
        [SerializeField] private Vector2 _offset;
        // 仮想グリッドの中心がセルになるか
        // true : centerが中心セルの中心になる / false : 中心セルが存在しない
        [SerializeField] private bool _isOdd = true;
        // オブジェクトがスナップする横の基準点
        [SerializeField] private HorizontalPivot _horizontalPivot = HorizontalPivot.Middle;
        // オブジェクトがスナップする縦の基準点
        [SerializeField] private VerticalPivot _verticalPivot = VerticalPivot.Midle;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private Renderer _myRenderer;
        // Gizmoのアイコン表示するかどうか
        // true : 表示する / false : 表示しない
        private readonly bool _displayGizmo = false;
        // Z位置更新用のオブジェクト
        private ZIndexUpdaterTile _zindex;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        private enum HorizontalPivot
        {
            Left = -1,
            Middle = 0,
            Right = 1
        }

        private enum VerticalPivot
        {
            Top = 1,
            Midle = 0,
            Bottom = -1
        }

        private Vector3 pivotCenter
        {
            get
            {
                var v = new Vector3(((int)_horizontalPivot + 1) / 2f, ((int)_verticalPivot + 1) / 2f, 0);
                return new Vector3(minX, minY, 0) + Vector3.Scale(new Vector3(_width, _height, 0), v);
            }
        }

        private Bounds bounds => _myRenderer.bounds;

        private float minX
        {
            get
            {
                switch (_horizontalPivot)
                {
                    case HorizontalPivot.Left:
                        return bounds.min.x + _offset.x;
                    case HorizontalPivot.Middle:
                        return bounds.center.x - (_cellSize.x * _width / 2) + _offset.x;
                    case HorizontalPivot.Right:
                        return bounds.max.x - (_cellSize.x * _width) + _offset.x;
                }
                return 0;
            }
        }

        private float minY
        {
            get
            {
                switch (_verticalPivot)
                {
                    case VerticalPivot.Top:
                        return bounds.max.y - (_cellSize.y * _height) + _offset.y;
                    case VerticalPivot.Midle:
                        return bounds.center.y - (_cellSize.y * _height / 2) + _offset.y;
                    case VerticalPivot.Bottom:
                        return bounds.min.y + _offset.y;
                }
                return 0;
            }
        }

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void OnEnable()
        {
            _myRenderer = GetComponent<Renderer>();
        }

#if UNITY_EDITOR 

        private void Update()
        {
            if (!Application.isPlaying)
            {
                DoSnap();
            }
        }

#endif

        private void OnDrawGizmos()
        {
            // Gizmoのアイコンを設定するなら自分で設定する
            if (_displayGizmo) Gizmos.DrawIcon(pivotCenter, "MyGizmoCircleOrange");

            Gizmos.color = Color.blue;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    float l = minX + (i * _cellSize.x);
                    float t = minY + (j * _cellSize.y);
                    float r = l + _cellSize.x;
                    float b = t + _cellSize.y;
                    DrawQuad(l, t, r, b);
                }
            }
        }

        //
        // Non-Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        private void DoSnap()
        {
            float x = minX + (_cellSize.x * _width);
            float y = minY + (_cellSize.y * _height);

            float v = Mathf.Floor(_center.x + (_cellSize.x * (int)(x / _cellSize.x)));
            float x0 = v;
            if (_isOdd)
            {
                x0 -= _cellSize.x / 2;
            }

            float y0 = Mathf.Floor(_center.y + (_cellSize.y * (int)(y / _cellSize.y)));
            if (_isOdd)
            {
                y0 -= _cellSize.y / 2;
            }

            float dx = x - x0 > 0.5f ? x0 + _cellSize.x - x : x0 - x;
            float dy = y - y0 > 0.5f ? y0 + _cellSize.y - y : y0 - y;
            transform.position += new Vector3(dx, dy, 0);
            //Debug.Log(this.transform.position);
        }

        private static void DrawQuad(float l, float t, float r, float b)
        {
            DrawLine2D(l, t, r, t);
            DrawLine2D(r, t, r, b);
            DrawLine2D(r, b, l, b);
            DrawLine2D(l, b, l, t);
        }

        private static void DrawLine2D(float sx, float sy, float ex, float ey)
        {
            Gizmos.DrawLine(new Vector3(sx, sy, 0), new Vector3(ex, ey, 0));
        }
    }
}
