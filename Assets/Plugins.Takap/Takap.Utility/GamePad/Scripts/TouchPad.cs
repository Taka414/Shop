//
// (C) 2022 Takap.
//

using System;
using DG.Tweening;
using Takap.Utility;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Takap
{
    /// <summary>
    /// タッチパッドを表します。
    /// </summary>
    internal class TouchPad : UIMonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 移動開始までのマージン
        [SerializeField] float _marginDistanceToMove = 20f;
        // 中心からの最大距離
        [SerializeField] float _maxDistance = 80f;

        //
        // Events
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// パッド操作が行われている事を通知します。
        /// </summary>
        public Observable<PadAction> PadActionPerFrame => _padActionPerFrame;
        private readonly Subject<PadAction> _padActionPerFrame = new();

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 見た目の制御
        TouchPadVisual001 _visual1;
        TouchPadVisual002 _visual2;

        // タッチ開始位置(ローカル座標)
        Vector2 _startPos;
        // 現在位置(ローカル座標)
        Vector2 _currentPos;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void OnValidate()
        {
            if (_visual2 != null)
            {
                _visual2.SetMaxDistance(_maxDistance);
            }
        }

        private void Awake()
        {
            SetupVisual1();
            SetupVisual2();
            enabled = false;
        }

        private void SetupVisual1()
        {
            GameObject go = this.GetChild("001");
            CanvasGroup group = go.GetComponent<CanvasGroup>();
            Image imgBase = this.GetChildComponentPath<Image>("001/Base");
            Image neon = this.GetChildComponentPath<Image>("001/Neon");

            _visual1 = new(go, group, imgBase, neon);
            _visual1.Init();
        }

        private void SetupVisual2()
        {
            GameObject go = this.GetChild("002");
            Image image002 = this.GetChildComponentPath<Image>("002");
            RectTransform rect = this.GetChildComponentPath<RectTransform>("002/Rect");
            Image baseImage = this.GetChildComponentPath<Image>("002/Rect/Base");
            Image neon = this.GetChildComponentPath<Image>("002/Rect/Neon");
            Image corner4 = this.GetChildComponentPath<Image>("002/Rect/Corner4");
            Image arrow = this.GetChildComponentPath<Image>("002/Arrow");

            _visual2 = new(go, image002, rect, baseImage, neon, corner4, arrow);
            _visual2.Init();
            _visual2.SetMaxDistance(_maxDistance);
        }

        private void OnDestroy()
        {
            using (_visual1) { }
            using (_visual2) { }
        }

        private void Update()
        {
            _padActionPerFrame.OnNext(new(GetPadTopplingAmount(), PadActionType.Move, GetAngle()));
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// パッド操作を開始します。
        /// </summary>
        public void Begin(in TouchAreaInfo info)
        {
            _startPos = info.LocalPos;
            _currentPos = _startPos;

            RectTransform.SetLocalPos(_startPos);

            _visual1.Show();
            _visual2.Hide();

            enabled = true;

            _padActionPerFrame.OnNext(new(0f, PadActionType.Start, GetAngle()));
        }

        /// <summary>
        /// パッド操作を終了します。
        /// </summary>
        public void End(in TouchAreaInfo _)
        {
            _visual1.Hide();
            _visual2.Hide();

            enabled = false;

            _padActionPerFrame.OnNext(new(0f, PadActionType.End, GetAngle()));
        }

        /// <summary>
        /// パッドを移動します。
        /// </summary>
        public void Move(in TouchAreaInfo info)
        {
            Vector2 currentPos = info.LocalPos;

            float distance = _startPos.Distance(currentPos);
            if (_visual1.Enabled)
            {
                if (distance > _marginDistanceToMove)
                {
                    _visual2.Show();
                    _visual1.Hide();
                }
            }

            _currentPos = currentPos;
            _visual2.Move(_startPos, distance, info);
        }

        /// <summary>
        /// 現在のオブジェクトの内容でパッドの操作量を 0 ～ 1.0 の範囲で取得します。
        /// </summary>
        private float GetPadTopplingAmount()
        {
            float distance = _startPos.Distance(_currentPos);
            return Mathf.InverseLerp(0, _maxDistance, distance);
        }

        /// <summary>
        /// 現在のオブジェクトの内容でパッドの操作角度をラジアンで取得します。
        /// </summary>
        private float GetAngle()
        {
            return _startPos.GetRad(_currentPos);
        }
    }

    /// <summary>
    /// ゲームオブジェクト '001' の見た目を制御するクラス
    /// </summary>
    internal class TouchPadVisual001 : IDisposable
    {
        GameObject _go;
        CanvasGroup _group;
        Image _base;
        Image _neon;

        bool _shown;

        Tween _animGroup;
        Tween _animBase;
        Tween _animNeon;

        public TouchPadVisual001(GameObject parent, CanvasGroup group, Image imgBase, Image neon)
        {
            _go = parent;
            _group = group;
            _base = imgBase;
            _neon = neon;
        }

        public bool Enabled => _go.activeSelf;

        public void Init()
        {
            _group.alpha = 0;
            _neon.SetAlpha(1);
        }

        public void Show()
        {
            if (_shown)
            {
                return;
            }
            _shown = true;

            _animGroup.Kill();
            _animGroup = _group.DOFade(1f, 0.1f);

            _animBase.Kill();
            _animBase =
                DOTween.Sequence()
                    .Append(_base.rectTransform.DOScale(1.2f, 0.1f))
                    .Append(_base.rectTransform.DOScale(1.0f, 0.05f));

            // ゆっくり点滅する
            _neon.SetAlpha(1);
            _animNeon.Kill();
            _animNeon =
                DOTween.Sequence()
                    .Append(_neon.DOFade(0f, 1.5f))
                    .Append(_neon.DOFade(1f, 1.5f)).SetLoops(-1);
        }

        public void Hide()
        {
            if (!_shown)
            {
                return;
            }
            _shown = false;

            _animGroup.Kill();
            _animGroup = _group.DOFade(0f, 0.07f);
        }

        public void Dispose()
        {
            _animGroup.Kill();
            _animBase.Kill();
            _animNeon.Kill();
        }
    }

    /// <summary>
    /// ゲームオブジェクト '002' の見た目を制御するクラス
    /// </summary>
    internal class TouchPadVisual002 : IDisposable
    {
        GameObject _go;
        Image _imageBase;
        Image _img002;
        RectTransform _rect;
        Image _neon;
        Image _corner4;
        Image _arrow;

        // サークルの表示・非表示
        Tween _animRect;
        // 内側の三角形の回転
        Tween _animCorner4;
        // 矢印の非表示
        Tween _animArrow;

        // 中心から矢印が離れられる量
        float _maxDistance;
        // 現在の矢印の中心からの距離
        float _currentDistance;

        public TouchPadVisual002(GameObject parent, Image img002, RectTransform rect, Image imageBase, Image neon, Image corner4, Image arrow)
        {
            _go = parent;
            _img002 = img002;
            _rect = rect;
            _imageBase = imageBase;
            _neon = neon;
            _corner4 = corner4;
            _arrow = arrow;
        }

        public void Init()
        {
            _go.SetActive(false);

            _rect.SetLocalScaleXY(0);
            _arrow.rectTransform.SetLocalScaleXY(0);

            // ずっと回転し続ける
            _animCorner4 = _corner4.transform.DORotate(new Vector3(0, 0, -180), 50.0f).SetLoops(-1);
            _animCorner4.Pause();
        }

        public void SetMaxDistance(float value)
        {
            _maxDistance = value;
        }

        public void Show()
        {
            if (_go.activeSelf)
            {
                return;
            }
            _go.SetActive(true);

            _animCorner4.Play();
            ShowRect();
            ShowArrow();
        }

        public void Hide()
        {
            if (!_go.activeSelf)
            {
                return;
            }

            _animCorner4.Pause();
            HideRect();
            HideArrow();
        }

        public void Move(in Vector2 startPos, float distance, in TouchAreaInfo info)
        {
            Degree deg = startPos.GetDeg(info.LocalPos);
            if (distance > _maxDistance)
            {
                _currentDistance = _maxDistance;
                _arrow.rectTransform.SetLocalPos(MathfUtil.GetVector(deg, _maxDistance));
            }
            else
            {
                _currentDistance = distance;
                _arrow.rectTransform.SetLocalPos(info.ScreenPos.ToLocalPos(_img002));
            }
            _arrow.rectTransform.SetLocalEulerZ(deg - 90f);
            _neon.rectTransform.SetLocalEulerZ(deg - 135f);
        }

        public void Dispose()
        {
            _animRect.Kill();
            _animCorner4.Kill();
            _animArrow.Kill();
        }

        private void ShowRect()
        {
            _animRect.Kill();

            _animRect = DOTween.Sequence()
                .Append(_rect.DOScale(1.2f, 0.12f))
                .Append(_rect.DOScale(1.0f, 0.005f).SetEase(Ease.Linear));
        }

        private void HideRect()
        {
            // サークルの非表示
            _animRect.Kill();
            _animRect = _rect.DOScale(0f, 0.15f).OnComplete(() =>
            {
                _go.SetActive(false);
            });
        }

        private void ShowArrow()
        {
            _animArrow.Kill();
            _animArrow = _arrow.rectTransform.DOScale(1f, 0.1f);
        }

        private void HideArrow()
        {
            // 非表示までの時間は距離に応じて変更する
            float hideArrowDuration = Mathf.InverseLerp(0, _maxDistance, _currentDistance) * 0.1f;

            // 矢印の非表示
            _animArrow.Kill();
            _animArrow =
                DOTween.Sequence()
                .Append(_arrow.rectTransform.DOLocalMove(Vector3.zero, hideArrowDuration))
                .Join(_arrow.rectTransform.DOScale(0f, hideArrowDuration));
        }
    }
}