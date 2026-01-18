//
// (C) 2025 Takap.
//

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// フェード管理とシーン変更を表します。
    /// </summary>
    public interface IScreenFader
    {
        /// <summary>
        /// フェード時の色を設定または取得します。
        /// </summary>
        Color FadeColor { get; set; }

        /// <summary>
        /// 現在シーンがフェードアウト中かどうかを取得します。
        /// </summary>
        bool IsFadeOut { get; }

        /// <summary>
        /// 画面をフェードアウトします。
        /// </summary>
        UniTask FadeOut(float duration);

        /// <summary>
        /// 画面をフェードインします。
        /// </summary>
        UniTask FadeIn(float duration);

        /// <summary>
        /// 画面をフェードアウトした後に指定したシーンに遷移してフェードインします。
        /// </summary>
        UniTask ChangeSceneWithFade(string sceneName);

        /// <summary>
        /// 画面をフェードアウトした後に指定したシーンに遷移してフェードインします。
        /// </summary>
        UniTask ChangeSceneWithFade(string sceneName, float outDuration, float inDuration);
    }
}