//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// スプライトアニメーションで必要なパラメーターを格納します。
    /// </summary>
    [Serializable]
    public class AnimationParam : IAnimationParam
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - -

        // 無限に繰り返すときの指定
        const int INFINIT_LOOPS = -1;

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // このパラメーターの名前
        [SerializeField] string _name = "";

        // アニメーションのループ回数
        // -1 で無限繰り返し、無限繰り返しの場合Completeは発生しない
        [SerializeField] int _loopsCount = -1;

        // 画像を切り替える速度(秒)
        [SerializeField] float _speed = 0.1f;

        // 左右を反転するかどうか
        // true: 反転する / false: しない
        [SerializeField] bool _flipX;

        // 上下を反転するかどうか
        // true: 反転する / false: しない
        [SerializeField] bool _flipY;

        // アニメーションする画像のリスト
        [SerializeField] List<Sprite> _spriteList = new();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -


        public string Name { get => _name; set => _name = value; }

        public int LoopsCount { get => _loopsCount; set => _loopsCount = value; }

        public float FrameSpeed { get => _speed; set => _speed = value; }

        public bool FlipX { get => _flipX; set => _flipX = value; }

        public bool FlipY { get => _flipY; set => _flipY = value; }

        public List<Sprite> SpriteList => _spriteList;
    }
}