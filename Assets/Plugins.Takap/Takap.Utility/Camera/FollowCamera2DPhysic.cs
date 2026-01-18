//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// カメラが指定したオブジェクトに少し遅れて追従する動作を行うコンポーネント
    /// （物理演算で動かしているものを追尾する用）
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class FollowCamera2DPhysic : MonoBehaviour
    {
        //
        // 参考
        // - - - - - - - - - - - - - - - - - - - -
        #region...
        //
        // 少し遅れて追従してくる2Dカメラ:
        // http://baba-s.hatenablog.com/entry/2017/11/30/113517
        // 
        // Vector3.SmoothDampの使い方:
        // https://tech.pjin.jp/blog/2016/12/14/unity_minor_1/
        //
        #endregion

        //
        // 使い方:
        //
        // (1) このコンポーネントをカメラにアタッチする
        // (2) インスペクターから追従したいGameObjectを選択する
        // 

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 目標に到達するまでの遅延時間(sec)
        [SerializeField] private float _delayTime = 0.2f;
        // このカメラが追従するオブジェクト
        [SerializeField] private Transform _target;

        //
        // 説明:
        //
        // Target が画面中央から offsetEnableDistance 以上離れた場合カメラ追従を開始して
        // offsetDisableDistance を下回ったらカメラ追従を停止する
        //
        // --->

        // このコンポーネントの動作が開始されるオフセットを指定するかどうかのフラグ
        // true : 指定する / false : 指定しない
        [SerializeField] private bool _isEnableOffset;
        // 'isEnableOffset' が有効の場合、カメラ追従を停止する距離を指定する(World座標で指定する)
        [SerializeField] private float _minDistance;
        // 'isEnableOffset' が有効の場合、カメラ追従を開始する距離を指定する(World座標で指定する)
        [SerializeField] private float _maxDistance;

        // <---

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // SmoothDampメソッドが使用する速度
        private Vector3 _velocity;
        // アクセス予定のオブジェクトのキャッシュ
        private Camera _myCamera;
        // 追従対象
        private Transform _myTransform;
        // カメラの遅延追従の有効・無効を表すフラグ
        // true : 有効 / false : 無効
        private bool _isEnabled;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        ///// <summary>
        ///// カメラが追従するオブジェクトを設定または取得します。
        ///// nullを指定すると追従しなくなります（但し挙動を無効にしたいときは enable = false が better）
        ///// </summary>
        //public Transform Target { get => _target; set => _target = value; }

        /// <summary>
        /// カメラが <see cref="Target"/> に到着するまでの時間を設定または取得します。
        /// </summary>
        public float DelayTime { get => _delayTime; set => _delayTime = value; }

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _myTransform);
            this.SetComponent(ref _myCamera);
        }

        private void FixedUpdate()
        {
            if (!_target)
            {
                return;
            }

            Vector3 selfPosition = _myTransform.position;
            Vector3 targetPosition = _target.position;
            Vector3 point = _myCamera.WorldToViewportPoint(targetPosition);
            Vector3 delta = targetPosition - _myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = selfPosition + delta;
            float distance = Vector2.Distance(delta, Vector2.zero); // 画面中央からの距離

            // Vector3.SmoothDamp(Vector3, Vector3, ref Vector3, flaot, float);
            // args:
            //   [1] : 移動させるオブジェクトの座標
            //   [2] : 目的地の座標
            //   [3] : 事前に初期化した速度
            //   [4] : 目的までの到達時間
            //   [5] : 最高速度 (省略可) 
            //   [6] : この関数が前回実行されてからの経過時間（デフォルトはTime.deltaTime）(省略可)
            // 
            // ** [5]で最高速度を設定した場合、自動算出された移動速度よりも優先される
            //    最高速度を遅く設定してしまうと[4]で設定した到達時間が経過しても目的地に到達しない可能性がある

            if (!_isEnableOffset)
            {
                // 常時追従
                _myTransform.position = Vector3.SmoothDamp(selfPosition, destination, ref _velocity, _delayTime);
            }
            else
            {
                // オフセット追従
                if (_isEnabled)
                {
                    _myTransform.position = Vector3.SmoothDamp(selfPosition, destination, ref _velocity, DelayTime);

                    if (distance < _minDistance) // 十分近寄ったら自動移動をOFFる
                    {
                        _isEnabled = false;
                    }
                }
                else
                {
                    if (distance > _maxDistance) // ある程度離れたら有効化する
                    {
                        _isEnabled = true;
                    }
                }
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        ///// <summary>
        ///// このオブジェクトを初期化します。
        ///// </summary>
        //public void Init()
        //{
        //    this.SetComponent(ref _myTransform);
        //    this.SetComponent(ref _myCamera);
        //}

        /// <summary>
        /// 追従対象のオブジェクトを設定します。
        /// </summary>
        public void SetTarget(Component target)
        {
            if (!target)
            {
                Log.Warn("target is null");
                return;
            }
            _target = target.transform;
        }

        public void ClearTarget()
        {
            _target = null;
        }

        /// <summary>
        /// アニメーションを使用せずに現在設定しているターゲットを画面の中心に表示します。
        /// </summary>
        public void AdjustTarget()
        {
            _myTransform.SetPos((Vector2)_target.transform.position);
        }

        /// <summary>
        /// 追従を終了します。
        /// </summary>
        public void Exit()
        {
            _target = null;
        }
    }
}