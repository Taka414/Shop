//
// (C) 2022 Takap.
//

using UnityEngine.Playables;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="PlayableDirector"/> の機能を拡張します。
    /// </summary>
    public static class PlayableDirectorExtensions
    {
        /// <summary>
        /// アニメーションが再生中だった場合停止して最初から再生します。
        /// </summary>
        public static void PlayOne(this PlayableDirector self)
        {
            if (!self)
            {
                return;
            }

            if (self.state == PlayState.Playing)
            {
                self.Stop();
            }
            self.Play();
        }

        /// <summary>
        /// アニメーションの最後までジャンプします。
        /// タイムラインの途中イベントは全て一気に発生します。
        /// </summary>
        public static void JumpTail(this PlayableDirector self)
        {
            self.time = self.duration;
        }
    }
}