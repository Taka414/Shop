//
// (C) 2025 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// DI向けのログ出力機能を表します。
    /// </summary>
    /// <remarks>
    /// なるべく.NET標準に近い形にしておく
    /// </remarks>
    public interface IAppLogger
    {
        /// <summary>
        /// 指定したレベルでログを出力します。
        /// </summary>
        void Log(AppLogLevel logLevel, string message, object context = null);
    }
}
