#if UNITY_EDITOR

//
// (C) 2024 Takap.
//

using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// TextMeshProのダイナミックフォントをクリアするためのスクリプト
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Font/Dynamic Font Clear")]
    public class RefreshDynamicFonts : ScriptableObject
    {
        // 
        // Inspectors 
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] TMP_FontAsset[] _fonts;

        // 
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 登録されている全てのダイナミックなデータ情報を削除します。
        /// </summary>
        [Button]
        public void ClearDynamicFonts()
        {
            foreach (var font in _fonts)
            {
                // テクスチャーをクリアする
                font.ClearFontAssetData();

                // 念のため保存する
                EditorUtility.SetDirty(font);
            }
            AssetDatabase.SaveAssets();
        }
    }
}

#endif
