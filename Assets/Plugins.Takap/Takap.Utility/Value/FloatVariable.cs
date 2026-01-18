//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Value/Float")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField, LabelText("Description")] string _descriptin;
        [SerializeField, LabelText("値(float)")] float _value;

        /// <summary>
        /// 値を設定または取得します。
        /// </summary>
        public float Value { get => _value; set => _value = value; }

        /// <summary>
        /// float に代入するときに.Value を指定しないように暗黙の変換を行います。
        /// </summary>
        public static implicit operator float(FloatVariable value) => value.Value;
    }
}
