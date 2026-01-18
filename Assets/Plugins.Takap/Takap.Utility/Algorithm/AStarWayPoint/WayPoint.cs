//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// ある地点を表します。
    /// </summary>
    [ExecuteAlways]
    public partial class WayPoint : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 移動可能な隣接ノード
        [SerializeField] List<WayPoint> _relations;
        // 通行可能かどうか
        [SerializeField] bool _canMove = true;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 直前のノード隣接ノード状態を記録するリスト
        readonly List<WayPoint> _previousList = new List<WayPoint>();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 移動可能な隣接ノードのリストを取得します。
        /// </summary>
        public List<WayPoint> Rerations => _relations;

        /// <summary>
        /// この地点が移動可能かどうかを取得します。
        /// </summary>
        public bool CanMove => _canMove;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            SynchronizeRelationsToPrevious();
            GizmoText = name;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (_relations is null || _previousList is null)
            {
                return;
            }

            // 接続先の情報を更新する
            if (_relations.Count > _previousList.Count)
            {
                //Log.Trace("増えた");

                // 相手に自分を追加する(復路登録)
                foreach (var p in _relations.Except(_previousList))
                {
                    //Log.Trace(p.name);
                    if (!p.Rerations.Contains(this))
                    {
                        p.Rerations.Add(this);
                        p.SynchronizeRelationsToPrevious();
                    }
                }
            }
            else if (_relations.Count < _previousList.Count)
            {
                //Log.Trace("減った");
                foreach (var p in _previousList.Except(_relations))
                {
                    //Log.Trace(p.name);
                    p.Rerations.Remove(this); // 相手から自分を削除
                    p.SynchronizeRelationsToPrevious();
                }
            }

            SynchronizeRelationsToPrevious();

            GizmoText = name;
        }
#endif
        // 
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="_previousList"/> の内容を <see cref="_relations"/> と同期します。
        /// </summary>
        public void SynchronizeRelationsToPrevious()
        {
            _previousList.Clear();
            foreach (var p in _relations)
            {
                _previousList.Add(p);
            }
        }
    }
}