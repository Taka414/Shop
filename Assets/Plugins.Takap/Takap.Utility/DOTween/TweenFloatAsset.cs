//
// (C) 2024 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Tween Parameter/Float")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Boxed, Expanded = true)]
    public class TweenFloatAsset : TweenParamAsset<float> { }
}
