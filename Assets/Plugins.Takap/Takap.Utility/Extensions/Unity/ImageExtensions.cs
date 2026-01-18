//
// (C) 2022 Takap.
//

using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Image"/> クラスを拡張します。
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// 指定した画像に変更しつつネイティブな大きさを合わせます。
        /// </summary>
        public static void SetNativeSize(this Image self, Sprite sp)
        {
            self.sprite = sp;
            self.SetNativeSize();
        }
    }
}