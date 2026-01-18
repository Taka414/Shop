using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Y軸ソートの時に前後関係が入れ替わる位置に補助線を引きます。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TransparencyUpdater : MonoBehaviour
    {
        // 説明:
        // Transparency Sort Mode が 'CustomAxis' で
        // X=0, Y=1, Z=1(★) の時にどの位置で前後関係が入れ替わるかを表示する補助線を表示する

        // 画像のスケールが変更されたことを検出して
        // Zの位置を自動的に設定した位置に追従するように調整する

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 基準倍率
        [SerializeField] private float _baseScaleY = 1.0f;
        // 基準倍率の時のZの位置
        [SerializeField] private float _zPosition;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // キャッシュ
        private Transform myTransform;
        // 直前のサイズ
        private Vector3 previousScale;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Start()
        {
            myTransform = transform;

            UpdateZPosition();
        }

        private void Update()
        {
            Vector3 scale = myTransform.lossyScale;

            if (Mathf.Approximately(scale.x, previousScale.x) &&
                Mathf.Approximately(scale.y, previousScale.y))
            {
                return;
            }

            UpdateZPosition();
        }

        private void OnDrawGizmosSelected()
        {
            Transform t = transform;
            Vector3 wpos = t.GetPos();

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            float harfWidth = sr.bounds.size.x / 2.0f;

            // Y軸ソートはPivotなどに関係なく Sprite の中心で起きる
            float z = sr.bounds.center.y + wpos.z;
            GizmoDrawer.DispCrossMark(new Vector2(sr.bounds.center.x, sr.bounds.center.y + wpos.z), 0.1f, Color.red);

            // デバッグ用の表示
            var s = new Vector3(sr.bounds.center.x - harfWidth - harfWidth * 0.5f, z, wpos.z);
            var e = new Vector3(sr.bounds.center.x + harfWidth + harfWidth * 0.5f, z, wpos.z);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(s, e);
        }

        // 現在のYの位置に応じてZの位置を更新する
        private void UpdateZPosition()
        {
            float z = myTransform.lossyScale.y / _baseScaleY * _zPosition;
            myTransform.SetPosZ(z);
            previousScale = myTransform.lossyScale;
        }
    }
}