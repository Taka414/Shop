//
// (C) 2025 Takap.
//

using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// フェードイン・アウトを使用した画面遷移を行います。
    /// </summary>
    public class ScreenFader : MonoBehaviour, IScreenFader
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -

        const float FADE_OUT_TIME = 0.4f;
        const float FADE_IN_TIME = 0.3f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        const string Grp = "フェード時間設定";
        // インスペクタ上で値を調整するかどうかのモード措定
        // true: インスペクタ値を使用 / false: 既定値を使用
        [SerializeField, BoxGroup(Grp)]
        bool _useInspectorMode;
        // フェードアウトの時間
        [SerializeField, EnableIf(nameof(_useInspectorMode)), BoxGroup(Grp)]
        float _fadeOutTime = FADE_OUT_TIME;
        // フェードインの時間
        [SerializeField, EnableIf(nameof(_useInspectorMode)), BoxGroup(Grp)]
        float _fadeInTime = FADE_IN_TIME;

        CanvasGroup _canvasGroup;
        Image _image;

        // フェードが実行中かどうかのフラグ
        // true: 実行中 / false: 未実行
        bool _fadeExecuting;
        // シーン変更が実行中かどうかのフラグ
        // true: 実行中 / false: 未実行
        bool _transitionExecuting;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// フェード時の色を設定または取得します。
        /// </summary>
        public Color FadeColor { get => _image.color; set => _image.color = value; }

        /// <summary>
        /// 現在シーンがフェードアウト中かどうかを取得します。
        /// </summary>
        public bool IsFadeOut { get; private set; }

        //
        // Runtimes
        // - - - - - - - - - - - - - - - - - - - -

        void Awake()
        {
            this.SetComponent(ref _canvasGroup);
            this.SetComponent(ref _image);
            _canvasGroup.alpha = 0f;
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 画面をフェードアウトします。
        /// </summary>
        [Button]
        public async UniTask FadeOut(float duration)
        {
            if (_fadeExecuting)
            {
                Log.Warn($"[拒否] フェードアウトが実行中です", this);
                return;
            }
            _fadeExecuting = true;

            try
            {
                Log.Trace("[開始] フェードアウト Start >>>>>", this);

                gameObject.SetActive(true);
                _canvasGroup.alpha = 0;

                await _canvasGroup.DOFade(1, duration).BindTo(this);

                IsFadeOut = true;
            }
            finally
            {
                _fadeExecuting = false;
                Log.Trace("[終了] フェードアウト <<<<<", this);
            }
        }

        /// <summary>
        /// 画面をフェードインします。
        /// </summary>
        [Button]
        public async UniTask FadeIn(float duration)
        {
            if (_fadeExecuting)
            {
                Log.Warn($"[拒否] フェードインが実行中です", this);
                return; // 多重呼び出し禁止
            }
            _fadeExecuting = true;

            try
            {
                Log.Trace("[開始] フェードイン >>>>>", this);

                await _canvasGroup.DOFade(0, duration).BindTo(this); // 中途半端な状態から再開は時間を可変にする

                gameObject.SetActive(false);

                IsFadeOut = false;
            }
            finally
            {
                _fadeExecuting = false;
                Log.Trace("[終了] フェードイン <<<<<", this);
            }
        }

        /// <summary>
        /// 画面をフェードアウトした後に指定したシーンに遷移してフェードインします。
        /// </summary>
        [Button]
        public async UniTask ChangeSceneWithFade(string sceneName)
        {
            float fOut = FADE_OUT_TIME;
            float fIn = FADE_IN_TIME;
            if (_useInspectorMode)
            {
                fOut = _fadeOutTime;
                fIn = _fadeInTime;
            }
            await ChangeSceneWithFade(sceneName, fOut, fIn);
        }

        /// <summary>
        /// 画面をフェードアウトした後に指定したシーンに遷移してフェードインします。
        /// </summary>
        [Button]
        public async UniTask ChangeSceneWithFade(string sceneName, float outDuration, float inDuration)
        {
            if (_transitionExecuting)
            {
                Log.Warn($"[拒否] シーンチェンジが実行中です", this);
                return; // 多重呼び出し禁止
            }
            _transitionExecuting = true;

            try
            {
                await FadeOut(outDuration);
                await SceneManager.LoadSceneAsync(sceneName);
                await FadeIn(inDuration);
            }
            finally
            {
                _transitionExecuting = false;
            }
        }
    }
}