//
// (C) 2022 Takap.
//

using System;
using System.Diagnostics;

namespace Takap.Utility
{
    /// <summary>
    /// 簡単に時間計測用をするためのクラス 
    /// </summary>
    public static class StopwatchContext
    {
        /// <summary>
        /// 時間を計測して時間を取得します。戻り値なし版。
        /// </summary>
        public static TimeSpan Run(Action f, int count = 1)
        {
            var sw = new Stopwatch();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                f();
                sw.Stop();
            }

            return sw.Elapsed;
        }

        /// <summary>
        /// 時間を計測して時間を取得します。戻り値あり版。
        /// </summary>
        public static TimeSpan Run<TResult>(Func<TResult> f, int count = 1)
        {
            var sw = new Stopwatch();
            sw.Reset();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                TResult restul = f(); // 読み捨て
                sw.Stop();
            }

            return TimeSpan.FromTicks(sw.ElapsedTicks);
        }

        /// <summary>
        /// [Unity用] に1回処理を実行して計測結果をログに出力する。戻り値なし版。
        /// </summary>
        public static void RunUnity(Action f, string key = "")
        {
            var sw = Stopwatch.StartNew();
            f();
            sw.Stop();
            Log.Trace($"{key} = {sw.Elapsed.TotalMilliseconds}msec");
        }

        /// <summary>
        /// [Unity用] に1回処理を実行して計測結果をログに出力する。戻り値あり版。
        /// </summary>
        public static TResult RunUnity<TResult>(Func<TResult> f, string key)
        {
            var sw = Stopwatch.StartNew();
            TResult ret = f();
            sw.Stop();
            Log.Trace($"{key} = {sw.Elapsed.TotalMilliseconds}msec");
            return ret;
        }
    }
}
