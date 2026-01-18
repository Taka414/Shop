//
// (C) 2022 Takap.
//

using Cysharp.Threading.Tasks;

namespace Takap.Utility
{
    /// <summary>
    /// シーン遷移（フェード演出付き）を制御するサービスを表します。  
    /// </summary>
    public interface ISceneTransitionService
    {
        /// <summary>
        /// 画面をフェードアウトします。
        /// </summary>
        UniTask FadeOut(float duration);

        /// <summary>
        /// 画面をフェードインします。
        /// </summary>
        UniTask FadeIn(float duration);

        /// <summary>
        /// 操作を停止してフェードイン/アウトを実行しながら指定したシーンに変更します。
        /// </summary>
        /// <param name="sceneName">次のシーン名</param>
        /// <returns></returns>
        UniTask ChangeSceneWithFade(string sceneName);

        /// <summary>
        /// 操作を停止してフェードイン/アウトを実行しながら指定したシーンに変更します。
        /// </summary>
        /// <param name="sceneName">次のシーン名</param>
        /// <param name="outDuration">(秒)</param>
        /// <param name="inDuration">(秒)</param>
        /// <returns></returns>
        UniTask ChangeSceneWithFade(string sceneName, float outDuration, float inDuration);
    }
}