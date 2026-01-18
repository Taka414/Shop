//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility.Colors
{
    /// <summary>
    /// <see cref="Image"/> の色設定を行います。
    /// </summary>
    public class ImageColor : ColorSettingBase
    {
        [SerializeField, ReadOnly] Image _target;

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
