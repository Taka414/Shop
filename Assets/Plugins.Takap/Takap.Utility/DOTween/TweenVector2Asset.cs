//
// (C) 2024 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Tween Parameter/Vecotr2")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Boxed, Expanded = true)]
    public class TweenVector2Asset : TweenParamAsset<Vector2> { }
}
