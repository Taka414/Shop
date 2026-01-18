//
// (c) 2020 Takap.
//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Zインデックスを更新するためのオブジェクト
    /// </summary>
    [ExecuteAlways]
    public class ZIndexUpdater : MonoBehaviour
    {
        //
        // 説明:
        // オブジェクトの位置に応じてZの位置を更新するスクリプト
        //  → GameObjectにアタッチすれば有効になる
        //
        #region...
        //
        // 補足:
        // 前後関係の描画の優先順位は SortingLayer > Order In Layer > Z値
        // 
        // Order In Layer で前後関係を処理したくない場合にこのコンポーネントを使用して
        // Z値で前後関係を表現する。3Dビューで見たときに視覚的に分かりやすいかもしれない。
        //
        // Order In Layer と Z値 でパフォーマンス良い悪いは存在しない(らしい)
        // 両方使うと処理が大変なのでできればどちらか一方にしておいた方が無難。
        // 
        // Custom Axis の Y=1 とは同居できるが Z=1 すると描画がおかしくなるのでどちらを使用するかは
        // プロジェクトごとに最初に決めておくべき。
        // 
        #endregion

        //
        // Inner types
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Z位置の更新方法を表します。
        /// </summary>
        public enum UpdateMode
        {
            /// <summary>1回更新したら終了します。</summary>
            Once = 0,
            /// <summary>常に更新し続けます。</summary>
            Always,
            /// <summary>編集中のみ常に更新を行い実行時は更新しません。</summary>
            AlwaysOnlyEditing,
        }

        /// <summary>
        /// Z高さのグループを表します。
        /// </summary>
        public enum ZGroup
        {
            /// <summary>デフォルト(一番手前)</summary>
            Default = 0,
            /// <summary>1番目に表示されるグループ</summary>
            Group1,
            /// <summary>2番目のレイヤー</summary>
            Group2,
            /// <summary>3番目のレイヤー</summary>
            Group3,
            /// <summary>4番目のレイヤー</summary>
            Group4,
        }

        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// レイヤーごとのオフセットを定義します。
        /// </summary>
        private Dictionary<ZGroup, float> layerTable;

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // このオブジェクトの更新タイプ
        [SerializeField] private UpdateMode mode = UpdateMode.AlwaysOnlyEditing;
        // レンダー内のオフセット
        [SerializeField] private float zOffset = default;
        // 所属するZのグループ
        //  → 本当はこれを使わないで 'Order In Layer' で前後を表現したほうがよさそう
        [SerializeField] private ZGroup zGroup = ZGroup.Default;
        // シーンビュー上にレイヤー名を表示するかどうか、true : 表示する / false : 表示しない
        [SerializeField] private bool showLayerName = false;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // Zの移動量の係数
        private const float factor = 0.01f;
        // キャッシュ
        private Transform myTransform;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            layerTable = CreateTable();
            myTransform = transform;
        }

        private void Start()
        {
#if UNITY_EDITOR
            // 編集中のみ更新の場合は実行時は即座に deactive にして終了
            if (EditorApplication.isPlaying && mode == UpdateMode.AlwaysOnlyEditing)
#else
            // 実行中は判定しない
            if (this.mode == UpdateMode.AlwaysOnlyEditing)
#endif
            {
                enabled = false;
                return;
            }

            // 'Once' の場合1回値を設定したら更新を終了する
            //   → 'Always' の場合、以降毎フレーム更新する
            if (mode == UpdateMode.Once)
            {
                UpdateZIndex();
                enabled = false;
            }
        }

        private void Update()
        {
            UpdateZIndex();
        }

        private void OnValidate()
        {
            if (mode == UpdateMode.Once)
            {
                return;
            }
            enabled = true;
        }

        protected void OnDrawGizmosSelected()
        {
            Transform t = transform;
            Vector3 wpos = t.GetPos();
            float y = wpos.y + zOffset * t.lossyScale.y;

            const float width = 1.0f;

            // シーンビュー上に目印を表示
            var s = new Vector3(t.GetPosX() + width + width * 0.5f, y, wpos.z);
            var e = new Vector3(t.GetPosX() - width - width * 0.5f, y, wpos.z);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(s, e);
            GizmoDrawer.DispCrossMark(transform.GetPos(), 0.1f, Color.red);

#if UNITY_EDITOR
            // 不要であれば削除する、そんなに有益なものでもない
            if (showLayerName)
            {
                var style = new GUIStyle();
                style.normal.textColor = Color.green;
                style.fontSize = 9;
                Vector3 l = t.position;
                l.y = s.y;
                Handles.Label(l, zGroup.ToString(), style);
            }
#endif
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void UpdateZIndex()
        {
            // シーンビュー上の処理なのでおかしくならないように少し書き方が変になってる
            Transform t = myTransform == null ? transform : myTransform;
            if (t == null)
            {
                return;
            }
            Dictionary<ZGroup, float> table = layerTable;
            if (table == null)
            {
                table = CreateTable();
            }

            float z = (t.GetPosY() + zOffset * t.lossyScale.y) * factor;
            z += table[zGroup];
            t.SetLocalPosZ(z);
        }

        /// <summary>
        /// スクリプト上から指定したモードに動作を変更します。
        /// </summary>
        public void ChangeMode(UpdateMode mode)
        {
            if (mode == UpdateMode.AlwaysOnlyEditing)
            {
                Debug.LogWarning($"This mode is unsuported in playing. {mode}");
                return; // 実行中の呼び出しを想定しているのでこれは受け付けない
            }

            enabled = true; // 有効にしてループで処理する
            Start();
        }

        /// <summary>
        /// Z値のグループごとのオフセットを表すテーブルを取得します。
        /// </summary>
        private Dictionary<ZGroup, float> CreateTable()
        {
            return new Dictionary<ZGroup, float>()
            {
                // プロジェクトごとに状況が異なるため
                // 前後関係がおかしくなるようであれば個々の値を大きくする
                { ZGroup.Default, 1f },
                { ZGroup.Group1,  2f },
                { ZGroup.Group2,  3f },
                { ZGroup.Group3,  4f },
                { ZGroup.Group4,  5f },
            };
        }
    }
}