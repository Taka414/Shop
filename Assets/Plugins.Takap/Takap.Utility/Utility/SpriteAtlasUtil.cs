//
// (C) 2020 Takap
//

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Takap.Utility;
using UnityEngine;
using UnityEngine.U2D;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="SpriteAtlas"/> クラスを拡張します。
    /// </summary>
    public static class SpriteAtlasUtil
    {
        /// <summary>
        /// 指定した <see cref="SpriteAtlas"/> の中のキーを定数クラス化します。
        /// </summary>
        public static string GenerateClass(this SpriteAtlas atlas)
        {
            // 中身のアクセス用のキーを全部取り出す
            var list = GetKeys(atlas).ToList();
            list.Sort();

            var buff = new StringBuilder();

            string enumName = $"{atlas.name.ToUpper(0)}Type";
            string className = $"{atlas.name.ToUpper(0)}AtlasKey";

            // 列挙型
            buff.AppendLine("/// <summary>");
            buff.AppendLine("/// Monster アトラスにアクセスするための列挙子");
            buff.AppendLine("/// </summary>");
            buff.AppendLine($"public enum {enumName}");
            buff.AppendLine("{");
            buff.AppendLine("    None = 0,");
            foreach (string key in list)
            {
                buff.AppendLine($"    {key.ToUpper(0)},");
            }
            buff.AppendLine("}");
            buff.AppendLine("");

            // 文字列定義
            buff.AppendLine("/// <summary>");
            buff.AppendLine("/// Monster アトラスにアクセスするための文字列キー");
            buff.AppendLine("/// </summary>");
            buff.AppendLine($"public static class {className}");
            buff.AppendLine("{");
            buff.AppendLine("    //");
            buff.AppendLine("    // アトラス内画像へのアクセスキー");
            buff.AppendLine("    //");
            buff.AppendLine("    #region...");
            buff.AppendLine("");
            buff.AppendLine("    None = \"None\";");
            // アクセス用の文字列定義
            foreach (string key in list)
            {
                buff.AppendLine($"    public const string {key.ToUpper(0)} = \"{key}\";");
            }
            buff.AppendLine("");
            buff.AppendLine("    #endregion");
            buff.AppendLine("");
            // enum - 文字列の関連付けテーブル
            buff.AppendLine("    // 文字列と列挙型を結びつけるためのテーブル");
            buff.AppendLine($"    private static readonly Dictionary<{enumName}, string> map = new Dictionary<{enumName}, string>()");
            buff.AppendLine("    {");
            buff.AppendLine("        #region...");
            foreach (string key in list)
            {
                string name = key.ToUpper(0);
                buff.AppendLine($"        {{ {enumName}.{name}, {name} }},");
            }
            buff.AppendLine("        #endregion");
            buff.AppendLine("    };");
            buff.AppendLine("");
            // enum から 文字列を取り出すメソッド
            buff.AppendLine("    /// <summary>");
            buff.AppendLine("    /// 指定したタイプに対応するアクセスキーを取得します。");
            buff.AppendLine("    /// </summary>");
            buff.AppendLine($"    public static string GetKey({enumName} type) => map[type];");
            buff.AppendLine("}");

            string def = buff.ToString();
            Log.Trace(def);
            return def;
        }

        /// <summary>
        /// 指定した <see cref="SpriteAtlas"/> からキーを全て列挙します。
        /// </summary>
        public static IEnumerable<string> GetKeys(this SpriteAtlas atlas)
        {
            //全Spriteを取得し、さらにその名前を取得(Cloneは除去する)
            var spriteArray = new Sprite[atlas.spriteCount];
            atlas.GetSprites(spriteArray);

            foreach (Sprite s in spriteArray)
            {
                yield return s.name.Replace("(Clone)", "");
            }
        }
    }
}