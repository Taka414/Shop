//
// (C) 2024 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Tween Parameter/Curve")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Boxed, Expanded = true)]
    public class AnimationCurveAsset : ScriptableObject
    {
        [SerializeField] AnimationCurve _animationCurve;
        public AnimationCurve AnimationCurve { get => _animationCurve; set => _animationCurve = value; }
        
        public static implicit operator AnimationCurve(AnimationCurveAsset value) => value._animationCurve;
    }
}
