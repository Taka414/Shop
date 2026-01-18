//
// (C) 2024 Takap.
//

namespace Takap.Utility
{
    // 
    // イージングを自分で計算するときに使う
    // 

    /// <summary>
    /// イージングの計算を行うユーティリティクラス
    /// </summary>
    public static class Ease2Utility
    {
        /// <summary>
        /// 経過時間に応じたイージングの値を計算します。
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float Evaluate(float elapsed, float duration, Ease2 ease)
        {
            if (elapsed > duration)
            {
                elapsed = duration;
            }
            float per = elapsed / duration;
            return Evaluate(per, ease);
        }

        /// <summary>
        /// イージングの値を計算します。
        /// </summary>
        /// <param name="per">進行度: 0～1.0まで</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float Evaluate(float per, Ease2 ease)
        {
            return ease switch
            {
                Ease2.Linear => Linear(per),
                Ease2.InSine => InSine(per),
                Ease2.OutSine => OutSine(per),
                Ease2.InOutSine => InOutSine(per),
                Ease2.InQuad => InQuad(per),
                Ease2.OutQuad => OutQuad(per),
                Ease2.InOutQuad => InOutQuad(per),
                Ease2.InCubic => InCubic(per),
                Ease2.OutCubic => OutCubic(per),
                Ease2.InOutCubic => InOutCubic(per),
                Ease2.InQuart => InQuart(per),
                Ease2.OutQuart => OutQuart(per),
                Ease2.InOutQuart => InOutQuart(per),
                Ease2.InQuint => InQuint(per),
                Ease2.OutQuint => OutQuint(per),
                Ease2.InOutQuint => InOutQuint(per),
                Ease2.InExpo => InExpo(per),
                Ease2.OutExpo => OutExpo(per),
                Ease2.InOutExpo => InOutExpo(per),
                Ease2.InCirc => InCirc(per),
                Ease2.OutCirc => OutCirc(per),
                Ease2.InOutCirc => InOutCirc(per),
                Ease2.InElastic => InElastic(per),
                Ease2.OutElastic => OutElastic(per),
                Ease2.InOutElastic => InOutElastic(per),
                Ease2.InBack => InBack(per),
                Ease2.OutBack => OutBack(per),
                Ease2.InOutBack => InOutBack(per),
                Ease2.InBounce => InBounce(per),
                Ease2.OutBounce => OutBounce(per),
                Ease2.InOutBounce => InOutBounce(per),
                _ => per,
            };
        }

        #region Private Methods

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float Linear(float x) => x;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InSine(float x) => 1f - Cos(x * PI / 2f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutSine(float x) => Sin(x * PI / 2f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutSine(float x) => -(Cos(PI * x) - 1f) / 2f;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InQuad(float x) => x * x;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutQuad(float x) => 1f - (1f - x) * (1f - x);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutQuad(float x) => x < 0.5f ? 2f * x * x : 1f - Pow(-2f * x + 2f, 2f) / 2f;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InCubic(float x) => x * x * x;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutCubic(float x) => 1f - Pow(1f - x, 3f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutCubic(float x) => x < 0.5f ? 4f * x * x * x : 1f - Pow(-2f * x + 2f, 3f) / 2f;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InQuart(float x) => x * x * x * x;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutQuart(float x) => 1f - Pow(1f - x, 4f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutQuart(float x) => x < 0.5f ? 8f * x * x * x * x : 1f - Pow(-2f * x + 2f, 4f) / 2f;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InQuint(float x) => x * x * x * x * x;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutQuint(float x) => 1f - Pow(1f - x, 5f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutQuint(float x) => x < 0.5f ? 16f * x * x * x * x * x : 1f - Pow(-2f * x + 2f, 5f) / 2f;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InExpo(float x) => x == 0f ? 0f : Pow(2f, 10f * x - 10f);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutExpo(float x) => x == 1f ? 1f : 1f - Pow(2f, -10f * x);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutExpo(float x)
        {
            return x == 0f ? 0f :
                x == 1f ? 1f :
                x < 0.5f ? Pow(2f, 20f * x - 10f) / 2f :
                (2f - Pow(2f, -20f * x + 10f)) / 2f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InCirc(float x) => 1f - Sqrt(1f - Pow(x, 2f));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutCirc(float x) => Sqrt(1f - Pow(x - 1f, 2f));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutCirc(float x)
        {
            return x < 0.5f ?
                (1f - Sqrt(1f - Pow(2f * x, 2f))) / 2f :
                (Sqrt(1f - Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return c3 * x * x * x - c1 * x * x;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return 1f + c3 * Pow(x - 1f, 3f) + c1 * Pow(x - 1f, 2f);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return x < 0.5f
                ? Pow(2f * x, 2f) * ((c2 + 1f) * 2f * x - c2) / 2f
                : (Pow(2f * x - 2f, 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) / 2f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InElastic(float x)
        {
            const float c4 = 2f * PI / 3f;

            return x == 0f ? 0f :
                x == 1f ? 1f :
                -Pow(2f, 10f * x - 10f) * Sin((x * 10f - 10.75f) * c4);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutElastic(float x)
        {
            const float c4 = 2f * PI / 3f;

            return x == 0f ? 0f :
                x == 1f ? 1f :
                Pow(2f, -10f * x) * Sin((x * 10f - 0.75f) * c4) + 1f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutElastic(float x)
        {
            const float c5 = 2f * PI / 4.5f;

            return x == 0f ? 0f :
                x == 1f ? 1f :
                x < 0.5f ?
                -(Pow(2f, 20f * x - 10f) * Sin((20f * x - 11.125f) * c5)) / 2f :
                Pow(2f, -20f * x + 10f) * Sin((20f * x - 11.125f) * c5) / 2f + 1f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InBounce(float x) => 1f - OutBounce(1f - x);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float OutBounce(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            float t = x;

            if (t < 1f / d1)
            {
                return n1 * t * t;
            }
            else if (t < 2f / d1)
            {
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            }
            else if (t < 2.5f / d1)
            {
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            }
            else
            {
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float InOutBounce(float x)
        {
            return x < 0.5f ?
                (1f - OutBounce(1f - 2f * x)) / 2f :
                (1f + OutBounce(2f * x - 1f)) / 2f;
        }

        #endregion

        #region MathF Copied

        public const float PI = System.MathF.PI;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float Cos(float x) => (float)System.Math.Cos(x);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float Pow(float x, float y) => (float)System.Math.Pow(x, y);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float Sin(float x) => (float)System.Math.Sin(x);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static float Sqrt(float x) => (float)System.Math.Sqrt(x);

        #endregion
    }

    /// <summary>
    /// イージングの種類を表します。
    /// </summary>
    public enum Ease2
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
        CustomAnimationCurve
    }
}
