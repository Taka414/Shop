//
// (C) 2024 Takap.
//

using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Takap.Utility
{
    public abstract class TweenParamAsset<T> : ScriptableObject
    {
        const string TAB1 = "Basic";
        const string TAB2 = "Option";
        const string TAB3 = "Curve";

        // 相対的な移動かどうか
        [TabGroup(TAB1), LabelText("Relative")]
        [SerializeField] public bool _isRelative = true;
        public bool IsRelative { get => _isRelative; set => _isRelative = value; }

        // 移動量
        [TabGroup(TAB1)]
        [SerializeField] T _endValue;
        public T EndValue { get => _endValue; set => _endValue = value; }

        // 実行時間
        [TabGroup(TAB1)]
        [SerializeField] float _duration = 1;
        public float Duration { get => _duration; set => _duration = value; }

        // イージングの種類
        [TabGroup(TAB1), DisableIf(nameof(_useAnimationCurve))]
        [SerializeField] Ease _ease = Ease.OutQuad;
        public Ease Ease { get => _ease; set => _ease = value; }

        const float WIDTH = 110f;

        // ループするかどうか(true: ループする / false: しない)
        [TabGroup(TAB2), LabelWidth(WIDTH)]
        [SerializeField] bool _isLooping;
        public bool IsLooping { get => _isLooping; set => _isLooping = value; }

        // ループの種類
        [TabGroup(TAB2), EnableIf(nameof(_isLooping)), Indent(1), LabelWidth(WIDTH)]
        [SerializeField] LoopType _loopType = LoopType.Restart;
        public LoopType LoopType { get => _loopType; set => _loopType = value; }

        // ループ回数(-1は無限)
        [TabGroup(TAB2), EnableIf(nameof(_isLooping)), Indent(1), LabelWidth(WIDTH)]
        [SerializeField] int _loops = -1;
        public int Loops { get => _loops; set => _loops = value; }

        // タイムスケールを無視するかどうか(true: 無視する / false: しない)
        [TabGroup(TAB2), LabelWidth(WIDTH)]
        [SerializeField] bool _ignoreTimeScale;
        public bool IgnoreTimeScale { get => _ignoreTimeScale; set => _ignoreTimeScale = value; }

        // イージングにアニメーションカーブを使うかどうか(true: 使う / false: 使わない)
        [TabGroup(TAB3), LabelText("Use Curve")]
        [SerializeField] bool _useAnimationCurve;
        public bool UseAnimationCurve { get => _useAnimationCurve; set => _useAnimationCurve = value; }

        // イージング用のアニメーションカーブ
        [TabGroup(TAB3), ShowIf(nameof(_useAnimationCurve))]
        [SerializeField] AnimationCurveAsset _animationCurve;
        public AnimationCurveAsset AnimationCurve { get => _animationCurve; set => _animationCurve = value; }
    }
}
