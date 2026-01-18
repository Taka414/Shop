//
// (C) 2022 Takap.
//

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// テクスチャー操作に関係する汎用的な機能を提供します。
    /// </summary>
    public static class Texture2DUtility
    {
        // Pivot Center用
        private static readonly Vector2 _center = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// 指定したパラメーターでテクスチャーの一部を切り出します。
        /// </summary>
        public static Sprite GetSprite(Texture2D texure, int columns, int rows, int x, int y, float pixelsPerUnit = 100, SpriteMeshType meshType = SpriteMeshType.FullRect)
        {
            // 切り出す部分の大きさ
            int width = texure.width / columns;
            int height = texure.height / rows;

            // 切り出す開始位置
            float xpos = x * width;
            float ypos = (rows - y - 1) * height;

            Rect rect = new(xpos, ypos, width, height);
            var sp = Sprite.Create(texure, rect, _center, pixelsPerUnit, 0, meshType);
            return sp;
        }
    }

    /// <summary>
    /// テクスチャーを動的にスライスするためのスクリプト
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Texture/DynamicSliceTextureSource")]
    public class DynamicSliceTexture : ScriptableObject
    {
        //
        // 説明:
        // 画像をスライスするときにSpriteEditorで2000枚とかになると
        // 操作がめちゃくちゃ重くて作業に支障が出るので
        // 実行時に指定のサイズでスライスするようにする
        //

        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -

        // Pivot Center用
        private static readonly Vector2 _center = new Vector2(0.5f, 0.5f);
        // 5桁のゼロ埋め
        private const string FORMAT = "D5";

        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // 対象テクスチャーを縦横に等分割する

        [SerializeField, LabelText("対象テクスチャ")] Texture2D _sourceTexture;
        [SerializeField, MinValue(1), LabelText("横の分割数")] int _columns = 1; // width =100 で 25分割したら1辺=4
        [SerializeField, MinValue(1), LabelText("縦の分割数")] int _rows = 1;    // height=100 で 20分割したら1辺=5
        [SerializeField] float _pixelsPerUnit = 100;
        // 1枚当たりの画像の大きさ
        [SerializeField, ReadOnly] int _width;
        [SerializeField, ReadOnly] int _height;
        [SerializeField, ReadOnly] int _imageCount = -1;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 要素数を取得します。
        /// </summary>
        public int CellCount => _imageCount;

        /// <summary>
        /// 横の分割数を取得します。
        /// </summary>
        public int Columns => _columns;

        /// <summary>
        /// 縦の分割数を取得します。
        /// </summary>
        public int Rows => _rows;

        /// <summary>
        /// 1ユニットのピクセル数を取得します。
        /// </summary>
        public float PixelsPerUnit => _pixelsPerUnit;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        public void OnValidate()
        {
            // 1セルごとの画像サイズ
            _width = 0;
            _height = 0;
            if (_sourceTexture != null)
            {
                _width = _sourceTexture.width / _columns;
                _height = _sourceTexture.height / _rows;
            }

            _imageCount = _columns * _rows;
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定したインデックスの画像取得します。
        /// </summary>
        /// <remarks>
        /// インデックスは左上から右下に振られる
        /// e.g.
        /// > 0 1 2 3
        /// > 4 5 6 7
        /// > 8 9...
        /// 
        /// 注意:
        /// 動的に生成したSpriteは使用が終了したらDestryを呼ばないとリークする
        /// Sprite sp;
        /// sp.sprite = Getsprite(0);
        /// // こうすると直接代入すると古いSpriteはシーンに残る（＝リークしているように見える）
        /// 
        /// Sprite old = sp.sprite;
        /// Destry(old):
        /// sp.sprite = Getsprite(0); // 先に破棄してから入れ替えること
        /// </remarks>
        public Sprite GetSprite(int index, SpriteMeshType meshType = SpriteMeshType.FullRect)
        {
            // 左下が原点(0,0)なのでそのように計算する

            int _index = index;
            if (index > _imageCount)
            {
                _index = index % _imageCount;
            }

            (int x, int y) = GetIndexXY(_index);
            float xpos = x * _width;
            float ypos = (_rows - y - 1) * _height;
            var rect = new Rect(xpos, ypos, _width, _height);
            var sp = Sprite.Create(_sourceTexture, rect, _center, _pixelsPerUnit, 0, meshType);
            sp.name = _index.ToString(FORMAT);

            return sp;
        }

        /// <summary>
        /// 指定した位置の画像を取得します。
        /// </summary>
        public Sprite GetSprite(int x, int y, SpriteMeshType meshType = SpriteMeshType.FullRect)
        {
            if (x > _columns || x < 1 || y > _rows || y < 1)
            {
                string msg1 = $"Value out of range. ";
                string msg2 = $"Range(x:0-{_columns}, y:0-{_rows}), Value(x={x} y={y})";
                throw new ArgumentOutOfRangeException(msg1 + msg2);
            }

            return GetSprite(y + (x * _columns), meshType);
        }

        /// <summary>
        /// 画像から任意の位置を任意の大きさに切り出して Sprite を取得します。
        /// </summary>
        public Sprite GetSprite(int x, int y,
                                int width, int height,
                                float pixelsPerUnit = 100, uint extrude = 0,
                                SpriteMeshType meshType = SpriteMeshType.FullRect)
        {
            var rect = new Rect(x, y, width, height);
            return Sprite.Create(_sourceTexture, rect, _center, pixelsPerUnit, extrude, meshType);
        }

        /// <summary>
        /// 指定した番号の画像をこのオブジェクトが持っているかどうかを取得します。
        /// true : 存在する / false : 存在しない(=問い合わせても画像が取得できない)
        /// </summary>
        public bool HasImage(int index)
        {
            return index < _imageCount;
        }

        /// <summary>
        /// 指定した <see cref="SpriteRenderer"/> に指定した番号の画像を設定します。
        /// </summary>
        public void ChangeSprite(SpriteRenderer sr, int index = 0)
        {
            Sprite old = sr.sprite; // 古いほうを削除してから新しいのを設定する
            if (old)
            {
                Destroy(old);
            }

            Sprite sp = GetSprite(index);
            sr.sprite = sp;
        }

        /// <summary>
        /// 指定した <see cref="ISpriteSelector"/> 経由で指定した番号の画像を設定します。
        /// </summary>
        public void ChangeSprite(ISpriteSelector selector, int index = 0)
        {
            Sprite old = selector.Sprite;
            if (old)
            {
                Destroy(old);
            }

            Sprite sp = GetSprite(index);
            selector.Sprite = sp;
        }

        // 番号をXとYの成分に分解する
        private (int x, int y) GetIndexXY(int index)
        {
            int x = index % _columns;
            int y = index / _rows;
            return (x, y);
        }
    }
}