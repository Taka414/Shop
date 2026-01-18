//
// (c) 2020 Takap.
//

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility.Fonts
{
    /// <summary>
    /// ビットマップフォントを定義します。
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Font/FontSet")]
    public class BitmapFontSetting : ScriptableObject
    {
        //
        // Constant
        // - - - - - - - - - - - - - - - - - - - -

        // Pivot Center用
        static readonly Vector2 _center = new Vector2(0.5f, 0.5f);

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 1文字の情報を表します。
        /// </summary>
        [Serializable]
        public class CharInfo
        {
            [HorizontalGroup("グループ A", LabelWidth = 20), LabelText("C")]
            [SerializeField]
            string _text;
            [HorizontalGroup("グループ A"), LabelText("L")]
            [SerializeField]
            int _x;
            [HorizontalGroup("グループ A"), LabelText("T")]
            [SerializeField]
            int _y;
            [HorizontalGroup("グループ A"), LabelText("H")]
            [SerializeField]
            int _height = 1;
            [HorizontalGroup("グループ A"), LabelText("W")]
            [SerializeField]
            int _width = 1;

            public string Text { get => _text; set => _text = value; }
            public int X { get => _x; set => _x = value; }
            public int Y { get => _y; set => _y = value; }
            public int Width { get => _width; set => _width = value; }
            public int Height { get => _height; set => _height = value; }
        }

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField, LabelText("対象テクスチャ")]
        Texture2D _sourceTexture;

        [SerializeField, LabelText("空白文字用テクスチャ")]
        Texture2D _spaceTexture;

        [SerializeField, LabelText("画像の余白: Top")]
        int _marginTop;

        [SerializeField, LabelText("画像の余白: Left")]
        int _marginLeft;

        [SerializeField, MinValue(0), LabelText("行ごと文字の間隔(上下)")]
        int _margiCharnVertical;

        [SerializeField, MinValue(1), LabelText("1文字の大きさ(高さ)")]
        int _charHeight = 1;

        [SerializeField, LabelText("文字マップ")]
        TextAsset _charMap;

        [SerializeField, LabelText("スペースの横幅")]
        int _spaceWidth = 1;

        [SerializeField, ReadOnly]
        List<CharInfo> _infoList = new List<CharInfo>();

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 定義のキャッシュ
        [ShowInInspector, ReadOnly]
        Dictionary<string, CharInfo> _infoTable;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// [開発用] フォントの位置情報を算出してオブジェクトを初期化します。
        /// </summary>
        [Button]
        public void Setup()
        {
            //ClearCache();

            if (!_charMap)
            {
                Log.Warn("文字マップが設定されていません。");
                return;
            }

            InitInfoList();
            InitInfoTable();
            //InitCache();
        }

        /// <summary>
        /// 現在利用可能な文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAvailableChars()
        {
            foreach (CharInfo item in _infoList)
            {
                yield return item.Text;
            }
            yield return " "; // 空白文字のサポート
        }

        /// <summary>
        /// 指定した文字に対応するスプライトを取得します。
        /// </summary>
        public Sprite GetSprite(string text, float pixelsPerUnit)
        {
            InitInfoTable();

            if (!_infoTable.ContainsKey(text))
            {
                Log.Warn($"'{text}' は存在しません。", this);
                return null;
            }

            if (text == " ")
            {
                var _rect = new Rect(0, 0, _spaceWidth, _charHeight);
                var _sp = Sprite.Create(_spaceTexture, _rect, _center, pixelsPerUnit, 0, SpriteMeshType.FullRect);
                _sp.name = "[Space]";
                return _sp;
            }

            CharInfo info = _infoTable[text];
            var rect = new Rect(info.X, info.Y, info.Width, info.Height);
            var sp = Sprite.Create(_sourceTexture, rect, _center, pixelsPerUnit, 0, SpriteMeshType.FullRect);
            sp.name = text;

            return sp;
        }

        //
        // Non-Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        // キャッシュ用のマップを初期化する
        void InitInfoTable()
        {
            if (_infoTable != null)
            {
                return;
            }

            _infoTable = new Dictionary<string, CharInfo>();
            foreach (CharInfo item in _infoList)
            {
                _infoTable[item.Text] = item;
            }
        }

        // 現在のオブジェクトの情報に従って各文字のスライス位置を事前計算する
        void InitInfoList()
        {
            _infoList.Clear();

            var charMap = CharMap.Load(_charMap);

            // 左上 → 右下に設定値で画像を切り抜いていく

            for (int y = 0; true; y++)
            {
                // 1行の高さは固定
                int ypos = _marginTop;
                if (y != 0)
                {
                    ypos += (_charHeight + _margiCharnVertical) * y;
                }

                if (ypos > _sourceTexture.height)
                {
                    break;
                }

                if (y > 1000)
                {
                    Log.Warn("1000回を超えたので中断しました。");
                    break;
                }

                // 左下がゼロなので本当の位置を出す
                int _ypos = _sourceTexture.height - ypos;

                // 水平方向は自動で分割
                int x = 0;
                for (int xpos = _marginLeft; xpos < _sourceTexture.width; /*nop*/)
                {
                    // 対応する文字をマップから取り出す
                    if (!charMap.TryGetChar(x++, y, out char _c))
                    {
                        break;
                    }

                    if (!GetRectangle(xpos, _ypos, _charHeight, out CharInfo rect))
                    {
                        break;
                    }

                    _infoList.Add(new CharInfo()
                    {
                        Text = _c.ToString(),
                        X = rect.X,
                        Y = rect.Y - rect.Height,
                        Width = rect.Width,
                        Height = rect.Height,
                    });

                    xpos = rect.X + rect.Width;
                }
            }
        }

        // 指定した位置から左方向に走査して有効なビットマップ中の有効な文字領域を取得する
        // true : 矩形を選択できた / false : それ以外(右端に到達)
        bool GetRectangle(/*Bitmap bmp, */int x, int y, int height, out CharInfo rect)
        {
            int beginX = x;
            int width = 1;
            rect = new CharInfo();

            // 開始位置の検索
            for (int xp = x; xp < _sourceTexture.width; xp++, beginX++)
            {
                if (IsValidPixel(/*bmp,*/ xp, y, height))
                {
                    break;
                }
            }

            if (beginX >= _sourceTexture.width)
            {
                return false;
            }

            // 幅の選択
            for (int xp = beginX + 1; xp < _sourceTexture.width; xp++, width++)
            {
                if (!IsValidPixel(/*bmp, */xp, y, height) && !IsValidPixel(/*bmp, */xp + 1, y, height))
                {
                    break; // 最低2px以上で違う文字と認識する
                }
            }

            if (x + width >= _sourceTexture.width)
            {
                return false;
            }

            rect.X = beginX;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;

            return true;
        }

        // 指定した位置を上端として下側にheight分の範囲に有効な行が存在するかどうか確認する
        // true : 存在する / false : それ以外
        //   0 1 2
        // 0 0 0 a ↓ こういう方向で検索する 
        // 1 0 0 a ↓
        // 2 0 0 a ↓
        bool IsValidPixel(/*Bitmap bmp, */int x, int y, int height)
        {
            for (int yp = 0; yp <= height; yp++)
            //for (int yp = this.sourceTexture.height; yp < height; yp--)
            {
                //Color c = this.sourceTexture.GetPixel(x, y + yp - this.sourceTexture.height);
                Color c = _sourceTexture.GetPixel(x, y - yp);
                if (c.a != 0)
                {
                    return true; // 不透明ピクセルは有効
                }
            }
            return false;
        }

        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -
        #region...

        /// <summary>
        /// どういう風に文字が並んでいるかを表します。
        /// </summary>
        public class CharMap
        {
            private readonly List<List<char>> _items = new List<List<char>>();

            public bool TryGetChar(int x, int y, out char c)
            {
                c = default;
                try
                {
                    c = _items[y][x];
                    return true;
                }
                catch (Exception) // 範囲外は例外で処理
                {
                    return false;
                }
            }

            public static CharMap Load(TextAsset textAsset)
            {
                //string[] lines = File.ReadAllLines(filePath);
                string[] lines = textAsset.text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                var map = new CharMap();
                foreach (string line in lines)
                {
                    string trimedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimedLine) || trimedLine.StartsWith("!"))
                    {
                        continue; // コメント行、空行の読み飛ばし
                    }

                    var subList = new List<char>();
                    map._items.Add(subList);

                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int p = 0; p < parts.Length; p++)
                    {
                        subList.Add(parts[p][0]);
                    }
                }
                return map;
            }
        }

        #endregion
    }
}
