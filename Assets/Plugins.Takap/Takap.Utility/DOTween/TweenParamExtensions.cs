//
// (C) 2024 Takap.
//

using System;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="TweenParamAsset{T}"/> を拡張します。
    /// </summary>
    public static class TweenParamExtensions
    {
        public static Tween DOMove(this Transform target, TweenVector3 param, bool snapping = false)
        {
            return target.DOMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOMove(this Transform target, TweenVector3Asset param, bool snapping = false)
        {
            return target.DOMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOMove(this Transform target, TweenVector2 param, bool snapping = false)
        {
            return target.DOMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOMove(this Transform target, TweenVector2Asset param, bool snapping = false)
        {
            return target.DOMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOMoveX(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOMoveX(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOMoveX(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOMoveX(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOMoveY(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOMoveY(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOMoveY(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOMoveY(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOMoveZ(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOMoveZ(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOMoveZ(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOMoveZ(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOLocalMove(this Transform target, TweenVector3 param, bool snapping = false)
        {
            return target.DOLocalMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOLocalMove(this Transform target, TweenVector3Asset param, bool snapping = false)
        {
            return target.DOLocalMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOLocalMove(this Transform target, TweenVector2 param, bool snapping = false)
        {
            return target.DOLocalMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOLocalMove(this Transform target, TweenVector2Asset param, bool snapping = false)
        {
            return target.DOLocalMove(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOLocalMoveX(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOLocalMoveX(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOLocalMoveX(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOLocalMoveX(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOLocalMoveY(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOLocalMoveY(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOLocalMoveY(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOLocalMoveY(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DOLocalMoveZ(this Transform target, TweenFloat param, bool snapping = false)
        {
            return target.DOLocalMoveZ(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        public static Tween DOLocalMoveZ(this Transform target, TweenFloatAsset param, bool snapping = false)
        {
            return target.DOLocalMoveZ(param.EndValue, param.Duration, snapping).AddOptions(param);
        }
        
        public static Tween DORotate(this Transform target, TweenVector3 param, RotateMode mode = RotateMode.Fast)
        {
            return target.DORotate(param.EndValue, param.Duration, mode).AddOptions(param);
        }
        public static Tween DORotate(this Transform target, TweenVector3Asset param, RotateMode mode = RotateMode.Fast)
        {
            return target.DORotate(param.EndValue, param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DORotateX(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            //float x = eulerAngles.x;
            float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DORotate(new Vector3(param.EndValue, y, z), param.Duration, mode).AddOptions(param);
        }
        public static Tween DORotateX(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            //float x = eulerAngles.x;
            float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DORotate(new Vector3(param.EndValue, y, z), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DORotateY(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            //float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DORotate(new Vector3(x, param.EndValue, z), param.Duration, mode).AddOptions(param);
        }
        public static Tween DORotateY(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            //float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DORotate(new Vector3(x, param.EndValue, z), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DORotateZ(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            float y = eulerAngles.y;
            //float z = eulerAngles.z;
            return target.DORotate(new Vector3(x, y, param.EndValue), param.Duration, mode).AddOptions(param);
        }
        public static Tween DORotateZ(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            float y = eulerAngles.y;
            //float z = eulerAngles.z;
            return target.DORotate(new Vector3(x, y, param.EndValue), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DOLocalRotate(this Transform target, TweenVector3 param, RotateMode mode = RotateMode.Fast)
        {
            return target.DOLocalRotate(param.EndValue, param.Duration, mode).AddOptions(param);
        }
        public static Tween DOLocalRotate(this Transform target, TweenVector3Asset param, RotateMode mode = RotateMode.Fast)
        {
            return target.DOLocalRotate(param.EndValue, param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DOLocalRotateX(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            //float x = eulerAngles.x;
            float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(param.EndValue, y, z), param.Duration, mode).AddOptions(param);
        }
        public static Tween DOLocalRotateX(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            //float x = eulerAngles.x;
            float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(param.EndValue, y, z), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DOLocalRotateY(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            //float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(x, param.EndValue, z), param.Duration, mode).AddOptions(param);
        }
        public static Tween DOLocalRotateY(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            //float y = eulerAngles.y;
            float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(x, param.EndValue, z), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DOLocalRotateZ(this Transform target, TweenFloat param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            float y = eulerAngles.y;
            //float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(x, y, param.EndValue), param.Duration, mode).AddOptions(param);
        }
        public static Tween DOLocalRotateZ(this Transform target, TweenFloatAsset param, RotateMode mode = RotateMode.Fast)
        {
            Vector3 eulerAngles = target.eulerAngles;
            float x = eulerAngles.x;
            float y = eulerAngles.y;
            //float z = eulerAngles.z;
            return target.DOLocalRotate(new Vector3(x, y, param.EndValue), param.Duration, mode).AddOptions(param);
        }
        
        public static Tween DOScale(this Transform target, TweenVector3 param)
        {
            return target.DOScale(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOScale(this Transform target, TweenVector3Asset param)
        {
            return target.DOScale(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOScale(this Transform target, TweenFloat param)
        {
            return target.DOScale(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOScale(this Transform target, TweenFloatAsset param)
        {
            return target.DOScale(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOScaleX(this Transform target, TweenFloat param)
        {
            return target.DOScaleX(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOScaleX(this Transform target, TweenFloatAsset param)
        {
            return target.DOScaleX(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOScaleY(this Transform target, TweenFloat param)
        {
            return target.DOScaleY(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOScaleY(this Transform target, TweenFloatAsset param)
        {
            return target.DOScaleY(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOScaleZ(this Transform target, TweenFloat param)
        {
            return target.DOScaleZ(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOScaleZ(this Transform target, TweenFloatAsset param)
        {
            return target.DOScaleZ(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOColor(this SpriteRenderer target, TweenColor param)
        {
            return target.DOColor(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOColor(this SpriteRenderer target, TweenColorAsset param)
        {
            return target.DOColor(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOColor(this Graphic target, TweenColor param)
        {
            return target.DOColor(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOColor(this Graphic target, TweenColorAsset param)
        {
            return target.DOColor(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOFade(this SpriteRenderer target, TweenFloat param)
        {
            return target.DOFade(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOFade(this SpriteRenderer target, TweenFloatAsset param)
        {
            return target.DOFade(param.EndValue, param.Duration).AddOptions(param);
        }
        
        public static Tween DOFade(this Graphic target, TweenFloat param)
        {
            return target.DOFade(param.EndValue, param.Duration).AddOptions(param);
        }
        public static Tween DOFade(this Graphic target, TweenFloatAsset param)
        {
            return target.DOFade(param.EndValue, param.Duration).AddOptions(param);
        }

        // -x-x- オプションの設定 -x-x-

        public static Tween AddOptions(this Tween tween, TweenVector3 param)
        {
            if (param.UseAnimationCurve)
            {
                if (param.AnimationCurve.keys.Length == 0)
                {
                    throw new System.ArgumentException("AnimationCurve is not set curve.");
                }
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }
            if (param.IsRelative) tween.SetRelative();
            if (param.IsLooping) tween.SetLoops(param.Loops, param.LoopType);
            if (param.IgnoreTimeScale) tween.SetUpdate(true);
            //if (param.UseStartEvent) tween.OnStart(() => param.StartEvent?.Invoke());
            //if (param.UsePlayEvent) tween.OnStart(() => param.PlayEvent?.Invoke());
            //if (param.UseCompleteEvent) tween.OnComplete(() => param.CompleteEvent?.Invoke());
            return tween;
        }
        public static Tween AddOptions(this Tween tween, TweenVector3Asset param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }
            if (param.IsRelative)
            {
                tween.SetRelative();
            }
            if (param.IsLooping)
            {
                tween.SetLoops(param.Loops, param.LoopType);
            }
            if (param.IgnoreTimeScale)
            {
                tween.SetUpdate(true);
            }
            return tween;
        }

        public static Tween AddOptions(this Tween tween, TweenVector2 param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }
            if (param.IsRelative) tween.SetRelative();
            if (param.IsLooping) tween.SetLoops(param.Loops, param.LoopType);
            if (param.IgnoreTimeScale) tween.SetUpdate(true);
            //if (param.UseStartEvent) tween.OnStart(() => param.StartEvent?.Invoke());
            //if (param.UsePlayEvent) tween.OnStart(() => param.PlayEvent?.Invoke());
            //if (param.UseCompleteEvent) tween.OnComplete(() => param.CompleteEvent?.Invoke());
            return tween;
        }
        public static Tween AddOptions(this Tween tween, TweenVector2Asset param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }

            if (param.IsRelative)
            {
                tween.SetRelative();
            }
            if (param.IsLooping)
            {
                tween.SetLoops(param.Loops, param.LoopType);
            }
            if (param.IgnoreTimeScale)
            {
                tween.SetUpdate(true);
            }
            return tween;
        }

        public static Tween AddOptions(this Tween tween, TweenFloat param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }
            if (param.IsRelative) tween.SetRelative();
            if (param.IsLooping) tween.SetLoops(param.Loops, param.LoopType);
            if (param.IgnoreTimeScale) tween.SetUpdate(true);
            //if (param.UseStartEvent) tween.OnStart(() => param.StartEvent?.Invoke());
            //if (param.UsePlayEvent) tween.OnStart(() => param.PlayEvent?.Invoke());
            //if (param.UseCompleteEvent) tween.OnComplete(() => param.CompleteEvent?.Invoke());
            return tween;
        }
        public static Tween AddOptions(this Tween tween, TweenFloatAsset param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }

            if (param.IsRelative)
            {
                tween.SetRelative();
            }
            if (param.IsLooping)
            {
                tween.SetLoops(param.Loops, param.LoopType);
            }
            if (param.IgnoreTimeScale)
            {
                tween.SetUpdate(true);
            }
            return tween;
        }

        public static Tween AddOptions(this Tween tween, TweenColor param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }
            if (param.IsRelative) tween.SetRelative();
            if (param.IsLooping) tween.SetLoops(param.Loops, param.LoopType);
            if (param.IgnoreTimeScale) tween.SetUpdate(true);
            //if (param.UseStartEvent) tween.OnStart(() => param.StartEvent?.Invoke());
            //if (param.UsePlayEvent) tween.OnStart(() => param.PlayEvent?.Invoke());
            //if (param.UseCompleteEvent) tween.OnComplete(() => param.CompleteEvent?.Invoke());
            return tween;
        }
        public static Tween AddOptions(this Tween tween, TweenColorAsset param)
        {
            if (param.UseAnimationCurve)
            {
                tween.SetEase(param.AnimationCurve);
            }
            else
            {
                tween.SetEase(param.Ease);
            }

            if (param.IsRelative)
            {
                tween.SetRelative();
            }
            if (param.IsLooping)
            {
                tween.SetLoops(param.Loops, param.LoopType);
            }
            if (param.IgnoreTimeScale)
            {
                tween.SetUpdate(true);
            }
            return tween;
        }

        /// <summary>
        /// 文字送りをDOTWeenで実行します。
        /// </summary>
        /// <param name="target">文字送りをする対象</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="typingSpeed">1文字ごとの表示速度(秒)</param>
        /// <param name="onUpdate">表示が更新されたときに呼び出されるコールバック</param>
        public static Tween DOFeedText(this TMP_Text target, string text, float typingSpeed, Action<FeedTextArgs> onUpdate = null)
        {
            // - - - - - - - - - - - - - - - - - - - -
            // 補足:
            // 最後の通知を受け取るために以下のように引数を指定すること
            // _anim.Complete(true);
            // - - - - - - - - - - - - - - - - - - - -

            // これ以上インターバルが長ければこの値を使用する値(要調整)
            const float MAX_DELAY = 0.08f;

            target.text = text;
            target.maxVisibleCharacters = 0;
            int tempPosition = 0;

            var seq = DOTween.Sequence().SetLink(target.gameObject);
            seq.Append(DOTween.To(() => tempPosition, value =>
            {
                tempPosition = value;
                if (target.maxVisibleCharacters != value)
                {
                    target.maxVisibleCharacters = value;

                    onUpdate?.Invoke(new FeedTextArgs(target, text, value, false));
                }
            }
            , text.Length
            , text.Length * typingSpeed)
                .SetEase(Ease.Linear));
            seq.AppendInterval(typingSpeed > MAX_DELAY ? MAX_DELAY : typingSpeed); // これより大きい値は使わない
            seq.AppendCallback(() => onUpdate?.Invoke(new FeedTextArgs(target, text, text.Length, true)));
            return seq;
        }

        /// <summary>
        /// オブジェクトをパラメーターに従ってジャンプします。
        /// </summary>
        public static Tween DOJump(this Transform target, JumpAction onLandCallback, params JumpParam[] jumpParams)
        {
            if (jumpParams == null || jumpParams.Length == 0)
            {
                throw new ArgumentException($"{nameof(jumpParams)} is null or empty.");
            }

            // 開始位置
            Vector3 startPos = target.localPosition;
            // 現在のジャンプパラメータ
            JumpParam cuurentParam = jumpParams[0];
            //Log.Trace($"{cuurentParam}");
            // 進行度 0～100% (0～1.0f)
            float per = 0;

            var seq = DOTween.Sequence();
            //.SetLink(target)
            //.SetRelative();

            seq.Append(DOTween.To(() => per
            , value =>
            {
                per = value;
                target.localPosition = startPos + cuurentParam.CalcPos(per);
            }
            , 1
            , cuurentParam.Duration)
                .SetEase(Ease.Linear));

            for (int i = 1; i < jumpParams.Length; i++)
            {
                int pp = i;
                float duration = jumpParams[pp].Duration;
                seq.AppendCallback(() =>
                {
                    per = 0;
                    cuurentParam = jumpParams[pp];
                    startPos = target.localPosition;
                    onLandCallback?.Invoke(new JumpOnLandArgs(jumpParams.Length, pp));
                    //Log.Trace($"{cuurentParam}");
                });
                seq.Append(DOTween.To(() => per
                , value =>
                {
                    per = value;
                    target.localPosition = startPos + cuurentParam.CalcPos(per);
                }
                , 1
                , duration)
                    .SetEase(Ease.Linear));
            }

            seq.AppendCallback(() => onLandCallback?.Invoke(new JumpOnLandArgs(jumpParams.Length, jumpParams.Length)));
            return seq;
        }
    }

    /// <summary>
    /// ジャンプ実行時のコールバック
    /// </summary>
    public delegate void JumpAction(JumpOnLandArgs e);

    /// <summary>
    /// ジャンプ実行時のイベント引数です。
    /// </summary>
    public readonly struct JumpOnLandArgs
    {
        /// <summary>
        /// ジャンプ回数を取得します。
        /// </summary>
        public readonly int JumpCount;

        /// <summary>
        /// 現在のジャンプ回数を取得します。
        /// </summary>
        public readonly int CurrentJumpCount;

        /// <summary>
        /// 最後のジャンプが完了したかどうかを取得します。
        /// </summary>
        public bool IsCompleted => JumpCount == CurrentJumpCount;

        public JumpOnLandArgs(int max, int current)
        {
            JumpCount = max;
            CurrentJumpCount = current;
        }
    }

    /// <summary>
    /// DOTweenのToをパラメータークラスに対応させたバージョン
    /// </summary>
    public static class DOTweenUtil
    {
        public static Tween To(DOGetter<float> getter, DOSetter<float> setter, TweenFloat param)
        {
            return DOTween.To(getter, setter, param.EndValue, param.Duration).AddOptions(param);

        }
        public static Tween To(DOGetter<float> getter, DOSetter<float> setter, TweenFloatAsset param)
        {
            return DOTween.To(getter, setter, param.EndValue, param.Duration).AddOptions(param);
        }
    }

    /// <summary>
    /// 文字送り中に発生するイベント用のクラス
    /// </summary>
    public readonly struct FeedTextArgs
    {
        /// <summary>
        /// 文字送りを実行中のコンポーネントを取得します。
        /// </summary>
        public readonly TMP_Text TMP_Text;

        /// <summary>
        /// 文字送りを行う文字を取得します。
        /// </summary>
        public readonly string Text;

        /// <summary>
        /// 現在の表示文字数を取得します。
        /// </summary>
        public readonly int Position;

        /// <summary>
        /// 文字送りを完了したかどうかを取得します。
        /// true: 完了 / false: 更新中
        /// </summary>
        public readonly bool IsCompleted;

        public FeedTextArgs(TMP_Text component, string text, int pos, bool isCompleted)
        {
            TMP_Text = component;
            Text = text;
            Position = pos;
            IsCompleted = isCompleted;
        }
    }

    /// <summary>
    /// ジャンプパラメーターを表します。
    /// </summary>
    public readonly struct JumpParam
    {
        // 相対的な終了位置
        public readonly Vector3 DeltaPos;
        // ジャンプの高さ
        public readonly float Height;
        // 実行時間
        public readonly float Duration;

        public JumpParam(Vector3 delta, float hiegth, float duration)
        {
            DeltaPos = delta;
            Height = hiegth;
            Duration = duration;
        }

        public JumpParam(float delta_x, float delta_y, float hiegth, float duration)
        {
            DeltaPos = new(delta_x, delta_y);
            Height = hiegth;
            Duration = duration;
        }

        public static JumpParam operator /(JumpParam param, float rate)
        {
            return new JumpParam(param.DeltaPos / rate, param.Height / rate, param.Duration / rate);
        }
        public static JumpParam operator *(JumpParam param, float rate)
        {
            return new JumpParam(param.DeltaPos * rate, param.Height * rate, param.Duration * rate);
        }

        /// <summary>
        /// 現在のオブジェクトの値と指定した割合に応じた位置を計算します。
        /// </summary>
        public Vector3 CalcPos(float per)
        {
            // 縦方向
            float frac = Mathf.Clamp01(per);
            float y = Height * 4.0f * frac * (1.0f - frac);
            y += DeltaPos.y * per;

            // 横方向
            float x = DeltaPos.x * per;
            float z = DeltaPos.z * per;

            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return $"Vec3={DeltaPos}, Height={Height}, Dration={Duration}";
        }
    }
}
