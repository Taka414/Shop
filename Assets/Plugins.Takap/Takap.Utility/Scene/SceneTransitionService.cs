//
// (C) 2022 Takap.
//

using Cysharp.Threading.Tasks;
using R3;

namespace Takap.Utility
{
    /// <summary>
    /// シーン変更を行います。
    /// </summary>
    public class SceneTransitionService : ISceneTransitionService, ISceneTransitionEvents
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        readonly IScreenFader _screenFader;
        readonly ISimpleTouchBlocker _touchBlocker;

        //
        // Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// シーン変更が終了した時に発生します。
        /// </summary>
        public Observable<Unit> SceneChangeCompleted => _sceneChangeCompleted;
        readonly Subject<Unit> _sceneChangeCompleted = new();

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public SceneTransitionService(IScreenFader screenFader,
                           ISimpleTouchBlocker touchBlocker)
        {
            _screenFader = screenFader;
            _touchBlocker = touchBlocker;
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 画面をフェードアウトします。
        /// </summary>
        public UniTask FadeOut(float duration)
        {
            return _screenFader.FadeOut(duration);
        }

        /// <summary>
        /// 画面をフェードインします。
        /// </summary>
        public UniTask FadeIn(float duration)
        {
            return _screenFader.FadeIn(duration);
        }

        /// <summary>
        /// 操作を停止してフェードイン/アウトを実行しながら指定したシーンに変更します。
        /// </summary>
        /// <param name="sceneName">次のシーン名</param>
        /// <returns></returns>
        public async UniTask ChangeSceneWithFade(string sceneName)
        {
            _touchBlocker.EnableBlock();

            await _screenFader.ChangeSceneWithFade(sceneName);

            _touchBlocker.DisableBlock();

            _sceneChangeCompleted.OnNext(Unit.Default);
        }

        /// <summary>
        /// 操作を停止してフェードイン/アウトを実行しながら指定したシーンに変更します。
        /// </summary>
        /// <param name="sceneName">次のシーン名</param>
        /// <param name="outDuration">(秒)</param>
        /// <param name="inDuration">(秒)</param>
        /// <returns></returns>
        public async UniTask ChangeSceneWithFade(string sceneName, float outDuration, float inDuration)
        {
            _touchBlocker.EnableBlock();

            await _screenFader.ChangeSceneWithFade(sceneName, outDuration, inDuration);

            _touchBlocker.DisableBlock();

            _sceneChangeCompleted.OnNext(Unit.Default);
        }
    }
}