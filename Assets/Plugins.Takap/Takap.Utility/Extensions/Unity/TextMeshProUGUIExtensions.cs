//
// (C) 2022 Takap.
//

using TMPro;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="TextMeshProUGUI"/> を拡張します。
    /// </summary>
    public static class TextMeshProUGUIExtensions
    {
        /// <summary>
        /// インスタンスが無くてもエラーを出さずに文字列を設定します。
        /// </summary>
        public static void SetTextSafe(this TextMeshProUGUI self, string value)
        {
            if (self != null)
            {
                self.text = value;
            }
        }

        /// <summary>
        /// テキストのサイズを取得します。
        /// </summary>
        public static (float textWidth, float textHeight) GetTextSize(this TextMeshProUGUI self)
        {
            return (self.preferredWidth, self.preferredHeight);
        }
    }
}

