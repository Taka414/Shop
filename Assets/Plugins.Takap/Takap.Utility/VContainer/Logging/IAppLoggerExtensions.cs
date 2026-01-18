//
// (C) 2025 Takap.
//

using System;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using Takap.Utility;

namespace UnityEngine
{
    // Unityを使用時(つまりUnityEngineが必要なやつら)の場合UnityEngine.Objectが使える拡張を利用する
    public static class IAppLoggerExtensions
    {
        // インターフェースはシンプルにクリーンにしつつ煩雑なショートカット系はコンテキストごとに拡張メソッドで対応
        public static void Trace(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Trace, BuildLogMsg("[Trace]", path, name, line, message), context);
        }
        public static void Debug(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Debug, BuildLogMsg("[Debug]", path, name, line, message), context);
        }
        public static void Info(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Information, BuildLogMsg("[Info]", path, name, line, message), context);
        }
        public static void Warn(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Warning, BuildLogMsg("[Warn]", path, name, line, message), context);
        }
        public static void Error(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Error, BuildLogMsg("[Error]", path, name, line, message), context);
        }
        public static void Error(this IAppLogger logger, Exception ex, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Error, BuildLogMsg("[Error]", path, name, line, ex.ToString()), context);
        }
        public static void Fatal(this IAppLogger logger, string message, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            logger.Log(AppLogLevel.Critical, BuildLogMsg("[Fatal]", path, name, line, message), context);
        }
        public static void Fatal(this IAppLogger logger, Exception ex, UnityEngine.Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            // こういう個別のやつも色々あつから拡張メソッドで対応してインターフェースは増やさない
            logger.Log(AppLogLevel.Critical, BuildLogMsg("[Fatal]", path, name, line, ex.ToString()), context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string BuildLogMsg(string logLevelStr, string path, string name, int line, string msg)
        {
            using var builder = ZString.CreateStringBuilder();
            builder.Append(logLevelStr);
            builder.Append(", ");
            builder.Append(System.IO.Path.GetFileName(path));
            builder.Append(", ");
            builder.Append(name);
            builder.Append(", ");
            builder.Append(line);
            builder.Append(", ");
            builder.Append(msg);
            return builder.ToString();
            // 以下の結合がUnityだと激遅なのでOSSを使用する
            //return $"{type}, {Path.GetFileName(filePath)}, {method}, {line.ToString()}, {msg.ToString()}";
        }
    }
}
