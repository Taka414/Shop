//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{

    /// <summary>
    /// ゲーム全体の共通の設定をするクラス
    /// </summary>
    public class GameEnvironment : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // 解像度の設定グループ -------------------------------------
        const string grp1 = "Screen Size Settings";
        // 解像度を指定するかどうかの指定
        // true: 指定する / false: 指定しない
        [SerializeField, BoxGroup(grp1)]
        bool _useScreenSize;
        // ゲームの基準解像度の設定
        [SerializeField, BoxGroup(grp1), EnableIf(nameof(_useScreenSize))]
        Size2DI _baseScreenSize = new Size2DI(1920, 1080);

        // FPSの設定グループ ----------------------------------------
        const string grp2 = "FPS Settings";

        // 自動FPSを指定する可能かの設定(基本こっちを常に使うべき)
        // ** 120FPSのAndroid端末で60FSPを指定するとタッチするたびに120FPSが発生してガクガクになる問題が起きる
        // true: 自動フレームレートを選択する(端末固定) / false: 指定しない
        [SerializeField, BoxGroup(grp2)]
        bool _useAutoFrameRate = true;
        // 固定FPS指定をするかどうかの指定
        // true: 指定する / false: 指定しない
        [SerializeField, BoxGroup(grp2), DisableIf(nameof(_useAutoFrameRate))]
        bool _useFixedTargetFrameRate;
        // 対象とするフレームレート(固定)
        [SerializeField, MinValue(1), BoxGroup(grp2), EnableIf(nameof(_useFixedTargetFrameRate))]
        int _targetFrameRate = 60;

        // 自動スリープするかどうかのフラグ
        // true: 無効 / false: システム設定に従う
        [SerializeField, BoxGroup(grp2)]
        bool _disableSleep;

        // 物理演算の設定グループ -----------------------------------
        const string grp3 = "Physics Settings";
        // このグループが有効か無効かどうかの設定
        // true: 使用する / false: 使用しない
        [SerializeField, BoxGroup(grp3)]
        bool _enablePhysicsSetting;
        // 3D物理演算を使用するかどうか
        // true: 使用する / false: 使用しない
        [SerializeField, BoxGroup(grp3), EnableIf(nameof(_enablePhysicsSetting))]
        SimulationMode _usePhysics3D = SimulationMode.FixedUpdate;
        // 2D物理演算を使用するかどうか
        [SerializeField, BoxGroup(grp3), EnableIf(nameof(_enablePhysicsSetting))]
        SimulationMode2D _usePhysics2D = SimulationMode2D.FixedUpdate;

        // ログの出力方法グループ -----------------------------------
        const string grp4 = "Log StackTrace Settings";
        // このグループが有効か無効かどうかの設定
        // true: 使用する / false: 使用しない
        [SerializeField, BoxGroup(grp4)]
        bool _enableLogStackTraceSetting;
        [SerializeField, BoxGroup(grp4), EnableIf(nameof(_enableLogStackTraceSetting))]
        StackTraceLogType _error = StackTraceLogType.Full;
        [SerializeField, BoxGroup(grp4), EnableIf(nameof(_enableLogStackTraceSetting))]
        StackTraceLogType _assert = StackTraceLogType.Full;
        [SerializeField, BoxGroup(grp4), EnableIf(nameof(_enableLogStackTraceSetting))]
        StackTraceLogType _warning = StackTraceLogType.Full;
        [SerializeField, BoxGroup(grp4), EnableIf(nameof(_enableLogStackTraceSetting))]
        StackTraceLogType _log = StackTraceLogType.Full;
        [SerializeField, BoxGroup(grp4), EnableIf(nameof(_enableLogStackTraceSetting))]
        StackTraceLogType _exception = StackTraceLogType.Full;

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            SetupLogStackTraceSetting();

            SetupScreenSize();
            
            SetFps();
            
            SetupPhysicsSetting();
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 解像度を設定します。
        /// </summary>
        private void SetupScreenSize()
        {
            if (!_useScreenSize)
            {
                Log.Trace("SetupScreenSize disabled", this);
                return;
            }
            Log.Trace("SetupScreenSize enabled", this);

            // 以下の処理だと縦長の端末とかで全然うまく動作しないと出とりあえず無効化しておく
            // 複数解像度の設定はもう少し調べてから実装する

            Screen.SetResolution(_baseScreenSize.Width, _baseScreenSize.Height, true);
            Log.Trace($"Screen SetResolution=({_baseScreenSize.Width} x {_baseScreenSize.Height})", this);
        }

        /// <summary>
        /// FPSを設定します。
        /// </summary>
        private void SetFps()
        {
            // 自動スリープするかどうか
            Screen.sleepTimeout
                = _disableSleep ?
                    SleepTimeout.NeverSleep :   // 無効
                    SleepTimeout.SystemSetting; // システム設定に従う


            if (_useAutoFrameRate)
            {
                double refreshRate = Screen.currentResolution.refreshRateRatio.value;
                if (double.IsNaN(refreshRate)) 
                {
                    refreshRate = 60;
                    Log.Warn("Screen.currentResolution.refreshRateRatio.value is NaN");
                }
                Application.targetFrameRate = (int)refreshRate;
                Log.Trace($"Target FrameRate(Auto)={Application.targetFrameRate}", this);
            }
            else
            {
                if (!_useFixedTargetFrameRate)
                {
                    Log.Trace("Use Fixed Target FrameRate disabled", this);
                    return;
                }
                Log.Trace("Use Fixed Target FrameRate enabled", this);

                // VSyncを同期しないように明示する
                // 0が無効(Don't Sync)
                QualitySettings.vSyncCount = 0;

                // フレームレートを指定する
                Application.targetFrameRate = _targetFrameRate;
                Log.Trace($"Target FrameRate(Fixed)={Application.targetFrameRate}", this);
            }
        }

        /// <summary>
        /// 物理演算をどうするか設定します。
        /// </summary>
        private void SetupPhysicsSetting()
        {
            if (!_enablePhysicsSetting)
            {
                Log.Trace("SetupPhysicsSetting disabled", this);
                return;
            }
            Log.Trace("SetupPhysicsSetting enabled", this);

            // false にすると無効化される
            // 2Dだと大抵無効化しておいた方がいいのでfalse固定かも
            Physics.simulationMode = _usePhysics3D;

            // これは SimulationMode2D.FixedUpdate で問題なさそう
            Physics2D.simulationMode = _usePhysics2D;
        }

        /// <summary>
        /// ログのスタックトレースをレベルごとに出力するかどうかを設定します・
        /// </summary>
        private void SetupLogStackTraceSetting()
        {
            if (!_enableLogStackTraceSetting)
            {
                Log.Trace("SetupLogSetting disabled", this);
                return;
            }
            Log.Trace("SetupLogSetting enabled", this);
            Application.SetStackTraceLogType(LogType.Error, _error);
            Application.SetStackTraceLogType(LogType.Assert, _assert);
            Application.SetStackTraceLogType(LogType.Warning, _warning);
            Application.SetStackTraceLogType(LogType.Log, _log);
            Application.SetStackTraceLogType(LogType.Exception, _exception);
            Log.Trace($"LogType.Error={_error}", this);
            Log.Trace($"LogType.Assert={_assert}", this);
            Log.Trace($"LogType.Warning={_warning}", this);
            Log.Trace($"LogType.Log={_log}", this);
            Log.Trace($"LogType.Exception={_exception}", this);
        }
    }
}
