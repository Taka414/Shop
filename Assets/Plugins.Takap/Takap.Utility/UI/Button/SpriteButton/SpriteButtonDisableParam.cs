//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Parameter/SpriteButtonDisableParam")]
    public class SpriteButtonDisableParam : ScriptableObject
    {
        const string GRP2 = "Disable";
        // 無効時のボタン画像
        [SerializeField, BoxGroup(GRP2)] Sprite _sprite;
        // 無効時にボタン色を変える場合こっち
        [SerializeField, BoxGroup(GRP2)] Color _color = Color.white;

        public Sprite Sprite => _sprite;
        public Color Color => _color;
    }
}
