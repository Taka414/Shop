//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <para>ゲーム用のランダムデータを簡単に生成できます。</para>
    /// <para><see cref="UnityEngine.Random"/> と <see cref="System.Random"/> の名前が紛らわしいのを解決します。</para>
    /// </summary>
    public static class UniRandom
    {
        // 
        // 補足:
        // 基本的にRandomクラスを使いたくないからメソッドを全部ラップして転送するような実装をしている
        // 

        // 
        // UnityEngine.Randomのラッパー
        // 
        #region...

        /// <summary>
        /// 一様分布のランダムな回転を返します（読み取り専用）
        /// </summary>
        public static Quaternion rotationUniform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.rotationUniform;
        }

        /// <summary>
        /// ランダムな回転を返します（読み取り専用）
        /// </summary>
        public static Quaternion rotation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.rotation;
        }

        /// <summary>
        /// 半径1.0の球体の表面上のランダムな点を返します（読み取り専用）
        /// </summary>
        public static Vector3 onUnitSphere
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.onUnitSphere;
        }

        /// <summary>
        /// 乱数生成器の完全な内部状態を取得または設定します（読み取り専用）
        /// </summary>
        public static Random.State state
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.state;
        }

        /// <summary>
        /// [0.0～1.0]の範囲のランダムな浮動小数点数を返します（読み取り専用）
        /// </summary>
        public static float value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.value;
        }

        /// <summary>
        /// 半径1.0の球体の内側または上にあるランダムな点を返します（読み取り専用）
        /// </summary>
        public static Vector3 insideUnitSphere
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Random.insideUnitSphere;
        }

        /// <summary>
        /// HSVとアルファの範囲からランダムな色を生成します。
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation [0..1].</param>
        /// <param name="valueMin">Minimum value [0..1].</param>
        /// <param name="valueMax">Maximum value [0..1].</param>
        /// <param name="alphaMin">Minimum alpha [0..1].</param>
        /// <param name="alphaMax">Maximum alpha [0..1].</param>
        /// <returns>
        /// HSV値とアルファ値を（含む）入力範囲に持つ，ランダムな色．値の値を線形補間することで、各成分の値を導き出します。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
        {
            return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
        }

        /// <summary>
        /// HSVとアルファの範囲からランダムな色を生成します。
        /// </summary>
        /// <returns>
        /// HSV値とアルファ値を（含む）入力範囲に持つ，ランダムな色．値の値を線形補間することで、各成分の値を導き出します。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ColorHSV() => Random.ColorHSV();

        /// <summary>
        /// HSVとアルファの範囲からランダムな色を生成します。
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <returns>
        /// HSV値とアルファ値を（含む）入力範囲に持つ，ランダムな色．値の値を線形補間することで、各成分の値を導き出します。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ColorHSV(float hueMin, float hueMax) => Random.ColorHSV(hueMin, hueMax);

        /// <summary>
        /// HSVとアルファの範囲からランダムな色を生成します。
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation [0..1].</param>
        /// <returns>
        /// HSV値とアルファ値を（含む）入力範囲に持つ，ランダムな色．値の値を線形補間することで、各成分の値を導き出します。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
        {
            return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax);
        }

        /// <summary>
        /// HSVとアルファの範囲からランダムな色を生成します。
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation [0..1].</param>
        /// <param name="valueMin">Minimum value [0..1].</param>
        /// <param name="valueMax">Maximum value [0..1].</param>
        /// <param name="alphaMin">Minimum alpha [0..1].</param>
        /// <param name="alphaMax">Maximum alpha [0..1].</param>
        /// <returns>
        /// HSV値とアルファ値を（含む）入力範囲に持つ，ランダムな色．値の値を線形補間することで、各成分の値を導き出します。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
        {
            return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
        }

        //
        // 概要:
        //     Initializes the random number generator state with a seed.
        //
        // パラメーター:
        //   seed:
        //     Seed used to initialize the random number generator.
        /// <summary>
        /// 乱数生成器の状態をシードで初期化します。
        /// </summary>
        /// <param name="seed">乱数生成器を初期化するためのシード。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitState(int seed) => Random.InitState(seed);

        /// <summary>
        /// min ～ max 以内のランダムな int を返します。
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値(この値は含まない)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(int minInclusive, int maxExclusive) => Random.Range(minInclusive, maxExclusive);

        public static float Range(RangeF range) => Random.Range(range.Min, range.Max);
        public static int Range(RangeI range) => Random.Range(range.Min, range.Max);

        /// <summary>
        /// min ～ max 以内のランダムな int を返します。
        /// 50%の確率でプラス、もう半分の確率でマイナスの符号が設定されます。
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値(この値は含まない)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range2Inv(int minInclusive, int maxExclusive)
        {
            int value = Random.Range(minInclusive, maxExclusive);
            return RangeBool() ? value : -value;
        }

        /// <summary>
        /// min ～ max 以内のランダムな float を返します。
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値(この値を含む)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(float minInclusive, float maxInclusive) => Random.Range(minInclusive, maxInclusive);

        /// <summary>
        /// min ～ max 以内のランダムな int を返します。
        /// 50%の確率でプラス、もう半分の確率でマイナスの符号が設定されます。
        /// </summary>
        /// <param name="minInclusive">最小値</param>
        /// <param name="maxExclusive">最大値(この値は含まない)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RangeSigned(float minInclusive, float maxInclusive)
        {
            float value = Random.Range(minInclusive, maxInclusive);
            return RangeBool() ? value : -value;
        }

        /// <summary>
        /// true / false をランダムに取得します。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RangeBool() => Random.value > 0.5f;

        #endregion

        // 
        // Utility系の処理
        // 
        #region...

        /// <summary>
        /// リストの中身を重複せずにランダムに取り出します。
        /// </summary>
        /// <remarks>
        /// 元のリストは変更されません。
        /// </remarks>
        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> list)
        {
            var tempList = new List<T>(list); // 入力をリストにコピーする
            //var r = new System.Random(); // 値を取り出すときに乱数を使用する

            while (tempList.Count != 0)
            {
                //int index = r.Next(0, tempList.Count);
                int index = UniRandom.Range(0, tempList.Count);

                T value = tempList[index];
                tempList.RemoveAt(index);

                yield return value;
            }
        }

        /// <summary>
        /// リストの中身を重複せずにランダムに取り出します。
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(T[] array) => Shuffle(array.ToList());

        /// <summary>
        /// 指定した確率で抽選します。例えば60%(=0.6)を指定したらその確率でtrueが返ります。
        /// true: 当たり / false: はずれ
        /// </summary>
        public static bool Lottery(float percent) => Random.value < percent;

        /// <summary>
        /// 0～最大値までの乱数を取得します。
        /// </summary>
        public static int MaxRand() => Random.Range(0, int.MaxValue);

        // State系の処理 >>>

        /// <summary>
        /// 指定した整数から<see cref="Random.State"/> を取得します。
        /// </summary>
        public static Random.State GetState(int seed)
        {
            var prevState = Random.state;
            Random.InitState(seed);
            Random.State created = Random.state;
            Random.state = prevState;
            return created;
        }

        /// <summary>
        /// 指定した整数から<see cref="Random.State"/> を取得します。
        /// </summary>
        /// <remarks>
        /// 結構重たいみたいなので毎回呼び出すとかはやめた方がいい。
        /// </remarks>
        public static Random.State GetState()
        {
            var prevState = Random.state;
            Random.InitState((int)System.DateTime.Now.Ticks);
            Random.State created = Random.state;
            Random.state = prevState;
            return created;
        }

        /// <summary>
        /// 指定した Seed で乱数生成します。
        /// </summary>
        public static float Range(float min, float max, ref Random.State state)
        {
            Random.State prevState = Random.state;
            try
            {
                Random.state = state;
                float value = Random.Range(min, max);
                state = Random.state;
                return value;
            }
            finally
            {
                Random.state = prevState;
            }
        }

        // State系の処理 <<<

        #endregion
    }
}