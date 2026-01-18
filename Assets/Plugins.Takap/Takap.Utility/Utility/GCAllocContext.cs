//
// (C) 2022 Takap.
//

using System;
using Unity.Profiling;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// GC Alloc がどれくらいされたのかを計測するためのクラス
    /// </summary>
    public readonly struct GCAllocContext : IDisposable
    {
        // 
        // 使い方:
        // 
        // private void Update()
        // {
        //     using (CheckGCAllocScope.Create("new List")) // こうやって計測範囲を囲うとログにGC Alloc が表示される
        //     {
        //         var list = new List<int>();
        //     }
        // }

#if ENABLE_RELEASE
        public void Dispose()
        {
        }

        public static CheckGCAllocScope Create(string name)
        {
            return default;
        }
#else
        // GCAlloc=0でも自分自身のインスタンス化で32と表示されるのでオフセットを乗せる
        const int OFFSET = 32;
        // 計測用
        private static ProfilerRecorder _profilerRecorder;
        // 計測の名前(ログに出すやつ)
        private readonly string _name;
        // 開始時のメモリ状態
        private readonly long _startValue;

        /// <summary>
        /// 名前を指定してオブジェクトを作成します。
        /// </summary>
        private GCAllocContext(string name) => (_name, _startValue) = (name, _profilerRecorder.CurrentValue + OFFSET);

        /// <summary>
        /// 測定区間を開始します。
        /// </summary>
        public static IDisposable Scope(string name) => new GCAllocContext(name);

        /// <summary>
        /// 計測区間を終了します。
        /// </summary>
        public void Dispose()
        {
            var endValue = _profilerRecorder.CurrentValue;
            var value = endValue - _startValue;
            Debug.Log($"[GC Alloc] {_name}: {value}");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeInitializeOnLoadMethod()
        {
            _profilerRecorder.Dispose();
            _profilerRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
        }
#endif
    }
}