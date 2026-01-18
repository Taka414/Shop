//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 自分でアップデートを管理するときに使用する基底クラスを表します。
    /// </summary>
    public abstract class CustomUpdateBehaviour : MonoBehaviour, IUpdatable
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // StartLocal を呼び出したかどうかのフラグ
        // true: 呼び出した / false: まだ
        bool _isStarted;

        LocalUpdateSystem _localUpdateSyatem;
        [VContainer.Inject]
        public void Construct(LocalUpdateSystem localUpdateSyatem)
        {
            Validator.SetValueIfThrowNull(ref _localUpdateSyatem, localUpdateSyatem);
        }


        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        public bool IsEnabled => enabled;

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void UpdateLocalCore()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                StartLocal();
            }
            UpdateLocal();
        }

        private void OnEnable()
        {
            _localUpdateSyatem.Add(this);
            OnEnabledLocal();
        }

        private void OnDisable()
        {
            _localUpdateSyatem.Remove(this);
            OnDisableLocal();
        }

        //
        // Abstract
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このオブジェクトを継承したクラスが Awake の代わりに使用するメソッドです。
        /// </summary>
        protected abstract void OnEnabledLocal(); // 忘れやすいので強制的に実装する

        /// <summary>
        /// このオブジェクトを継承したクラスが OnDestroy の代わりに使用するメソッドです。
        /// </summary>
        protected abstract void OnDisableLocal();

        /// <summary>
        /// このオブジェクトを継承したクラスが Start() の代わりに使用するメソッドです。
        /// <see cref="UpdateLocal"/> が呼ばれる直前に1度だけ呼び出されます。
        /// </summary>
        protected abstract void StartLocal();

        /// <summary>
        /// このオブジェクトを継承したクラスが使用する Update() の代わりに使用するメソッドです。
        /// Updateの代わりに毎フレーム呼び出されます。
        /// </summary>
        protected abstract void UpdateLocal();
    }
}