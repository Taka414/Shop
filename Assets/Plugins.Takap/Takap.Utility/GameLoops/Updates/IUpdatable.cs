//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 自分で Update を管理する事を表す
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// フレームごとに <see cref="UpdateLocalCore"/> を呼び出すかどうかを取得します。
        /// true: 呼び出す / false: しない
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// フレームごとに呼び出される処理
        /// </summary>
        void UpdateLocalCore();
    }
}