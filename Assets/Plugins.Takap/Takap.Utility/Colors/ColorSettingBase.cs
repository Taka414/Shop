//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility.Colors
{
    /// <summary>
    /// カラーパレットに応じて色を設定します。
    /// </summary>
    public abstract class ColorSettingBase : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 色の定義
        [SerializeField] ColorPallet _pallet;
        // 設定する色のキー
        [SerializeField] int _id;
        // 設定中の色のサンプル
        [SerializeField, ReadOnly] Color _sampleColor;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        protected virtual void OnValidate()
        {
            if (_pallet is null)
            {
                _sampleColor = ColorPallet.FAILED_COLOR;
            }
            else
            {
                Color c = _pallet.GetColor(_id);
                SetColor(c);
                _sampleColor = c;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        protected virtual void Awake()
        {
            Color c = _pallet.GetColor(_id);
            SetColor(c);
            _sampleColor = c;
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        protected abstract void SetColor(in Color c);
    }
}
