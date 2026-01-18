//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Tween"/> の機能を拡張します。
    /// </summary>
    public static class TweenExtensions
    {
        //
        // DOTween を使う際の注意点
        // - - - - - - - - - - - - - - - - - - - -
        #region...
        //
        // ★Capacity
        // ------------------------------------------------------------------
        // 最初のTweenを実行するときもしくはDOTween.Init()をコールした時にまとめてメモリ確保を実行します。
        // 以下のメソッドで確保するメモリ量を指定できます。
        //
        // DOTween.SetTweensCapacity(Tween数, Sequence数);
        // デフォルトはTween200、Sequence50です。
        // 最初から余裕をもったCapacityを設定しておきましょう。
        //
        // スマートフォン端末などでは初回のGCAllocでほぼ間違いなく画面がカクつきます。
        // DOTween.Init()をロード画面など見た目に影響が出ないところで実行しておきましょう。
        // ------------------------------------------------------------------
        //
        // ★Killの罠
        // ------------------------------------------------------------------
        // // Tweenはこれでもいいし、t.Kill()でもOK
        // Tween t = transform.DOLocalMove(new Vector3(0, 0, 10f), 0.5f);
        // DOTween.Kill(t);
        // 
        // // SequenceではDOTween.Kill()ではKillできない！！
        // Sequence sequence = DOTween.Sequence();
        // sequence.Kill(); // 必ずこちらで
        // ------------------------------------------------------------------
        //
        // とはいえ、通常は以下のように書いているので問題なさそう。
        //
        // Tween _anim;
        // _anum = DOTween.Sequence();
        // _anum.Kill();
        // 
        #endregion

        /// <summary>
        /// 指定したオブジェクトが破棄されたら Tween も破棄されるように設定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SetLink<T>(this T self, Component component) where T : Tween
        {
            return self.SetLink(component.gameObject);
        }

        /// <summary>
        /// 無限に繰り返すように設定します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SetInfinitLoops<T>(this T t) where T : Tween => t.SetLoops(-1);

        /// <summary>
        /// 全てのアニメーションを停止します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void KillAll<T>(this List<T> self) where T : Tween => self.ForEach(tween => tween.Kill());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void KillAll<T>(this IEnumerable<T> list) where T : Tween => list.ForEach(tween => tween.Kill());

        /// <summary>
        /// 全てのアニメーションを停止してリストをクリアします。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void KillAllAndClear<T>(this List<T> self) where T : Tween
        {
            self.ForEach(tween => tween.Kill());
            self.Clear();
        }
    }
}
