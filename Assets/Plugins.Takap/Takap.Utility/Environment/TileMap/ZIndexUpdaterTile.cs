//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Zインデックスを更新するためのオブジェクト
    /// </summary>
    [ExecuteAlways]
    //[RequireComponent(typeof(SpriteRenderer))]
    public partial class ZIndexUpdaterTile : MonoBehaviour
    {
        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        // レベル
        private static readonly ValueDropdownList<float> levels = new ValueDropdownList<float>(){
            { "None"   , 0      },
            { "Layer 1", 0.001f },
            { "Layer 2", 0.002f },
            { "Layer 3", 0.003f },
            { "Layer 4", 0.004f },
         };

        //private enum Offset
        //{
        //    None = 0,
        //    Layer_1,
        //    Layer_2,
        //    Layer_3,
        //    Layer_4,
        //}

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // セルのサイズ
        [SerializeField, BoxGroup("GridSeting")] Vector2 _cellSize = new Vector2(1, 0.5f);

        // このオブジェクトの更新タイプ
        [SerializeField] private ZIndexMode _mode = ZIndexMode.Once;
        //// 前後が入れ替わる位置のオフセット
        //[SerializeField] private float yOffset = 0.0f;
        // 最後に加算するオフセット(これ変える事なさそう
        [SerializeField] private float _globalOffset = 1;
        // 表示する緑色の線の長さ
        [SerializeField, BoxGroup("Display")] private float _gizmoWidth = 1.0f;

        // ピボットを動かさないで画像を動かすかどうかのフラグ
        // true : 画像を動かす / false : ピボッドを動かす
        [SerializeField, BoxGroup("Offset Settings"), OnValueChanged(nameof(onPinedChanged))]
        private bool _pined = true;
        // ピンされたときの位置
        [SerializeField, BoxGroup("Offset Settings"), ShowIf(nameof(_pined)), ReadOnly]
        private Vector2 _pinedPos = Vector2.zero;
        // 同じレイヤー内でマルチレイヤー表示するときに加算するオフセット量
        [SerializeField, BoxGroup("Offset Settings"), ShowIf(nameof(_pined)), ValueDropdown(nameof(levels)), LabelText("レイヤー内微調整")]
        private float _offsetLevel = 0;
        // 位置のオフセット
        [SerializeField, BoxGroup("Offset Settings"), ShowIf(nameof(_pined)), OnValueChanged(nameof(onPinedYOffsetChanged)), Range(-2, 2)]
        private float _pinedYOffset = 0.0f;

        // ランダム配置を受け付けるかどうかのフラグ
        // true : 受け付ける / false : 受け付けない
        [SerializeField, BoxGroup("Random")]
        private bool _acceptRandom = false;
        // ランダム配置するときの下限値
        [SerializeField, BoxGroup("Random"), ShowIf(nameof(_acceptRandom)), Range(-0.6f, 0.6f)]
        private float _randMin;
        [SerializeField, BoxGroup("Random"), ShowIf(nameof(_acceptRandom)), Range(-0.6f, 0.6f)]
        private float _randMax;

        // セルのアウトラインを表示するかどうか
        // true : 表示する / false : 表示しない
        [SerializeField, BoxGroup("Display")] bool _showCellGizmo = true;

        // このセルが持つ画像のリスト
        [SerializeField] List<Sprite> _spriteList;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // Zの移動量の係数
        private const float FACTOR = 0.01f;
        // キャッシュ
        private Transform _myTransform;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// セルの位置を表す Gizmo を表示するかどうかを設定または取得します。
        /// true : 表示する / false : 表示しない
        /// </summary>
        public bool ShowCellGizmo { get => _showCellGizmo; set => _showCellGizmo = value; }

        /// <summary>
        /// このセルを現在の位置でピンするかどうかを設定または取得します。
        /// true : ピンする / false : ピン解除
        /// </summary>
        public bool Pined
        {
            get => _pined;
            set
            {
                if (_pined == value)
                {
                    return;
                }
                _pined = value;
                onPinedChanged();
            }
        }

        /// <summary>
        /// ピンされたオブセットの高さを設定または取得します。
        /// </summary>
        public float OffsetY
        {
            get => _pinedYOffset;
            set
            {
                if (!_pined)
                {
                    return;
                }
                _pinedYOffset = value;
                onPinedYOffsetChanged();
            }
        }

        /// <summary>
        /// ピンされた位置を取得します。
        /// </summary>
        public Vector2 PinedPos => _pinedPos;

        //
        // Inspector - events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="_pined"/> がインスペクター上で変更された時に呼び出されます。
        /// </summary>
        private void onPinedChanged()
        {
            if (_pined)
            {
                _pinedPos = transform.GetLocalPos();
            }
            else
            {
                transform.AddLocalPosY(_pinedYOffset);
                _pinedPos = Vector2.zero;
                _pinedYOffset = 0;
            }
        }

        /// <summary>
        /// <see cref="_pinedYOffset"/> がインスペクター上で変更された時に呼び出されます。
        /// </summary>
        private void onPinedYOffsetChanged()
        {
            if (!_pined)
            {
                return;
            }

            transform.SetLocalPosY(_pinedPos.y - _pinedYOffset);
        }

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void OnValidate()
        {
            if (_mode == ZIndexMode.Always)
            {
                enabled = true;
            }
        }

        private void Awake()
        {
            _myTransform = transform;
        }

        private void Update()
        {
            UpdateZIndex();

            // 'Once' の場合1回値を設定したら更新を終了する
            //   → 'Always' の場合、以降毎フレーム更新する
            if (_mode == ZIndexMode.Once)
            {
                UpdateZIndex();
                enabled = false;
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!_showCellGizmo)
            {
                return;
            }

            Vector3 c = GetCrossPos();

            float xf = _cellSize.x;
            float yf = _cellSize.y;

            var t = new Vector3(c.x, c.y + yf, c.z);
            var r = new Vector3(c.x + xf, c.y, c.z);
            var b = new Vector3(c.x, c.y - yf, c.z);
            var l = new Vector3(c.x - xf, c.y, c.z);

            Gizmos.color = enabled ? EnabledColoe : Color.yellow;
            Gizmos.DrawLine(t, r);
            Gizmos.DrawLine(r, b);
            Gizmos.DrawLine(b, l);
            Gizmos.DrawLine(l, t);

            // 前後が入れ替わる位置
            Transform trans = transform;
            Vector3 wpos = trans.GetPos();
            float y = wpos.y + _pinedYOffset * trans.lossyScale.y;

            // 中心のバツマークを表示
            GizmoDrawer.DispCrossMark(wpos.x, y, wpos.z, 0.1f, enabled ? EnabledColoe : Color.yellow);

            // ピンされている場合文字を表示
            if (_pined)
            {
                var style = new GUIStyle();
                style.normal.textColor = enabled ? EnabledColoe : Color.yellow;
                style.fontSize = 10;
                style.alignment = TextAnchor.MiddleCenter;
                Vector3 pos = c;
                pos.y -= 0.1f;
                pos.x -= 0.05f;
                Handles.Label(pos, "Pin", style);
            }

            // ランダム化する場合文字を表示
            if (_acceptRandom)
            {
                var style = new GUIStyle();
                style.normal.textColor = enabled ? EnabledColoe : Color.yellow;
                style.fontSize = 10;
                style.alignment = TextAnchor.MiddleCenter;
                Vector3 pos = c;
                pos.y += 0.3f;
                pos.x -= 0.05f;
                Handles.Label(pos, "Rand", style);
            }
        }

        // 更新中の線の色
        private readonly Color EnabledColoe = new Color(1, 0.4313726f, 0);

        private void OnDrawGizmosSelected()
        {
            if (!_showCellGizmo)
            {
                return;
            }

            Transform t = transform;
            Vector3 wpos = t.GetPos();
            float y = wpos.y + _pinedYOffset * t.lossyScale.y;

            float width = _gizmoWidth;

            // シーンビュー上に目印を表示
            var s = new Vector3(t.GetPos().x + width, y, wpos.z);
            var e = new Vector3(t.GetPos().x - width, y, wpos.z);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(s, e);

            //if (this.acceptRandom)
            //{
            //    GizmoDrawer.DispCrossMark(new Vector3(wpos.x, y, wpos.z), 0.1f, Color.yellow);
            //}
            //else
            //{
            //    GizmoDrawer.DispCrossMark(new Vector3(wpos.x, y, wpos.z), 0.1f, Color.red);
            //}
        }

#endif

        /// <summary>
        /// クロスマークの位置を取得します。
        /// </summary>
        public Vector3 GetCrossPos()
        {
            Transform t = transform;
            Vector3 wpos = t.GetPos();
            float y = wpos.y + _pinedYOffset * t.lossyScale.y;
            return new Vector3(wpos.x, y, wpos.z);
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        [Button]
        public void UpdateZIndex()
        {
            if (_myTransform == null)
            {
                _myTransform = transform;
            }
            Transform t = _myTransform;

            float z = (t.GetLocalPosY() + _pinedYOffset * t.lossyScale.y) * FACTOR;
            //z += this.globalOffset;
            z += _globalOffset - _offsetLevel;
            //if (this.offsetLevel != Offset.None)
            //{
            //    z -= (int)this.offsetLevel * factorOffset;
            //}
            t.SetLocalPosZ(z);
        }

        /// <summary>
        /// 描画のモードを変更します。
        /// </summary>
        [Button]
        public void ChangeMode()
        {
            switch (_mode)
            {
                case ZIndexMode.Once:
                {
                    _mode = ZIndexMode.Always;
                    enabled = true;
                    return;
                }
                case ZIndexMode.Always:
                {
                    _mode = ZIndexMode.Once;
                    return;
                }
                default: break;
            }
        }

        /// <summary>
        /// 描画のモードを設定します。
        /// </summary>
        [Button]
        public void SetMode(ZIndexMode mode)
        {
            if (_mode == mode)
            {
                return;
            }
            _mode = mode;
            if (_mode == ZIndexMode.Always)
            {
                enabled = true;
            }
        }

        /// <summary>
        /// 表示する画像を変更します。
        /// </summary>
        [Button(Name = "画像の変更")]
        public void ChangeSprite()
        {
            if (_spriteList.Count == 0)
            {
                Debug.Log("画像のリストが空です。");
                return;
            }

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                Debug.Log("SpriteRenderer をアタッチしていません。");
                return;
            }
            int index = _spriteList.FindIndex(s => s.name == sr.sprite.name);
            if (index == -1)
            {
                sr.sprite = _spriteList[0];
            }
            else
            {
                if (index == _spriteList.Count - 1)
                {
                    index = -1;
                }
                sr.sprite = _spriteList[index + 1];
            }
        }

        /// <summary>
        /// 高さをランダムに変更します。
        /// </summary>
        [Button(Name = "ランダム化"), BoxGroup("Random")]
        public void Random()
        {
            if (!_acceptRandom)
            {
                return; // 設定次第で受け付けない
            }
            if (!_pined)
            {
                return; // ピンされていないものは無視
            }
            OffsetY = UniRandom.Range(_randMin, _randMax);
        }

        [Button]
        public void FitGridXY()
        {
            Vector3 pos = transform.GetLocalPos();
            transform.SetLocalPosXY(
                Mathf.Round(pos.x * 10.0f) / 10.0f,
                Mathf.Round(pos.y * 10.0f) / 10.0f);
        }
    }
}