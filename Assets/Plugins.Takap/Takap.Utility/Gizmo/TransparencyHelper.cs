
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Y軸ソートの時に
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class TransparencyHelper : MonoBehaviour
    {
        // 説明:
        // Transparency Sort Mode が 'CustomAxis' で
        // X=0, Y=1, Z=1(★) の時にどの位置で前後関係が入れ替わるかを表示する補助線を表示する

        private void OnDrawGizmos()
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
    }
}