//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Takap.Utility.Colors
{
    /// <summary>
    /// <see cref="TextMeshPro"/> もしくは <see cref="TextMeshProUGUI"/> の色設定を行います。
    /// </summary>
    public class TextColor : ColorSettingBase
    {
        [SerializeField, ReadOnly] TMP_Text _target;

        protected override void OnValidate()
        {
            this.SetComponent(ref _target);
            base.OnValidate();
        }

        protected override void Awake()
        {
            this.SetComponent(ref _target);
            base.Awake();
        }

        protected override void SetColor(in Color c)
        {
            _target.color = c;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_target);
#endif
        }
    }
}
