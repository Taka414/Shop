//
// (C) 2022 Takap.
//

using R3;

namespace Takap.Utility
{
    /// <summary>
    /// シーン遷移に関するイベント通知を提供します。
    /// </summary>
    public interface ISceneTransitionEvents
    {
        /// <summary>
        /// シーン変更が終了した時に発生します。
        /// </summary>
        Observable<Unit> SceneChangeCompleted { get; }
    }
}