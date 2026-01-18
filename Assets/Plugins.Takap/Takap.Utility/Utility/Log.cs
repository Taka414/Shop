//
// (C) 2022 Takap.
//

using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 自作のログ出力用のクラスを表します。
    /// </summary>
    /// <remarks>
    /// リリース時にログを出力しないようにする機能を持った Debug.Log をラップするクラス
    /// </remarks>
    public static class Log
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -
        #region...

        // 説明:
        // 以下の定義で2通りの動作を定義している
        //   (1) 'DEBUG' シンボルが定義されている場合ログ出力を行う
        //   (2) 'DEBUG' シンボルが定義されてない場合でもエディター上ならログ出力を行う
#if DEBUG
        const string CONDITIONAL_TEXT = "DEBUG";
#else
        const string CONDITIONAL_TEXT = "UNITY_EDITOR";
#endif

        const string DEBUG = "[Debug]";
        const string TRACE = "[Trace]";
        const string WARN = "[Warn]";
        const string ERROR = "[Error]";
        const string SEPARAOTR1 = " ";
        const string SEPARAOTR2 = ", ";

        #endregion

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        [Conditional(CONDITIONAL_TEXT)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message, Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            if (context)
            {
                UnityEngine.Debug.Log(BuildLogMsg(DEBUG, path, name, line, message), context);
            }
            else
            {
                UnityEngine.Debug.Log(BuildLogMsg(DEBUG, path, name, line, message));
            }
        }

        [Conditional(CONDITIONAL_TEXT)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Trace(string message, Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            if (context)
            {
                UnityEngine.Debug.Log(BuildLogMsg(TRACE, path, name, line, message), context);
            }
            else
            {
                UnityEngine.Debug.Log(BuildLogMsg(TRACE, path, name, line, message));
            }
        }

        [Conditional(CONDITIONAL_TEXT)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(string message, Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            if (context)
            {
                UnityEngine.Debug.LogWarning(BuildLogMsg(WARN, path, name, line, message), context);
            }
            else
            {
                UnityEngine.Debug.LogWarning(BuildLogMsg(WARN, path, name, line, message));
            }
        }

        [Conditional(CONDITIONAL_TEXT)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(string message, Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            if (context)
            {
                UnityEngine.Debug.LogError(BuildLogMsg(ERROR, path, name, line, message), context);
            }
            else
            {
                UnityEngine.Debug.LogError(BuildLogMsg(ERROR, path, name, line, message));
            }
        }

        [Conditional(CONDITIONAL_TEXT)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Exception(System.Exception exception, Object context = null, [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            if (context)
            {
                UnityEngine.Debug.LogException(exception, context);
            }
            else
            {
                UnityEngine.Debug.LogException(exception);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string BuildLogMsg(string type, string filePath, string method, int line, string msg)
        {
            var builder = ZString.CreateStringBuilder();
            builder.Append(type);
            builder.Append(SEPARAOTR1);
            builder.Append(Path.GetFileName(filePath));
            builder.Append(SEPARAOTR2);
            builder.Append(method);
            builder.Append(SEPARAOTR2);
            builder.Append(line);
            builder.Append(SEPARAOTR2);
            builder.Append(msg);
            return builder.ToString();
            //return $"{type}, {Path.GetFileName(filePath)}, {method}, {line.ToString()}, {msg.ToString()}";
        }
    }
}
