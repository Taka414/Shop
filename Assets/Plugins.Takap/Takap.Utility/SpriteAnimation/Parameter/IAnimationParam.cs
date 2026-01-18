//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// アニメーションの設定を表します。
    /// </summary>
    public interface IAnimationParam
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - -

        // 無限に繰り返すときの指定
        const int INFINIT_LOOPS = -1;

        /// <summary>
        /// パラメータの名前を得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 画像を何回するのかを設定または取得します。
        /// </summary>
        /// <remarks>
        /// <see cref="INFINIT_LOOPS"/> (-1) の場合無限ループを表します。
        /// </remarks>
        int LoopsCount { get; set; }

        /// <summary>
        /// 画像を切り替える速度を秒単位で設定または取得します。
        /// </summary>
        float FrameSpeed { get; set; }

        /// <summary>
        /// 左右を反転するかどうかを設定または取得します。
        /// </summary>
        bool FlipX { get; set; }

        /// <summary>
        /// アニメーションする <see cref="Sprite"/> のリストを取得します。
        /// </summary>
        List<Sprite> SpriteList { get; }
    }
}