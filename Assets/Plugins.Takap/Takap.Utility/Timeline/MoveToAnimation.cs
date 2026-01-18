//
// (C) 2022 Takap.
//

using DG.Tweening;
using UnityEngine;

namespace Takap.Utility.Timeline
{
    /// <summary>
    /// TimeLineを使った演出用基底クラス。
    /// </summary>
    public class MoveToAnimation : TimeLinePreviwer
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] private Vector3 _toMovePosision;
        [SerializeField, Range(0.1f, 10.0f)] private float _toMoveDuration = 1.0f;
        [SerializeField] private Ease _easingType = Ease.Linear;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 移動先の座標を設定または取得します。
        /// </summary>
        public Vector3 ToMovePosision { get => _toMovePosision; set => _toMovePosision = value; }

        /// <summary>
        /// 実行時間を設定または取得します。
        /// </summary>
        public float ToMoveDuration { get => _toMoveDuration; set => _toMoveDuration = value; }

        /// <summary>
        /// イージングの種類を設定または取得します。
        /// </summary>
        public Ease EasingType { get => _easingType; set => _easingType = value; }

        //
        // BaseTimeControl impl
        // - - - - - - - - - - - - - - - - - - - -

        // プレビュー対象のアニメーションの作成
        protected override void ImplementSequence(Sequence seq)
        {
            seq.Append(transform.DOLocalMove(ToMovePosision, ToMoveDuration).SetEase(EasingType));
        }
    }
}