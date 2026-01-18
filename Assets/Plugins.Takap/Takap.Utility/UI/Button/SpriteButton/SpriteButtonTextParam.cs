//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Parameter/SpriteButtonTextOffsetParam")]
    public class SpriteButtonTextParam : ScriptableObject
    {
        // 押したときのテキストの移動量
        [SerializeField] Vector2 _offset;

        public Vector2 Offset => _offset;
    }
}
