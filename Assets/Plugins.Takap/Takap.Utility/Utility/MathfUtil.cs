//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 2Dの計算をするための機能を提供します。
    /// </summary>
    public static class MathfUtil
    {
        //
        // Constants
        // - - - - - - - - - - - - - - - - - - - -

        public const float Rad90 = 1.0f / 2.0f * Mathf.PI;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 角度をラジアンに変換します。
        /// </summary>
        public static Radian ToRad(in Degree deg) => deg * Mathf.Deg2Rad;

        /// <summary>
        /// ラジアンを角度に変換します。
        /// </summary>
        public static Degree ToDeg(in Radian rad) => rad * Mathf.Rad2Deg;

        /// <summary>
        /// 指定した角度からXとYの組み合わせを取得します。
        /// </summary>
        public static Vector2 GetVector(float deg, float rate = 1.0f) => GetVector(ToRad(deg), rate);

        /// <summary>
        /// 指定した角度からXとYの組み合わせを取得します。
        /// </summary>
        public static Vector2 GetVector(in Degree deg, float rate = 1.0f) => GetVector(ToRad(deg), rate);

        /// <summary>
        /// 指定した角度からXとYの組み合わせを取得します。
        /// </summary>
        public static Vector2 GetVectorDeg(float deg, float rate = 1.0f) => GetVector((Degree)deg, rate);

        /// <summary>
        /// 指定したラジアン角度からベクトルを取得します。
        /// </summary>
        public static Vector2 GetVector(in Radian rad, float rate = 1.0f)
        {
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);

            if (rate == 1.0f)
            {
                return new Vector2(x, y);
            }
            return new Vector2(x * rate, y * rate);
        }

        /// <summary>
        /// 指定したラジアン角度からベクトルを取得します。
        /// </summary>
        public static Vector2 GetVectorRad(float deg, float rate = 1.0f) => GetVector((Radian)deg, rate);

        /// <summary>
        /// 適当なラジアン角を作成してXとYの組み合わせを取得します。
        /// </summary>
        public static (Vector2 vec, Radian rad) GetVectorRandom(float rate = 1.0f)
        {
            float deg = UniRandom.Range(0f, 360.0f);
            float rad = ToRad(deg);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);

            if (rate == 1.0f)
            {
                return (new Vector2(x, y), rad);
            }
            return (new Vector2(x * rate, y * rate), rad);
        }

        /// <summary>
        /// 指定した2地点のラジアンを取得します。
        /// </summary>
        public static Radian GetRad(in Vector3 from, in Vector3 to)
        {
            Vector3 diff = to - from;
            return Mathf.Atan2(diff.y, diff.x);
        }

        /// <summary>
        /// 指定した2地点のラジアンを取得します。
        /// </summary>
        public static Radian GetRad(in Vector2 from, in Vector2 to)
        {
            Vector2 diff = to - from;
            return Mathf.Atan2(diff.y, diff.x);
        }

        /// <summary>
        /// 指定した2地点の角度を取得します。
        /// </summary>
        public static Degree GetDeg(in Vector3 from, in Vector3 to) => ToDeg(GetRad(from, to));

        /// <summary>
        /// 指定した2地点の角度を取得します。
        /// </summary>
        public static Degree GetDeg(in Vector2 from, in Vector2 to) => ToDeg(GetRad(from, to));

        /// <summary>
        /// 指定した2地点の角度を取得します。
        /// </summary>
        public static Degree GetDeg(RectTransform from, in Vector2 to) => ToDeg(GetRad((Vector2)from.GetLocalPos(), to));

        /// <summary>
        /// 新しい地点を取得します。(0, 0)原点
        /// </summary>
        public static Vector2 GetPostion2D(in Radian rad, float distance)
        {
            float x = Mathf.Cos(rad) * distance;
            float y = Mathf.Sin(rad) * distance;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 指定した地点から角度と距離分離れた新しい位置を計算します。
        /// </summary>
        public static Vector3 GetPostion2D(in Vector3 pos, in Radian rad, float distance)
        {
            float x = Mathf.Cos(rad) * distance + pos.x;
            float y = Mathf.Sin(rad) * distance + pos.y;
            return new Vector3(x, y);
        }

        //
        // 最後の部分:
        // https://nekosuko.jp/blog/267
        // 
        // 嘘が書いてあるので正しくはこっち
        // https://tyfkda.github.io/blog/2015/07/15/unity-world-to-canvas.html
        //

        ///// <summary>
        ///// スクリーン座標を <see cref="Canvas"/> 内の座標に変換します。
        ///// </summary>
        //public static Vector2 GetPosInCanvas(Canvas target, in Vector3 wpos)
        //{
        //    // 使ってるカメラの WorldToViewportPoint で、ワールド座標系からビューポート座標系に変換
        //    RectTransform rect = target.GetComponent<RectTransform>();

        //    // Canvas の RectTransform のサイズの1/2を引く（ビューポートは左下が原点なのに対して、 Canvas は中央が原点のため）
        //    Vector3 vpos = Camera.main.WorldToViewportPoint(wpos);
        //    Vector2 sd = rect.sizeDelta;

        //    return new Vector2()
        //    {
        //        x = (vpos.x * sd.x) - (sd.x * 0.5f),
        //        y = (vpos.y * sd.y) - (sd.y * 0.5f)
        //    };
        //}

        // --- Obsolete ---

        ///// <summary>
        ///// 真上が0度になるように調整した右回転の角度を取得します。
        ///// </summary>
        //public static float GetDeg360(Vector3 begin, Vector3 end)
        //{
        //    Vector3 dt = end - begin;
        //    float rad = Mathf.Atan2(-dt.x, dt.y);
        //    float deg = rad * Mathf.Rad2Deg;
        //
        //    if (deg < 0.0f)
        //    {
        //        deg += 360.0f;
        //    }
        //
        //    return deg;
        //}
    }
}