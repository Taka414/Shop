//
// (C) 2022 Takap.
//

using System.Reflection;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Random.State"/> を拡張します。
    /// </summary>
    public static class RandumStateExtensions
    {
        /// <summary>
        /// 現在の <see cref="Random.State"/> の内容を文字列として取得します。
        /// </summary>
        public static string GetString(this in Random.State self)
        {
            System.Type stateType = self.GetType();
            FieldInfo[] fields = stateType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            string str = "";
            foreach (FieldInfo field in fields)
            {
                int value = (int)field.GetValue(self);
                str += $"{field.Name}={value}, ";
            }
            return str;
        }
    }
}