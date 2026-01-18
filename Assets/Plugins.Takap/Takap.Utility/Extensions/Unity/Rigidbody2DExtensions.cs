//
// (C) 2023 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Rigidbody2D"/> を拡張します。
    /// </summary>
    public static class Rigidbody2DExtensions
    {
        /// <summary>
        /// 指定したオブジェクトに爆発する力を加えます。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="explosionForce">爆発の強さ</param>
        /// <param name="explosionPosition">爆発の中心点</param>
        /// <param name="explosionRadius">爆発の半径</param>
        /// <param name="upwardsModifier">追加の上向きの力</param>
        /// <param name="mode">ForceMode2D</param>
        public static void AddExplosionForce(this Rigidbody2D self, float explosionForce, in Vector3 explosionPosition, float explosionRadius, float upwardsModifier = 0, ForceMode2D mode = ForceMode2D.Force)
        {
            Vector2 explosionDirection = (Vector2)self.transform.position - (Vector2)explosionPosition;
            float magnitude = explosionDirection.magnitude;

            explosionDirection.Normalize();
            explosionDirection.y += upwardsModifier;

            float force = Mathf.Clamp01(1 - (magnitude / explosionRadius)) * explosionForce;

            switch (mode)
            {
                case ForceMode2D.Force: 
                    self.AddForce(explosionDirection * force, mode); break;
                case ForceMode2D.Impulse: 
                    self.AddForce(explosionDirection * force, mode); break;
            }
        }
        public static void AddExplosionForce(this Rigidbody2D self, float explosionForce, in Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
        {
            AddExplosionForce(self, explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode2D.Force);
        }
        public static void AddExplosionForce(this Rigidbody2D self, float explosionForce, in Vector3 explosionPosition, float explosionRadius)
        {
            AddExplosionForce(self, explosionForce, explosionPosition, explosionRadius, 0);
        }
    }
}
