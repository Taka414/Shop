//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Parameter/SpriteToggleParam")]
    public class SpriteToggleParam : ScriptableObject
    {
        const string GRP1 = "Toggle";

        // トグル時の色
        [SerializeField, BoxGroup(GRP1)] Color _normalColor;
        // トグルON時に押したときの色
        [SerializeField, BoxGroup(GRP1)] Color _pressedColor;

        public Color NormalColor =>_normalColor;
        public Color PressedColor => _pressedColor;
    }
}
