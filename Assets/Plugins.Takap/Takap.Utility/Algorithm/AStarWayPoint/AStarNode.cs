//
// (C) 2022 Takap.
//

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// 経路探索用のノード情報を表します。
    /// </summary>
    public class AStarNode
    {
        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 親ノードを設定または取得します。
        /// </summary>
        public AStarNode Parent { get; private set; }

        /// <summary>
        /// 次の移動地点を表します。
        /// </summary>
        public readonly WayPoint WayPoint; // Derived from Monobehaiour

        /// <summary>
        /// 探索済みかどうかを設定または取得します。
        /// </summary>
        public NodeStatus Status { get; set; }

        /// <summary>
        /// 開始地点からの総移動距離を取得します。
        /// </summary>
        public float Total { get; private set; }

        /// <summary>
        /// ゴールまでの推定移動距離を取得します。
        /// </summary>
        public float Estimated { get; private set; }

        /// <summary>
        /// ノードのスコアを取得します。
        /// </summary>
        public float Score => Total + Estimated;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public AStarNode(WayPoint wayPoint)
        {
            WayPoint = wayPoint;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void Open(AStarNode previous, AStarNode goal)
        {
            if (previous is not null)
            {
                Total = previous.Total + UnityEngine.Vector3.Distance(previous.WayPoint.transform.position, WayPoint.transform.position);
            }
            Estimated = UnityEngine.Vector3.Distance(WayPoint.transform.position, goal.WayPoint.transform.position);
            Parent = previous;
            Status = NodeStatus.Open;
        }
    }

    /// <summary>
    /// 探索状態を表します。
    /// </summary>
    public enum NodeStatus
    {
        /// <summary>何もしていない</summary>
        None = 0,
        /// <summary>探索済み</summary>
        Open,
        /// <summary>検索除外</summary>
        Close,
        /// <summary>検索対象外</summary>
        Exclude,
    }
}