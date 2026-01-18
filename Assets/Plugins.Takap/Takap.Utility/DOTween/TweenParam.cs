//
// (C) 2024 Takap.
//

using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Takap.Utility
{
    [Serializable]
    public class TweenVector3 : TweenParam<Vector3> { }

    [Serializable]
    public class TweenVector2 : TweenParam<Vector2> { }

    [Serializable]
    public class TweenFloat : TweenParam<float> { }

    [Serializable]
    public class TweenColor : TweenParam<Color> { }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>
    /// 任意の型を指定できるTweenパラメータの基底クラスです。
    /// </summary>
    [Serializable]
    public abstract class TweenParam<T>
    {
        const string TAB1 = "Basic";

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


        const string TAB2 = "Option";
        const float WIDTH1 = 110f;

        // ループするかどうか(true: ループする / false: しない)
        [TabGroup(TAB2), LabelWidth(WIDTH1)]
        [SerializeField] bool _isLooping;
        public bool IsLooping { get => _isLooping; set => _isLooping = value; }

        // ループの種類
        [TabGroup(TAB2), EnableIf(nameof(_isLooping)), Indent(1), LabelWidth(WIDTH1)]
        [SerializeField] LoopType _loopType = LoopType.Restart;
        public LoopType LoopType { get => _loopType; set => _loopType = value; }

        // ループ回数(-1は無限)
        [TabGroup(TAB2), EnableIf(nameof(_isLooping)), Indent(1), LabelWidth(WIDTH1)]
        [SerializeField] int _loops = -1;
        public int Loops { get => _loops; set => _loops = value; }

        // タイムスケールを無視するかどうか(true: 無視する / false: しない)
        [TabGroup(TAB2), LabelWidth(WIDTH1)]
        [SerializeField] bool _ignoreTimeScale;
        public bool IgnoreTimeScale { get => _ignoreTimeScale; set => _ignoreTimeScale = value; }


        const string TAB3 = "Curve";

        // イージングにアニメーションカーブを使うかどうか(true: 使う / false: 使わない)
        [TabGroup(TAB3), LabelText("Use Curve")]
        [SerializeField] bool _useAnimationCurve;
        public bool UseAnimationCurve { get => _useAnimationCurve; set => _useAnimationCurve = value; }

        // イージング用のアニメーションカーブ
        [TabGroup(TAB3), EnableIf(nameof(_useAnimationCurve))]
        [SerializeField] AnimationCurve _animationCurve;
        public AnimationCurve AnimationCurve { get => _animationCurve; set => _animationCurve = value; }
    }
}
