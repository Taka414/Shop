//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Samples
{
    /// <summary>
    /// ホーミングする弾のサンプル実装です。
    /// </summary>
    public class HomingBullet : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] float _limit = 1.0f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 移動速度
        Vector3 _velocity;
        // 自分の位置
        Vector3 _position;
        // 追尾目標
        Transform _targetTransform;
        // 着弾までの時間
        float _period;

        //
        // Props & Events
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        public void Update()
        {
            // 追尾対象に向かって位置を更新する
            var acceleration = Vector3.zero;
            var diff = _targetTransform.position - _position;
            acceleration += (diff - _velocity * _period) * 2f / (_period * _period);
            if (acceleration.magnitude > _limit)
            {
                acceleration = acceleration.normalized * _limit;
            }

            _period -= Time.deltaTime;
            if (_period < -3f)
            {
                Destroy(gameObject);
            }

            // 加速して位置の移動
            _velocity += acceleration * Time.deltaTime;
            _position += _velocity * Time.deltaTime;
            transform.position = _position;
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した対象にホーミングを開始します。
        /// </summary>
        public void Shoot(Transform target, Vector3 velocity, float period)
        {
            enabled = true;
            _targetTransform = target;
            _velocity = velocity;
            _period = period;
            _position = transform.position;
        }
    }
}
