//
// (c) 2022 Takap.
//

using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

namespace Takap.Utility.Fonts
{
    /// <summary>
    /// フォントのキャッシュを管理します。
    /// </summary>
    public class BitmapFontCache : MonoBehaviour
    {
        //
        // InnerTypes
        // - - - - - - - - - - - - - - - - - - - -

        [Serializable]
        public readonly struct SpriteCacheKey  : IEquatable<SpriteCacheKey>
        {
            [ShowInInspector, ReadOnly]
            public readonly string FontName;

            [ShowInInspector, ReadOnly]
            public readonly string Str;
            
            [ShowInInspector, ReadOnly]
            public readonly float PixelsPerUnit;

            public SpriteCacheKey(string fontName, string str, float pixelPerUnit)
            {
                FontName = fontName;
                Str = str;
                PixelsPerUnit = pixelPerUnit;
            }

            // structをDictionaryのTKeyに指定する場合以下を実装しないと処理が遅くなる

            public bool Equals(SpriteCacheKey other)
            {
                return ReferenceEquals(this, other) ||
                       FontName == other.FontName &&
                       Str == other.Str &&
                       PixelsPerUnit == other.PixelsPerUnit;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(FontName, Str, PixelsPerUnit);
            }
        }

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        [ShowInInspector, ReadOnly]
        Dictionary<SpriteCacheKey, Sprite> _table = new();

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// ビットマップフォント用の画像をキャッシュします。
        /// </summary>
        public void AddSprite(string fontName, string cStr, float pixelPerUnit, Sprite sp)
        {
            SpriteCacheKey key = new(fontName, cStr, pixelPerUnit);
            if (_table.ContainsKey(key))
            {
                return;
            }
            _table[key] = sp;
        }

        /// <summary>
        /// キャッシュしたオブジェクトを取得します。
        /// </summary>
        public Sprite GetSprite(string fontName, string cStr, float pixelsPerUnit)
        {
            SpriteCacheKey key = new(fontName, cStr, pixelsPerUnit);
            return _table[key];
        }

        public bool TryGetSprite(string fontName, string cStr, float pixelsPerUnit, out Sprite sp)
        {
            sp = null;
            
            SpriteCacheKey key = new(fontName, cStr, pixelsPerUnit);
            if (_table.ContainsKey(key))
            {
                sp = _table[key];
            }
            return sp != null;
        }

        /// <summary>
        /// キャッシュが存在するかどうかを取得します。
        /// </summary>
        public bool HasCache(string fontName, string cStr, float pixelsPerUnit)
        {
            SpriteCacheKey key = new(fontName, cStr, pixelsPerUnit);
            return _table.ContainsKey(key);
        }
    }
}