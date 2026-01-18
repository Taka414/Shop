//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Takap.Utility
{
    //public class SpriteToggleButton : ToggleBase
    //{
    //}

    /// <summary>
    /// 
    /// </summary>
    public class ToggleGroupEx : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] private bool _allowSwitchOff;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private List<ToggleBase> _toggleList = new();

        private List<ToggleBase> _activeTogglesTemp = new();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        //public bool AllowSwitchOff
        //{
        //    get => _allowSwitchOff;
        //    set => _allowSwitchOff = value;
        //}

        /// <summary>
        /// 現在登録されているトグルのリストを取得します。
        /// </summary>
        public IEnumerable<ToggleBase> ToggleList => _toggleList;

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// グループ内のトグルがONになった通知を受け取ります。
        /// </summary>
        internal void NotifyToggleOnFromToggle(ToggleBase toggle)
        {
            if (toggle == null || !_toggleList.Contains(toggle))
            {
                throw new ArgumentException($"Toggle {toggle} is not part of ToggleGroupEx {this}");
            }

            if (!toggle.IsOn)
            {
                return; // OFFなら何もしない
            }

            // ほかのToggleをすべてOFFにする
            for (var i = 0; i < _toggleList.Count; i++)
            {
                var item = _toggleList[i];
                if (item == toggle)
                {
                    continue;
                }
                
                item.IsOn = false;
            }
        }

        public void UnregisterToggle(ToggleBase toggle)
        {
            _toggleList.Remove(toggle);
        }

        public void RegisterToggle(ToggleBase toggle)
        {
            _toggleList.AddUnique(toggle);
        }

        public bool AnyTogglesOn()
        {
            return _toggleList.Find(x => x.IsOn) != null;
        }

        public List<ToggleBase> ActiveToggles()
        {
            _activeTogglesTemp.Clear();

            int cnt = _toggleList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var item = _toggleList[i];
                if (item.IsOn)
                {
                    _activeTogglesTemp.Add(item);
                }
            }

            return _activeTogglesTemp;
        }

        /// <summary>
        /// オブジェクトの中に有効な
        /// </summary>
        public bool ContainsAvtive()
        {
            int cnt = _toggleList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var toggle = _toggleList[i];
                if (toggle.IsOn)
                {
                    return true;
                }
            }
            return false;
        }

        //public void SetAllTogglesOff()
        //{
        //    bool oldAllowSwitchOff = _allowSwitchOff;
        //    _allowSwitchOff = true;
        //
        //    for (var i = 0; i < _toggleList.Count; i++)
        //    {
        //        _toggleList[i].IsOn = false;
        //    }
        //
        //    _allowSwitchOff = oldAllowSwitchOff;
        //}
    }
}
