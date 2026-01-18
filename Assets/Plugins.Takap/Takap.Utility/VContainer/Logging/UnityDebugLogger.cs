//
// (C) 2025 Takap.
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// Unityのデバッグコンソール向けの出力を行うログの実装を表します。
    /// </summary>
    public class UnityDebugLogger : IAppLogger
    {
        // 説明:
        // 以下の定義で2通りの動作を定義している
        //   (1) 'DEBUG' シンボルが定義されている場合ログ出力を行う
        //   (2) 'DEBUG' シンボルが定義されてない場合でもエディター上ならログ出力を行う
#if DEBUG || UNITY_EDITOR
        static readonly bool IsEnabled = true;
        static readonly AppLogLevel MinLevel = AppLogLevel.Trace;
#else
        static readonly bool IsEnabled = false;
        static readonly AppLogLevel MinLevel = AppLogLevel.Information;
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(AppLogLevel logLevel, string message, object context = null)
        {
            if (!IsEnabled || logLevel == AppLogLevel.None || logLevel < MinLevel)
            {
                return; // こういう時は出力しない
            }
            if (context is UnityEngine.Object uo)
            {
                UnityEngine.Debug.unityLogger.Log(ToUnityLogType(logLevel), (object)message, uo);
            }
            else
            {
                UnityEngine.Debug.unityLogger.Log(ToUnityLogType(logLevel), message);
            }
        }

        // 自作の.NET風のログレベルをUnityのログ種別に変換
        static LogType ToUnityLogType(AppLogLevel level)
        {
            return level switch
            {
                AppLogLevel.Trace => LogType.Log,
                AppLogLevel.Debug => LogType.Log,
                AppLogLevel.Information => LogType.Log,
                AppLogLevel.Warning => LogType.Warning,
                AppLogLevel.Error => LogType.Error,
                AppLogLevel.Critical => LogType.Exception,
                _ => LogType.Log
            };
        }
    }
}
