//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Zインデックスを更新するためのオブジェクト
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public partial class ZIndexUpdaterTile_V2 : MonoBehaviour
    {
        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        // Zの移動量の係数
        private const float FACTOR = 0.01f;
        // レベル
        private static readonly ValueDropdownList<float> _levels = new ValueDropdownList<float>()
        {
            { "Default", 0      },
            { "Layer 1", 0.001f },
            { "Layer 2", 0.002f },
            { "Layer 3", 0.003f },
            { "Layer 4", 0.004f },
        };
        // レイヤー内のオフセット
        private const float MIN_RAND = -0.25f;
        private const float MAX_RAND = 0.25f;
        // 更新中の線の色
        private readonly Color _EnabledColoe = new Color(1, 0.4313726f, 0);

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 子要素の画像オブジェクト
        [SerializeField, BoxGroup("基本設定")]
        private SpriteRenderer _childSprite;
        // セルのサイズ
        [SerializeField, BoxGroup("基本設定")]
        private Vector2 _cellSize = new Vector2(1, 0.5f);
        // このオブジェクトの更新タイプ
        // true : 常に更新する / false : 一回だけ
        [SerializeField, BoxGroup("基本設定")]
        private bool _isAlwaysUpdate = true;
        // 最後に加算するオフセット(これ変える事なさそう
        [SerializeField, BoxGroup("基本設定")]
        private float _globalOffset = 1;
        // テンプレート表示モードで表示するかどうか
        // true : テンプレート表示 / false : 通常表示
        [SerializeField, BoxGroup("基本設定"), LabelText("テンプレート表示モード"), OnValueChanged(nameof(OnIsDisplayTemplateModeChanged))]
        private bool _isDisplayTemplateMode;
        // 子要素を非表示にするかどうか？
        // true : 表示しないする / false : 表示する
        [SerializeField, BoxGroup("基本設定"), LabelText("非表示モード"), OnValueChanged(nameof(OnHideChildObjectChanged))]
        private bool _hideChildObject = true;

        // 表示する緑色の線の長さ
        [SerializeField, BoxGroup("デバッグ表示")] private float _gizmoWidth = 0.5f;
        // セルのアウトラインを表示するかどうか
        // true : 表示する / false : 表示しない
        [SerializeField, BoxGroup("デバッグ表示")] bool _showCellGizmo = true;

        // 同じレイヤー内でマルチレイヤー表示するときに加算するオフセット量
        [SerializeField, BoxGroup("オフセット調整"), LabelText("レイヤー内微調整"), ValueDropdown(nameof(_levels))]
        private float _inLayerOffsetY = 0;
        // Y位置のオフセット
        [SerializeField, BoxGroup("オフセット調整"), LabelText("Yオフセット(1)"), OnValueChanged(nameof(OnOffsetYChanged)), Range(-1, 1)]
        private float _offsetY_1 = 0.0f;
        // Y位置のオフセット
        [SerializeField, BoxGroup("オフセット調整"), LabelText("Yオフセット(2)"), OnValueChanged(nameof(OnOffsetYChanged)), Range(-1, 1)]
        private float _offsetY_2 = 0.0f;
        // Y位置のオフセット
        [SerializeField, BoxGroup("オフセット調整"), LabelText("Yオフセット(3)"), OnValueChanged(nameof(OnOffsetYChanged)), Range(-1, 1)]
        private float _offsetY_3 = 0.0f;

        // ランダム配置を受け付けるかどうかのフラグ
        // true : 受け付ける / false : 受け付けない
        [SerializeField, BoxGroup("ランダム化")]
        private bool _acceptRandom;
        // ランダム配置するときの下限値
        [SerializeField, BoxGroup("ランダム化"), ShowIf(nameof(_acceptRandom)), Range(MIN_RAND, 0)]
        private float _randMin;
        [SerializeField, BoxGroup("ランダム化"), ShowIf(nameof(_acceptRandom)), Range(0, MAX_RAND)]
        private float _randMax;

        // このセルが持つ画像のリスト
        [SerializeField] List<Sprite> _spriteList;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // キャッシュ
        private Transform cache_Transform;
        private SpriteRenderer cache_SpriteRenderer;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// テンプレート表示中かどうかを取得します。
        /// </summary>
        public bool IsDisplayTemplate
        {
            get => _isDisplayTemplateMode;
            set
            {
                _isDisplayTemplateMode = value;
                OnIsDisplayTemplateModeChanged();
            }
        }

        /// <summary>
        /// オフセット量を設定または取得します。
        /// </summary>
        public float OffsetY
        {
            get => TotalOffsetY;
        }

        /// <summary>
        /// このオブジェクトの <see cref="SpriteRenderer"/> を取得します。
        /// </summary>
        public SpriteRenderer SpriteRenderer_Cache
        {
            get
            {
                if (cache_SpriteRenderer == null)
                {
                    cache_SpriteRenderer = GetComponent<SpriteRenderer>();
                }
                return cache_SpriteRenderer;
            }
        }

        /// <summary>
        /// このオブジェクトの <see cref="Transform"/> を取得します。
        /// </summary>
        public Transform Transform_Cache
        {
            get
            {
                if (cache_Transform == null)
                {
                    cache_Transform = transform;
                }
                return cache_Transform;
            }
        }

        /// <summary>
        /// オブセット全体の量を取得します。
        /// </summary>
        public float TotalOffsetY => _offsetY_1 + _offsetY_2 + _offsetY_3;

        //
        // Inspector - events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="_offsetY_3"/> がインスペクター上で変更された時に呼び出されます。
        /// </summary>
        private void OnOffsetYChanged()
        {
            _childSprite.transform.SetLocalPosY(TotalOffsetY);
        }

        /// <summary>
        /// <see cref="_isDisplayTemplateMode"/> がインスペクター上で変更された時に呼び出されます。
        /// </summary>
        private void OnIsDisplayTemplateModeChanged()
        {
            if (_isDisplayTemplateMode)
            {
                // テンプレート表示モード
                SpriteRenderer_Cache.SetColorA(1);
                _childSprite.gameObject.SetActive(false);
            }
            else
            {
                // 通常表示モード
                SpriteRenderer_Cache.SetColorA(0);
                _childSprite.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// <see cref="_hideChildObject"/> がインスペクター上で変更された時に呼び出されます。
        /// </summary>
        private void OnHideChildObjectChanged()
        {
            GameObject go = _childSprite.gameObject;
            if (go)
            {
                _childSprite.gameObject.hideFlags = _hideChildObject ? HideFlags.HideInHierarchy : HideFlags.None;
            }
        }

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void OnValidate()
        {
            OnHideChildObjectChanged();

            if (_isAlwaysUpdate)
            {
                enabled = true;
            }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                SpriteRenderer_Cache.enabled = false;
            }
            else
            {
                SpriteRenderer_Cache.enabled = true;
            }
#else
            this.SpriteRenderer_Cache.enabled = false;
#endif

        }

        private void Update()
        {
            UpdateZIndex();

            // 常時更新でない場合は一回更新して終わり
            if (!_isAlwaysUpdate)
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

            Gizmos.color = enabled ? _EnabledColoe : Color.yellow;
            Gizmos.DrawLine(t, r);
            Gizmos.DrawLine(r, b);
            Gizmos.DrawLine(b, l);
            Gizmos.DrawLine(l, t);

            // 前後が入れ替わる位置
            Transform trans = Transform_Cache;
            Vector3 wpos = trans.GetPos();
            float y = wpos.y * trans.lossyScale.y;

            // 中心のバツマークを表示
            GizmoDrawer.DispCrossMark(wpos.x, y, wpos.z, 0.1f, enabled ? _EnabledColoe : Color.yellow);

            if (IsDisplayTemplate)
            {
                // テンプレート表示モードかどうかを表示
                // ランダム化する場合文字を表示
                var style = new GUIStyle();
                style.normal.textColor = Color.green;
                style.fontSize = 10;
                style.alignment = TextAnchor.MiddleCenter;
                Vector3 pos = c;
                pos.y += 0.3f;
                pos.x -= 0.05f;
                Handles.Label(pos, "Tp", style);
            }
            else if (/*!this.IsDisplayTemplate && */_acceptRandom)
            {
                // ランダム化する場合文字を表示
                var style = new GUIStyle();
                style.normal.textColor = enabled ? _EnabledColoe : Color.yellow;
                style.fontSize = 10;
                style.alignment = TextAnchor.MiddleCenter;
                Vector3 pos = c;
                pos.y += 0.3f;
                pos.x -= 0.05f;
                Handles.Label(pos, "Rand", style);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!_showCellGizmo)
            {
                return;
            }

            Transform t = Transform_Cache;
            Vector3 wpos = t.GetPos();
            float y = wpos.y * t.lossyScale.y;

            float width = _gizmoWidth;

            // シーンビュー上に目印を表示
            var s = new Vector3(t.GetPos().x + width, y, wpos.z);
            var e = new Vector3(t.GetPos().x - width, y, wpos.z);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(s, e);
        }

#endif

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このオブジェクトを使用できる状態にセットアップします。
        /// </summary>
        [Button("セットアップ"), BoxGroup("基本設定")]
        public void Setup()
        {
            var list = this.GetChilds().ToList();
            if (list.Count != 0)
            {
                return; // 子要素がある場合は何もしない
            }

            var child = new GameObject("Tile");
            child.SetParent(this);

            SpriteRenderer sr = child.AddComponent<SpriteRenderer>();
            _childSprite = sr;

            if (_spriteList == null || _spriteList.Count == 0)
            {
                _childSprite.sprite = SpriteRenderer_Cache.sprite;
            }
            else
            {
                _childSprite.sprite = _spriteList[0];
            }

            _isDisplayTemplateMode = true;
            OnIsDisplayTemplateModeChanged();

            _hideChildObject = true;
            OnHideChildObjectChanged();
        }

        /// <summary>
        /// オフセット量をリセットします。
        /// </summary>
        [Button("リセット"), BoxGroup("オフセット調整")]
        public void ResetOffset()
        {
            //this.OffsetY = 0;
            _offsetY_1 = 0;
            _offsetY_2 = 0;
            _offsetY_3 = 0;
        }

        /// <summary>
        /// Z位置を更新します。
        /// </summary>
        [Button("Z位置更新")]
        public void UpdateZIndex()
        {
            Transform t = Transform_Cache;
            float z = t.GetLocalPosY() * FACTOR;
            z += _globalOffset - _inLayerOffsetY;
            t.SetLocalPosZ(z);
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

            SpriteRenderer sr = _childSprite;
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
        [Button(Name = "ランダム化 実行"), BoxGroup("ランダム化")]
        public void Random()
        {
            if (!_acceptRandom || IsDisplayTemplate)
            {
                return; // 設定次第で受け付けない, テンプレート表示の時は無視
            }
            _offsetY_1 = UniRandom.Range(_randMin, _randMax);
            _offsetY_2 = 0;
            _offsetY_3 = 0;
            OnOffsetYChanged();
        }

        /// <summary>
        /// クロスマークの位置を取得します。
        /// </summary>
        public Vector3 GetCrossPos()
        {
            Transform t = Transform_Cache;
            Vector3 wpos = t.GetPos();
            float y = wpos.y;
            return new Vector3(wpos.x, y, wpos.z);
        }

        /// <summary>
        /// 実際のゲーム実行時に呼び出します。
        /// </summary>
        [Button("リリースモードに切り替え")]
        public void SetReleaseMode()
        {
            SpriteRenderer sp = SpriteRenderer_Cache;
            sp.enabled = false;
            _childSprite.enabled = true;
            _childSprite.gameObject.SetActive(true);

            _isDisplayTemplateMode = false;
            OnIsDisplayTemplateModeChanged();

            _hideChildObject = true;
            OnHideChildObjectChanged();

            _isAlwaysUpdate = false;
            enabled = false;
        }
    }
}