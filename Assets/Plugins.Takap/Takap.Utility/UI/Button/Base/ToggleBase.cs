//
// (C) 2022 Takap.
//

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Takap.Utility
{
    public abstract class ToggleBase : ButtonBase
    {
        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        [Serializable] public class ToggleEvent : UnityEvent<ToggleBase> { }

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] bool _isOn;

        [SerializeField] string _key;

        [SerializeField] ToggleGroupEx _group;

        [SerializeField] ToggleEvent _onValueChanged;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// トグルが ON 状態かどうかを取得します。
        /// </summary>
        public bool IsOn
        {
            get => _isOn;
            set => Set(value);
        }

        /// <summary>
        /// トグルのキーを取得します。
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// トグルの状態が変更されたときに発生します。
        /// </summary>
        public ToggleEvent OnValueChanged => _onValueChanged;

        //
        // External Methods
        // - - - - - - - - - - - - - - - - - - - -

        // トグル状態が変わったときに呼び出されます
        protected virtual void OnChangeToggleState(bool state) { }

        protected override void Clicked()
        {
            IsOn = !IsOn;
        }

        //
        // Unity Events
        // - - - - - - - - - - - - - - - - - - - -

#if UNITY_EDITOR
        protected override void __OnValidate()
        {
            base.__OnValidate();

            OnChangeToggleState(_isOn);
            
            NotifyToggleOnToGroup(_isOn);
        }
#endif

        private void OnEnable()
        {
            SetupGroup(true);
        }

        private void OnDisable()
        {
            SetupGroup(false);
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        private void Set(bool toggled)
        {
            if (_isOn == toggled)
            {
                return;
            }

            _isOn = toggled;
            OnChangeToggleState(toggled);

            _onValueChanged.Invoke(this);

            NotifyToggleOnToGroup(toggled);
        }

        private void NotifyToggleOnToGroup(bool toggled)
        {
            if (_group && toggled)
            {
                _group.NotifyToggleOnFromToggle(this);
            }
        }

        private void SetupGroup(bool addGroup)
        {
            if (!_group)
            {
                return;
            }

            if (addGroup)
            {
                _group.RegisterToggle(this);
            }
            else
            {
                _group.UnregisterToggle(this);
            }

            if (_isOn && enabled && gameObject.activeInHierarchy)
            {
                _group.NotifyToggleOnFromToggle(this);
            }
        }
    }
}
