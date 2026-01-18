//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Parameter/SpriteButtonParam")]
    public class SpriteButtonParam : ScriptableObject
    {
        const string GRP1 = "Normal";
        // 通常のボタン画像
        [SerializeField, BoxGroup(GRP1)] Sprite _normalSprite;
        // 通常時の色
        [SerializeField, BoxGroup(GRP1)] Color _normalColor = Color.white;
        // 押したときの色
        [SerializeField, BoxGroup(GRP1)] Color _pressColor = Color.white;
        // 子要素のテキストの色
        [SerializeField, BoxGroup(GRP1)] Color _textColor = Color.white;
        // 無効時の設定
        [SerializeField, InlineEditor] SpriteButtonDisableParam _disableParam;
        // テキストの設定
        [SerializeField, InlineEditor] SpriteButtonTextParam _textParam;

        public Sprite NormalSprite => _normalSprite;
        public Color NormalColor => _normalColor;
        public Color PressColor => _pressColor;
        public Color TextColor => _textColor;
        public SpriteButtonDisableParam DisableParam => _disableParam;
        public SpriteButtonTextParam TextParam => _textParam;
    }
}
