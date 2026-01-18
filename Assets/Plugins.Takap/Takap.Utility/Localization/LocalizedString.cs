//
// (C) 2022 Takap.
//

using System;
using Sirenix.OdinInspector;
using Takap.Utility;
using TMPro;
using UnityEngine;

namespace Takap
{
    /// <summary>
    /// キャプション用のローカライズコンポーネント
    /// </summary>
    public abstract class LocalizedString<T> : MonoBehaviour
    {
        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // 表示する文字のキー
        [SerializeField] T _id;
        // ロードした文字列(表示するだけ)
        [ShowInInspector, ReadOnly] string _message;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 
        ISetText _setter;
        // イベント用のハンドル
        private IDisposable _evHandlr;

        //
        // Runtimes
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            if (TryGetComponent<TMP_Text>(out TMP_Text tmpText))
            {
                _setter = new TextMeshProSetter(tmpText);
            }
            else
            {
                Log.Error("Not supported.", this);
            }

            InitOnLangChanged();

            UpdateString();
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 言語設定に変更があったときのイベント処理を行います。
        /// </summary>
        protected abstract void InitOnLangChanged();

        /// <summary>
        /// 指定した ID から文字列を取得します。
        /// </summary>
        protected abstract string GetString(T id);

        /// <summary>
        /// 文字表示を更新します。
        /// </summary>
        protected void UpdateString()
        {
            if (_setter == null)
            {
                return;
            }

            string str = GetString(_id);
            _setter.SetText(str);
            _message = str;
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -
        #region...

        /// <summary>
        /// 文字を設定できることを表すインターフェース。
        /// </summary>
        private interface ISetText
        {
            /// <summary>
            /// 文字を設定します。
            /// </summary>
            void SetText(string text);
        }

        /// <summary>
        /// <see cref="TextMeshPro"/> 用の文字列設定クラス
        /// </summary>
        private class TextMeshProSetter : ISetText
        {
            TMP_Text _target;
            public TextMeshProSetter(TMP_Text component)
            {
                _target = component;
            }

            public void SetText(string text)
            {
                _target.text = text;
            }
        }

        #endregion
    }
}
