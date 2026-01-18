//
// (C) 2022 Takap.
//

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Takap.Utility.Algorithm
{
    // Gizmo描画処理
    public partial class WayPoint
    {
        // Gizmoで表示するテキスト
        public string GizmoText { get; set; }
        // Gizmoの線の色
        public Color GizmoColor { get; set; } = Color.white;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GizmoDrawer.DispCrossMark(this, 0.2f, GizmoColor);
            Handles.Label(transform.localPosition, GizmoText);

            if (_relations is null || _relations.Count == 0)
            {
                return;
            }

            foreach (var next in _relations)
            {
                if (next == null) continue;

                // ポイント間に線を引く
                Color previous = Gizmos.color;
                if (GizmoText.Contains("Step") && next.GizmoText.Contains($"Step"))
                {
                    Gizmos.color = Color.red;
                }
                else if (GizmoText.Contains($"{NodeStatus.Open}") || next.GizmoText.Contains($"{NodeStatus.Open}") &&
                       !(GizmoText.Contains($"{NodeStatus.None}") || next.GizmoText.Contains($"{NodeStatus.None}")))
                {
                    Gizmos.color = Color.yellow;
                }
                else if (GizmoText.Contains($"{NodeStatus.Close}") || next.GizmoText.Contains($"{NodeStatus.Close}"))
                {
                    Gizmos.color = Color.blue;
                }
                else if (GizmoText.Contains($"{NodeStatus.Exclude}") || next.GizmoText.Contains($"{NodeStatus.Exclude}"))
                {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawLine(transform.localPosition, next.transform.localPosition);
                Gizmos.color = previous;
            }
        }
#endif
    }
}