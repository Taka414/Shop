//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Takap.Utility;
using UnityEngine;

using UnityEngine.AI;

namespace Takap
{
    /// <summary>
    /// プレイヤーを追尾するダミーの敵キャラクターを表します。
    /// </summary>
    /// <remarks>
    /// NavMeshを使った移動方法のリファレンス実装
    /// </remarks>
    public class NavMeshSample : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 追尾対象
        [SerializeField] Transform _target;
        // 移動速度
        [SerializeField] float _moveSpeed = 1f;
        // どれくらい近づいたら次のポイントに移るか
        [SerializeField] float _minDistance = 0.05f;
        // プレイヤーへの経路再計算をする間隔
        [SerializeField] float _reCalcTime = 0.5f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 次の移動先
        private Vector2 _nextPoint;

        // キャッシュ類
        private Transform _plyaerTransform;
        private Transform _myTransform;

        // AI用
        private NavMeshPath _navMeshPath;
        private Queue<Vector3> _navMeshCorners = new();

        // 計算したときのプレイヤーの位置
        Vector3 _calcedPlayerPos;
        // 次に再計算するまでの時間
        private float _elapsed;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        public void Awake()
        {
            _myTransform = transform;
            _plyaerTransform = _target.transform;
            _nextPoint = _myTransform.position;
            _navMeshPath = new NavMeshPath();
        }

        public void Update()
        {
            if (_calcedPlayerPos != _plyaerTransform.localPosition)
            {
                _elapsed += Time.deltaTime;
                if (_elapsed > _reCalcTime)
                {
                    _elapsed = 0;

                    UpdateRoute();
                    _calcedPlayerPos = _plyaerTransform.localPosition; // ルート出したときの位置
                    //Log.Trace("Route update", this);
                }
            }

            // 目的地近辺に移動したら次の目的地に変更する
            Vector2 currentPos = _myTransform.localPosition;
            if (Vector2.Distance(_nextPoint, currentPos) < _minDistance)
            {
                NextStep();
            }

            Vector2 diff = _nextPoint - currentPos;
            if (diff == Vector2.zero)
            {
                return;
            }

            Vector2 step = _moveSpeed * Time.deltaTime * diff.normalized;
            _myTransform.Translate(step);
        }

        private void UpdateRoute()
        {
            // NavMeshで経路を計算する
            // 自分の位置 → プレイヤーの位置
            bool isOk = NavMesh.CalculatePath(_myTransform.position, _plyaerTransform.position, NavMesh.AllAreas, _navMeshPath);
            if (!isOk)
            {
                Log.Warn("Failed to NavMesh.CalculatePath.", this);
            }

            _navMeshCorners.Clear();
            _navMeshCorners.EnqueueRange(_navMeshPath.corners);
            _nextPoint = _myTransform.localPosition;
        }

        private void NextStep()
        {
            if (_navMeshCorners.Count == 0)
            {
                _nextPoint = _myTransform.localPosition;
                return;
            }
            _nextPoint = _navMeshCorners.Dequeue();
        }
    }
}