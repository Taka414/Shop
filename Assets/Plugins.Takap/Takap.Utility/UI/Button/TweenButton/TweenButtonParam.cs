//
// (C) 2022 Takap.
//

using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Parameter/TweenButtonParam")]
    public class TweenButtonParam : ScriptableObject
    {
        const string grp = "Animation Params";
        // 押したときのアニメーションの速度
        [SerializeField, BoxGroup(grp)] float _pressDuration = 0.5f;
        // 押したときの大き
        [SerializeField, BoxGroup(grp)] float _pressScale = 0.8f;
        // 押したときのアニメーションのイージング
        [SerializeField, BoxGroup(grp)] Ease _pressEase;
        // 離したときのアニメーションの速度
        [SerializeField, BoxGroup(grp)] float _releaseDuration = 0.5f;
        // 離したときのアニメーションのイージング
        [SerializeField, BoxGroup(grp)] Ease _releaseEase;

        public float PressDuration => _pressDuration;
        public float PressScale => _pressScale;
        public Ease PressEase => _pressEase;
        public float ReleaseDuration => _releaseDuration;
        public Ease ReleaseEase => _releaseEase;
    }
}
