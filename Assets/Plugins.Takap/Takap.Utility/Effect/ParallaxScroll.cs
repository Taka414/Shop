//
// (C) 2022 Takap.
//

using System;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 視差スクロールを行うためのクラスです。
    /// </summary>
    public class ParallaxScroll : MonoBehaviour
    {
        //
        // 使い方:
        // (1) 任意のゲームオブジェクトにこのオブジェクトを追加
        // (2) レイヤーをインスペクターから設定する
        // (3) カメラを動かす
        //
        // 補足:
        // 水平方向のみサポート
        //

        //
        // Inner Tyeps
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 移動対象のオブジェクトを設定する
        /// </summary>
        [Serializable]
        public class BackgroundLayer
        {
            // 移動対象のオブジェクト
            [SerializeField] private Transform[] _objects;
            // 移動速度
            [SerializeField, Range(-1, 1)] private float _speedOverCam;

            public Transform[] Objects => _objects;
            public float SpeedOverCam => _speedOverCam;
        }

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 追従する対象のカメラ
        [SerializeField] private Camera _cam;
        // 制御対象のレイヤーの設定
        [SerializeField] private BackgroundLayer[] _layers;
        // カメラの位置
        private Transform _camTransform;

        //
        // Rintimes
        // - - - - - - - - - - - - - - - - - - - -

        // 直前の位置
        private Vector3 previousPos = Vector3.zero;

        private void Start()
        {
            _camTransform = _cam.transform;
            previousPos = _camTransform.GetPos();
        }

        private void Update()
        {
            Vector3 current = _camTransform.GetPos();
            Vector3 diff = current - previousPos;

            float camSpeed = diff.x;
            previousPos = current;
            Debug.Log(camSpeed);

            if (camSpeed == 0)
            {
                return;
            }

            foreach (BackgroundLayer layer in _layers)
            {
                float speed = camSpeed * layer.SpeedOverCam;

                foreach (Transform trans in layer.Objects)
                {
                    trans.AddLocalPosX(speed);
                }
            }
        }
    }
}