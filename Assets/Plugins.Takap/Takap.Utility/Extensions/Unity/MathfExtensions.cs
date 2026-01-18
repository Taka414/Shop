//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Mathf"/> の機能を拡張して基本型そのものを操作対象にします。
    /// </summary>
    public static class MathfExtensions
    {
        public static bool Approximately(this float self, float target) => Mathf.Approximately(self, target);
        public static void Clamp01Self(this ref float self) => self = Mathf.Clamp01(self);

        public static float Abs(this float self) => Mathf.Abs(self);
        public static int Abs(this int self) => Mathf.Abs(self);

        public static float Atan(this float self) => Mathf.Atan(self);
        public static void AtanSelf(this ref float self) => self = Mathf.Atan(self);

        public static float Sin(this float self) => Mathf.Sin(self);
        public static void SinSelf(this ref float self) => self = Mathf.Sin(self);

        public static float Cos(this float self) => Mathf.Cos(self);
        public static void CosSelf(this ref float self) => self = Mathf.Cos(self);

        public static float Tan(this float self) => Mathf.Tan(self);
        public static void TanSelf(this ref float self) => self = Mathf.Tan(self);

        public static float Sqrt(this float self) => Mathf.Sqrt(self);
        public static void SqrtSelf(this ref float self) => self = Mathf.Sqrt(self);

        public static float Pow(this float self, float p) => Mathf.Pow(self, p);
        public static void PowSelf(this ref float self, float p) => self = Mathf.Pow(self, p);

        public static float Round(this float self) => Mathf.Round(self);
        public static void Round(this ref float self) => self = Mathf.Round(self);
        public static int RoundToInt(this float self) => Mathf.RoundToInt(self);

        public static float Floor(this float self) => Mathf.Floor(self);
        public static void FloorSelf(this ref float self) => self = Mathf.Floor(self);
        public static int FloorToInt(this float self) => Mathf.FloorToInt(self);

        public static float Ceil(this float self) => Mathf.Ceil(self);
        public static void CeilSelf(this ref float self) => self = Mathf.Ceil(self);
        public static int CeilToInt(this float self) => Mathf.CeilToInt(self);

        public static float Log(this float self) => Mathf.Log(self);
        public static void LogSelf(this ref float self) => self = Mathf.Log(self);
        public static float Log(this float self, float p) => Mathf.Log(self, p);
        public static void LogSelf(this ref float self, float p) => self = Mathf.Log(self, p);
        public static float Log10(this float self) => Mathf.Log10(self);
        public static void Log10Self(this ref float self) => self = Mathf.Log10(self);
    }
}
