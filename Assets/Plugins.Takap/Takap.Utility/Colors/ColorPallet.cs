//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility.Colors
{
    /// <summary>
    /// ゲームで使用するカラーパレットを定義します。
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Color/ColorSource")]
    public class ColorPallet : SerializedScriptableObject
    {
        //
        // Inner Types
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 色を保持するためのクラス
        /// </summary>
        [Serializable]
        public class ColorDef
        {
            const string grp = "a";

            // 入りの名前
            [SerializeField, HorizontalGroup(grp), HideLabel] string _name;
            // 色の番号
            [SerializeField, HorizontalGroup(grp), HideLabel] int _id;
            // 対応する色
            [SerializeField, HorizontalGroup(grp), HideLabel] Color _color;

            /// <summary>
            /// 色の番号を取得します。
            /// </summary>
            public int Key => _id;

            /// <summary>
            /// 色を取得します。
            /// </summary>
            public Color Color => _color;
        }

        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        // 存在しない色を要求したときの色
        public static readonly Color FAILED_COLOR = new Color(0xE6 / 255F, 0, 0x7E / 255F); // pink

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // カラーパレットの名前
        [SerializeField] string _palletName;
        // カラーパレットの定義
        [SerializeField] List<ColorDef> _colorList;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // キャッシュ
        private Dictionary<int, Color> _colorTable;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した番号の取得します。
        /// </summary>
        public Color GetColor(int key)
        {
            if (_colorTable == null)
            {
                _colorTable = new(); // 初回アクセス時にキャッシュを作成
            }

            if (_colorTable.Count != _colorList.Count)
            {
                _colorTable.Clear();
                foreach (ColorDef item in _colorList)
                {
                    if (_colorTable.ContainsKey(item.Key))
                    {
                        Debug.LogWarning($"色の指定が重複しています。ID={item.Key}");
                        continue;
                    }
                    else
                    {
                        _colorTable[item.Key] = item.Color;
                    }
                }
            }

            if (_colorTable.ContainsKey(key))
            {
                return _colorTable[key];
            }
            else
            {
                Debug.Log($"未定義の色IDが指定されました。ID={key}");
                return FAILED_COLOR;
            }
        }

        /// <summary>
        /// キャッシュを作成します。
        /// </summary>
        private void CreateCacheTable()
        {
            if (_colorTable == null)
            {
                _colorTable = new(); // 初回アクセス時にキャッシュを作成
            }

            if (_colorTable.Count != _colorList.Count)
            {
                _colorTable.Clear();
                foreach (ColorDef item in _colorList)
                {
                    if (_colorTable.ContainsKey(item.Key))
                    {
                        Debug.LogWarning($"色の指定が重複しています。ID={item.Key}");
                        continue;
                    }
                    else
                    {
                        _colorTable[item.Key] = item.Color;
                    }
                }
            }
        }

        /// <summary>
        /// [Editor] 色を変更したときになどにキャッシュテーブルを再作成します。
        /// </summary>
        [Button]
        public void Reload()
        {
            _colorTable.Clear();
            CreateCacheTable();
        }
    }
}
