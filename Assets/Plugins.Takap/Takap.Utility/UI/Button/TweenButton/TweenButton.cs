//
// (C) 2022 Takap.
//

using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap.Utility
{

    /// <summary>
    /// Tweenアニメーションのシンプルなボタンを表します。
    /// </summary>
    public class TweenButton : ButtonBase
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        const string grp1 = "Setting";
        [SerializeField, BoxGroup(grp1)] Color _colorEnable = new Color(1, 1, 1, 1);
        [SerializeField, BoxGroup(grp1)] Color _colorDisable = new Color(1, 1, 1, 1);
        [SerializeField, BoxGroup(grp1)] Image _targetImage;

        // ScriptableObjectからアニメーションパラメーターをとるかどうかのフラグ
        // true: ScriptableObjectから取得 / false: フィールドから取得
        [SerializeField] bool _useAsset;

        const string grp2 = "Animation";
        // 押したときのアニメーションの速度
        [SerializeField, BoxGroup(grp2), HideIf(nameof(_useAsset))] float _pressDuration = 0.5f;
        // 押したときの大き
        [SerializeField, BoxGroup(grp2), HideIf(nameof(_useAsset))] float _pressScale = 0.8f;
        // 押したときのアニメーションのイージング
        [SerializeField, BoxGroup(grp2), HideIf(nameof(_useAsset))] Ease _pressEase;
        // 離したときのアニメーションの速度
        [SerializeField, BoxGroup(grp2), HideIf(nameof(_useAsset))] float _releaseDuration = 0.5f;
        // 離したときのアニメーションのイージング
        [SerializeField, BoxGroup(grp2), HideIf(nameof(_useAsset))] Ease _releaseEase;
        // ScriptableObjectのパラメーター設定
        [SerializeField, InlineEditor, ShowIf(nameof(_useAsset))] TweenButtonParam _param;

        private float PressDuration => _useAsset ? _param.PressDuration : _pressDuration;
        private float PressScale => _useAsset ? _param.PressScale : _pressScale;
        private Ease PressEase => _useAsset ? _param.PressEase : _pressEase;
        private float ReleaseDuration => _useAsset ? _param.ReleaseDuration : _releaseDuration;
        private Ease ReleaseEase => _useAsset ? _param.ReleaseEase : _releaseEase;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        float _initImageScaleX;
        float _initImageScaleY;

        Tween _tween;

        //
        // Unity impl
        // - - - - - - - - - - - - - - - - - - - -

        protected override void Awake()
        {
            if (_targetImage == null)
            {
                this.SetComponent(ref _targetImage);
            }
            _initImageScaleX = _targetImage.transform.GetLocalScaleX();
            _initImageScaleY = _targetImage.transform.GetLocalScaleY();
        }

        //
        // BaseClass impl
        // - - - - - - - - - - - - - - - - - - - -

        protected override void PointerDown(PointerEventData e)
        {
            PressAnim();
        }

        protected override void PointerUp(PointerEventData e)
        {
            ReleaseAnim();
        }

        protected override void PointerEnter(PointerEventData e)
        {
            PressAnim();
        }

        protected override void PointerExit(PointerEventData e)
        {
            ReleaseAnim();
        }

        protected override void OnChangeButtonInteractable(bool interactable)
        {
            if (_targetImage == null)
            {
                return;
            }

            _targetImage.color = interactable ? _colorEnable : _colorDisable;
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        // 押したときのアニメーション
        private void PressAnim()
        {
            _tween.Kill();
            _tween = DOScale(_initImageScaleX * PressScale, _initImageScaleY * PressScale, PressDuration, PressEase).SetEase(PressEase);
        }

        // 離したときのアニメーション
        private void ReleaseAnim()
        {
            _tween.Kill();
            _tween = DOScale(_initImageScaleX, _initImageScaleY, ReleaseDuration, ReleaseEase).SetEase(ReleaseEase);
        }

        // 共通の拡大率変更アニメーションXY
        private Tween DOScale(float scaleX, float scaleY, float duration, Ease ease)
        {
            return DOTween.Sequence().
                Append(_targetImage.rectTransform.DOScaleX(scaleX, duration).SetEase(ease)).
                Join(_targetImage.rectTransform.DOScaleY(scaleY, duration).SetEase(ease));
        }
    }
}
