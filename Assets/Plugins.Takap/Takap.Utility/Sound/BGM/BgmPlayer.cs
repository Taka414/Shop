// 
// (C) 2023 Takap.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Takap.Utility.Sound
{
    /// <summary>
    /// 4つの<see cref="AudioSource"/>とスケジューリングメソッドを連携させイントロ部分で隙間のないループ音楽を実現するコンポーネントです。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 2 つの <see cref="AudioSource"/> は、オーディオを正確につなぎ合わせるためにスケジューリングメソッドを使用し
    /// 他の2つのソースは、新しいイントロループオーディオへのクロスフェードをサポートするためにあります。
    /// </para>
    /// <para>
    /// 4つのソースが同時に再生される瞬間があります。<br/>
    /// (1) 1つのイントロループオーディオが継ぎ目で再生されながら、継ぎ目付近で始まる別の イントロループオーディオにクロスフェードするタスクが課されている場合<br/>
    /// (2) あるイントロループオーディオが継ぎ目付近で始まり、別のイントロループオーディオにクロスフェードするタスク
    /// </para>
    /// </remarks>
    public class BgmPlayer : MonoBehaviour, IBgmPlayer
    {
        //
        // Constant
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// このフェードは聴こえないので、突然曲を止めたときの大きなポップ音やクリック音を消すのに役立ちます。<br/>
        /// これは<see cref="Stop()"/>した時に自動的に使われます。<br/>
        /// どうしてもこれが必要でなければ、<see cref="Stop(float)"/>でフェードの長さを0秒にしてください。
        /// </summary>
        const float _popRemovalFadeTime = 0.055f;

        //
        // Fields (Static)
        // - - - - - - - - - - - - - - - - - - - -

        //// シングルトン用
        //static BgmPlayer _instance;
        //static AudioSource _singletonInstanceTemplateSource;

        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// AudioSourceのClips スロットと同じ働きをします。API で Play() を呼び出した時に再生されるアセットです。
        /// ただし、Introloop にも Play オーバーロードがあり、このフィールドに接続されているアセット参照に関係なく、引数を介して再生するアセットを送信することができます。
        /// </summary>
        [SerializeField] BgmAudioClip _defaultIntroloopAudio;

        /// <summary>
        /// Introloop はループを管理するために、実行時に4つの AudioSource を生成します。<br/>
        /// すべてのソースは、出力の AudioMixerGroup を含め、このテンプレートの AudioSource から設定を継承します。<br/>
        /// この IntroloopPlayer コンポーネントの隣のテンプレートに AudioSource を追加して、このスロットに接続することができます。<br/>
        /// AudioSourceが 割り当てられていない場合は、初回再生時にこのコンポーネントの隣に、Priority 0, Spatial Blend 2D の設定で新しい AudioSource コンポーネントが追加されます。<br/>
        /// (BGM 用として期待される設定です) そして、このフィールドは、新しく追加された AudioSource をテンプレートとして参照します。
        /// </summary>
        [SerializeField] AudioSource _templateSource;

        ///// <summary>
        ///// Play On Awake of AudioSource のように動作し、Awake() 時に接続されている「Default Introloop Audio」アセットを自動的に再生します。
        ///// </summary>
        //[SerializeField] bool _playOnAwake;

        ///// <summary>
        ///// 設定用のオブジェクト
        ///// </summary>
        //[SerializeField] IntroloopSettings _introloopSettings;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        readonly float[] _fadeLength = new float[2];
        readonly float[] _towardsVolume = new float[2];
        readonly IntroloopTrack[] _twoTracks = new IntroloopTrack[2];
        readonly bool[] _willPause = new bool[2];
        readonly bool[] _willStop = new bool[2];

        // 最初の<see cref="Play(IntroloopAudio,float,float)"/>で0に変わる。0が最初のトラック
        int _currentTrack = 1;

        // セットアップしたかどうかのフラグ
        // true: セットアップした / false: まだ
        bool _isSetupChildComponent;

        BgmAudioClip _previousPlay;

        float _timeBeforePause;

        // 子要素のAudioSource
        AudioSource[] _audioSourceCache;

        // 
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// <see cref="AudioSource.clip"/> プロパティのように動作します。
        /// これを任意の <see cref="BgmAudioClip"/> に設定すると、<see cref="Play()"/>または<see cref="Play(float,float)"/> を呼び出した時に使用されます。
        /// </summary>
        public BgmAudioClip DefaultIntroloopAudio
        {
            get => _defaultIntroloopAudio;
            set => _defaultIntroloopAudio = value;
        }

        ///// <summary>
        ///// <see cref="AudioSource.playOnAwake"/> のように動作し、接続された<see cref="DefaultIntroloopAudio"/> を Awake 時に自動的に再生します。
        ///// </summary>
        //public bool PlayOnAwake
        //{
        //    get => _playOnAwake;
        //    set => _playOnAwake = value;
        //}

        /// <summary>
        /// <para>
        /// Start() で初めて4つの <see cref="AudioSource"/> を生成する場合、この
        /// <see cref="AudioSource"/> の参照からフィールドを読み出し、4つすべてにコピーします。
        /// 既に基礎となる4つの <see cref="AudioSource"/> が生成された後にこれを割り当てても何もおきません。
        /// </para>
        /// <para>
        /// Start() の後にこのテンプレートを再び適用するには、<see cref="ApplyAudioSource"/> を使います。
        /// 引数はこの <see cref="TemplateSource"/> か他の <see cref="AudioSource"/> です。
        /// </para>
        /// </summary>
        /// <remarks>
        /// <see cref="AudioSource"/>は、<see cref="MonoBehaviour.enabled"/>である必要はありません。
        /// また、シーン上のどこかにある必要はなく、プロジェクト内の<see cref="AudioSource"/>を持つプレハブから取得できます。
        /// </remarks>
        public AudioSource TemplateSource
        {
            get
            {
                if (_templateSource != null)
                {
                    return _templateSource;
                }

                // A fallback to pickup nearby AudioSource as a template.
                _templateSource = GetComponent<AudioSource>();
                if (_templateSource != null)
                {
                    return _templateSource;
                }

                // If no template source, make it a high priority + 2D source by default.
                _templateSource = gameObject.AddComponent<AudioSource>();
                SetupDefaultAudioSourceForIntroloop(_templateSource);

                return _templateSource;
            }
            set => _templateSource = value;
        }

        /// <summary>
        /// Introloopが利用する4つの <see cref="AudioSource"/> すべてに
        /// 一度に影響を与えるようなことをしたい場合は、このプロパティを<c>foreach</c>してください。
        /// </summary>
        /// <remarks>
        /// Awake でこれを使うべきではありません。Introloop がまだ <see cref="AudioSource"/> を生成していないかもしれないからです。
        /// </remarks>
        public IEnumerable<AudioSource> InternalAudioSources
        {
            get
            {
                ThrowsExceptionIfNotSetup();

                foreach (var aSource in _twoTracks[0].AllAudioSources)
                {
                    yield return aSource;
                }

                foreach (var aSource in _twoTracks[1].AllAudioSources)
                {
                    yield return aSource;
                }
            }
        }

        ///// <summary>
        ///// <para>
        ///// コードのどこからでも <see cref="BgmPlayer"/> の便利なシングルトンインスタンスを取得できます。
        ///// これは DontDestroyOnLoad が適用されています。
        ///// </para>
        ///// <para>
        ///// これを初めて呼び出す前に、スクリプトから <see cref="TemplateSource"/> をセットアップするために
        ///// まず <see cref="SetSinglontInstanceTemplateSource"/> を呼び出します。
        ///// (それは実行時まで存在しないので、シングルトンでないインスタンスとは異なり、前もってテンプレートをセットアップすることはできません)
        ///// </para>
        ///// </summary>
        //public static BgmPlayer Instance
        //{
        //    get
        //    {
        //        if (_instance != null)
        //        {
        //            return _instance;
        //        }
        //
        //        _instance = InstantiateSingletonPlayer<BgmPlayer>(_singletonInstanceTemplateSource);
        //        //_instance.name = IntroloopSettings.singletonObjectPrefix + _instance.name;
        //        _instance.name = "[-] BGM Player";
        //
        //        return _instance;
        //    }
        //}

        /// <summary>
        /// 現在のボリュームを設定または取得します。
        /// 0.0 ～ 1.0
        /// </summary>
        /// <remarks>
        /// 初期化時に音量を指定しないと音量ゼロなので注意
        /// </remarks>
        public float Volume
        {
            get => _volume.Value;
            set
            {
                ThrowsExceptionIfNotSetup();

                if (Mathf.Approximately(value, _volume.Value)) return;
                _volume.Value = Mathf.Clamp01(value);

                _audioSourceCache ??= InternalAudioSources.ToArray();
                foreach (AudioSource src in _audioSourceCache)
                {
                    src.volume = _volume.Value;
                }
            }
        }
        public R3.Observable<float> VolumeChanged => _volume;
        private readonly R3.ReactiveProperty<float> _volume = new();

        /// <summary>
        /// 音量を0.1上げます。
        /// </summary>
        public void VolumeUp()
        {
            Volume = (float)Math.Round(_volume.Value + 0.1f, 1);
        }

        /// <summary>
        /// 音量を0.1下げます。
        /// </summary>
        public void VolumeDown()
        {
            Volume = (float)Math.Round(_volume.Value - 0.1f, 1);
        }

        //
        // Unity impl
        // - - - - - - - - - - - - - - - - - - - -

        void Awake()
        {
            //if (_introloopSettings == null)
            //{
            //    _introloopSettings = new IntroloopSettings();
            //}

            SetupChildComponents();

            //if (_playOnAwake)
            //{
            //    Play();
            //}
        }

        void Start()
        {
            TemplateSource.enabled = false;
            ApplyAudioSource(TemplateSource);
        }

        void Update()
        {
            FadeUpdate();
            _twoTracks[0].SchedulingUpdate();
            _twoTracks[1].SchedulingUpdate();
        }

        /// <summary>
        ///     <para>
        ///         これは、2019.1以上ででゲームの最小化または <see cref="AudioListener"/> 一時停止時に、
        ///         すべての <see cref="AudioSource.SetScheduledEndTime(double)"/> が失われるバグの汚い回避策です。
        ///     </para>
        ///     <para>
        ///         2018.4 LTS では問題ないことを確認しました。
        ///         理想的な修正方法は、ゲームが最小化される直前に一時停止を呼び出し、再スケジュールするためにカムバックした後に再開することです。
        ///     </para>
        ///     <para>
        ///         しかし、このコールバックでは、すべてのオーディオがすでに一時停止に向かっているため、一時停止は機能しません。
        ///         そこで別の方法として、一時停止する直前の時間を記憶しておき、その時間を使って戻ってきた後に再度再生するという方法がありますが
        ///         ここでは Play の代わりに Seek メソッドを使うことができるので、前のオーディオを指定する必要はありません。
        ///     </para>
        ///     <para>
        ///         Please see : https://forum.unity.com/threads/introloop-easily-play-looping-music-with-intro-section-v4-0-0-2019.378370/#post-4793741
        ///         Track the case here : https://fogbugz.unity3d.com/default.asp?1151637_4i53coq9v07qctp1
        ///     </para>
        /// </summary>
        public void OnApplicationPause(bool paused)
        {
#if !UNITY_6000_0_OR_NEWER
            // これが有効だとUnity6だと再開しない不具合がある
            if (paused)
            {
                _timeBeforePause = GetPlayheadTime();
            }
            else
            {
                Seek(_timeBeforePause);
            }
#endif
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        ///// <summary>
        ///// 現在 <see cref="DefaultIntroloopAudio"/> に割り当てられている <see cref="BgmAudioClip"/> アセットを再生します。
        ///// </summary>
        ///// <remarks>
        /////     <para>
        /////         It applies <see cref="BgmAudioClip.Volume"/> and <see cref="BgmAudioClip.Pitch"/>
        /////         to the underlying <see cref="AudioSource"/>.
        /////     </para>
        /////     <para>
        /////         If an another <see cref="BgmAudioClip"/> is playing on this player,
        /////         it could cross-fade between the two if <paramref name="fadeLengthSeconds"/> is provided.
        /////         The faded out audio will be unloaded automatically once the fade is finished.
        /////     </para>
        ///// </remarks>
        ///// <param name="fadeLengthSeconds">
        /////     Fade in/out length to use in seconds.
        /////     <list type="bullet">
        /////         <item>
        /////             <description>If 0, it uses a small pop removal fade time.</description>
        /////         </item>
        /////         <item>
        /////             <description>If negative, it is immediate.</description>
        /////         </item>
        /////     </list>
        /////     The audio will be unloaded only after it had fade out completely.
        ///// </param>
        ///// <param name="startTime">
        /////     <para>
        /////         Specify starting point in time instead of starting from the beginning.
        /////     </para>
        /////     <para>
        /////         The time you specify here will be converted to "play head time", Introloop will make the play head
        /////         at the point in time as if you had played for this amount of time before starting.
        /////     </para>
        /////     <para>
        /////         Since <see cref="BgmAudioClip"/> conceptually has infinite length, any number that is over looping boundary
        /////         will be wrapped over to the intro boundary in the calculation. (Except that if the audio is non-looping)
        /////     </para>
        /////     <para>
        /////         The time specified here is <b>not</b> taking <see cref="BgmAudioClip.Pitch"/> into account.
        /////         It's an elapsed time as if <see cref="BgmAudioClip.Pitch"/> is 1.
        /////     </para>
        ///// </param>
        ///// <exception cref="ArgumentNullException">
        /////     Thrown when <see cref="DefaultIntroloopAudio"/> was not assigned.
        ///// </exception>
        //[Obsolete("使わないで")]
        //public void Play(float fadeLengthSeconds = 0, float startTime = 0)
        //{
        //    if (_defaultIntroloopAudio == null)
        //    {
        //        throw new ArgumentNullException(nameof(_defaultIntroloopAudio),
        //            "Default Introloop Audio was not assigned, but you called " +
        //            "Play overload without IntroloopAudio argument.");
        //    }
        //
        //    Play(_defaultIntroloopAudio, fadeLengthSeconds, startTime);
        //}

        /// <summary>
        /// 指定した <paramref name="introloopAudio"/> を再生します。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         It applies <see cref="BgmAudioClip.Volume"/> and <see cref="BgmAudioClip.Pitch"/>
        ///         to the underlying <see cref="AudioSource"/>.
        ///     </para>
        ///     <para>
        ///         If an another <see cref="BgmAudioClip"/> is playing on this player,
        ///         it could cross-fade between the two if <paramref name="fadeLengthSeconds"/> is provided.
        ///         The faded out audio will be unloaded automatically once the fade is finished.
        ///     </para>
        /// </remarks>
        /// <param name="introloopAudio">
        ///     A reference to <see cref="BgmAudioClip"/> asset file to play.
        /// </param>
        /// <param name="fadeLengthSeconds">
        ///     Fade in/out length to use in seconds.
        ///     <list type="bullet">
        ///         <item>
        ///             <description>If 0, it uses a small pop removal fade time.</description>
        ///         </item>
        ///         <item>
        ///             <description>If negative, it is immediate.</description>
        ///         </item>
        ///     </list>
        ///     The audio will be unloaded only after it had fade out completely.
        /// </param>
        /// <param name="startTime">
        ///     <para>
        ///         Specify starting point in time instead of starting from the beginning.
        ///     </para>
        ///     <para>
        ///         The time you specify here will be converted to "play head time", Introloop will make the play head
        ///         at the point in time as if you had played for this amount of time before starting.
        ///     </para>
        ///     <para>
        ///         Since <see cref="BgmAudioClip"/> conceptually has infinite length, any number that is over looping boundary
        ///         will be wrapped over to the intro boundary in the calculation. (Except that if the audio is non-looping)
        ///     </para>
        ///     <para>
        ///         The time specified here is <b>not</b> taking <see cref="BgmAudioClip.Pitch"/> into account.
        ///         It's an elapsed time as if <see cref="BgmAudioClip.Pitch"/> is 1.
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="introloopAudio"/> is `null`.</exception>
        public void Play(BgmAudioClip introloopAudio, float fadeLengthSeconds = 0, float startTime = 0)
        {
            if (introloopAudio == null)
            {
                throw new ArgumentNullException(nameof(introloopAudio), "Played IntroloopAudio is null");
            }

            // 音量を上書き -> IntroloopAudioClipに設定されている音量は無効化する
            introloopAudio.Volume = _volume.Value;

            //Auto cross fade old ones. If no fade length specified, a very very small fade will be used to avoid pops/clicks.
            Stop(fadeLengthSeconds);

            var next = (_currentTrack + 1) % 2;
            _twoTracks[next].Play(introloopAudio, fadeLengthSeconds == 0 ? false : true, startTime);
            _towardsVolume[next] = 1;
            _fadeLength[next] = TranslateFadeLength(fadeLengthSeconds);

            _currentTrack = next;
            _previousPlay = introloopAudio;
        }

        /// <summary>
        ///     Similar to <see cref="Play(BgmAudioClip, float, float)"/> overload, but has only a single
        ///     argument so it is able to receive calls from <see cref="UnityEvent"/>.
        /// </summary>
        public void Play(BgmAudioClip introloopAudio)
        {
            Play(introloopAudio, 0);
        }

        ///// <summary>
        /////     Similar to <see cref="Play(BgmAudioClip, float, float)"/> overload, but has no
        /////     optional arguments so it is able to receive calls from <see cref="UnityEvent"/>.
        ///// </summary>
        //[Obsolete("使わないで")]
        //public void Play()
        //{
        //    Play(0);
        //}

        /// <summary>
        ///     Move the play head of the currently playing audio to anywhere in terms of elapsed time.
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 If it is currently playing, you can instantly move the play head position to anywhere else.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 If it is not playing, no effect. (This includes while in paused state, you cannot seek in paused state.)
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         An internal implementation is not actually a seek, but a completely new
        ///         <see cref="Play(BgmAudioClip, float, float)"/> with the previous <see cref="BgmAudioClip"/>.
        ///     </para>
        ///     <para>
        ///         This is why you cannot seek while in pause, as it actually does a new play for you.
        ///         It is handy because it doesn't require you to remember and specify that audio again.
        ///     </para>
        /// </remarks>
        /// <param name="elapsedTime">
        ///     <para>
        ///         Introloop will make the play head at the point in time as if you had played for this amount
        ///         of time before starting.
        ///     </para>
        ///     <para>
        ///         The time you specify here will be converted to "play head time", Introloop will make the play head
        ///         at the point in time as if you had played for this amount of time before starting.
        ///     </para>
        ///     <para>
        ///         Since <see cref="BgmAudioClip"/> conceptually has infinite length, any number that is over looping boundary
        ///         will be wrapped over to the intro boundary in the calculation. (Except that if the audio is non-looping)
        ///         The time specified here is <b>not</b> taking <see cref="BgmAudioClip.Pitch"/> into account.
        ///         It's an elapsed time as if <see cref="BgmAudioClip.Pitch"/> is 1.
        ///     </para>
        /// </param>
        public void Seek(float elapsedTime)
        {
            if (!_twoTracks[_currentTrack].IsPlaying)
            {
                return;
            }

            _twoTracks[_currentTrack].Play(_previousPlay, false, elapsedTime);
            _towardsVolume[_currentTrack] = 1;
            _fadeLength[_currentTrack] = 0;
        }

        /// <summary>
        ///     Stop the currently playing <see cref="BgmAudioClip"/> immediately and unload it from memory.
        /// </summary>
        public void Stop()
        {
            _willStop[_currentTrack] = false;
            _willPause[_currentTrack] = false;
            _fadeLength[_currentTrack] = 0;
            _twoTracks[_currentTrack].FadeVolume = 0;
            _twoTracks[_currentTrack].Stop();
            UnloadTrack(_currentTrack);
        }

        /// <summary>
        ///     Fading out to stop the currently playing <see cref="BgmAudioClip"/>, and unload it from memory
        ///     once it is completely faded out.
        /// </summary>
        /// <param name="fadeLengthSeconds">
        ///     Fade out length to use in seconds.
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 is a special value that will still apply small pop removal fade time.</description>
        ///         </item>
        ///         <item>
        ///             <description>If negative, this method works like <see cref="Stop()"/> overload.</description>
        ///         </item>
        ///     </list>
        /// </param>
        public void Stop(float fadeLengthSeconds)
        {
            if (fadeLengthSeconds < 0)
            {
                Stop();
            }
            else
            {
                _willStop[_currentTrack] = true;
                _willPause[_currentTrack] = false;
                _fadeLength[_currentTrack] = TranslateFadeLength(fadeLengthSeconds);
                _towardsVolume[_currentTrack] = 0;
            }
        }

        /// <summary>
        ///     Pause the currently playing <see cref="BgmAudioClip"/> immediately without unloading.
        ///     Call <see cref="Resume(float)"/> to continue playing.
        /// </summary>
        public void Pause()
        {
            if (_twoTracks[_currentTrack].IsPausable())
            {
                _willStop[_currentTrack] = false;
                _willPause[_currentTrack] = false;
                _fadeLength[_currentTrack] = 0;
                _twoTracks[_currentTrack].FadeVolume = 0;
                _twoTracks[_currentTrack].Pause();
            }
        }

        /// <summary>
        ///     Fading out to pause the currently playing <see cref="BgmAudioClip"/> without unloading.
        ///     Call <see cref="Resume(float)"/> to continue playing.
        /// </summary>
        /// <param name="fadeLengthSeconds">
        ///     Fade out length to use in seconds.
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 is a special value that will still apply small pop removal fade time.</description>
        ///         </item>
        ///         <item>
        ///             <description>If negative, this method works like <see cref="Pause()"/> overload.</description>
        ///         </item>
        ///     </list>
        /// </param>
        public void Pause(float fadeLengthSeconds)
        {
            if (_twoTracks[_currentTrack].IsPausable())
            {
                if (fadeLengthSeconds < 0)
                {
                    Pause();
                }
                else
                {
                    _willPause[_currentTrack] = true;
                    _willStop[_currentTrack] = false;
                    _fadeLength[_currentTrack] = TranslateFadeLength(fadeLengthSeconds);
                    ;
                    _towardsVolume[_currentTrack] = 0;
                }
            }
        }

        /// <summary>
        ///     Resume playing of previously paused (<see cref="Pause(float)"/>) <see cref="BgmAudioClip"/>.
        ///     If currently not pausing, it does nothing.
        /// </summary>
        /// <remarks>
        ///     Note that if it is currently "fading to pause", the state is not considered paused
        ///     yet so you can't resume in that time.
        /// </remarks>
        /// <param name="fadeLengthSeconds">
        ///     Fade out length to use in seconds.
        ///     <list type="bullet">
        ///         <item>
        ///             <description>If 0, it uses a small pop removal fade time.</description>
        ///         </item>
        ///         <item>
        ///             <description>If negative, it resumes immediately.</description>
        ///         </item>
        ///     </list>
        /// </param>
        public void Resume(float fadeLengthSeconds = 0)
        {
            if (_twoTracks[_currentTrack].Resume())
            {
                //Resume success
                _willStop[_currentTrack] = false;
                _willPause[_currentTrack] = false;
                _towardsVolume[_currentTrack] = 1;
                _fadeLength[_currentTrack] = TranslateFadeLength(fadeLengthSeconds);
            }
        }

        /// <summary>
        ///     An experimental feature in the case that you really want the audio to start
        ///     in an instant you call <see cref="Play(BgmAudioClip, float, float)"/>. You must use the same
        ///     <see cref="BgmAudioClip"/> that you preload in the next play.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         By normally using <see cref="Play(BgmAudioClip, float, float)"/> and <see cref="Stop(float)"/>
        ///         it loads the audio the moment you called <see cref="Play(BgmAudioClip, float, float)"/>.
        ///         Introloop waits for an audio to load before playing with a coroutine.
        ///     </para>
        ///     <para>
        ///         (Only if you have <see cref="AudioClip.loadInBackground"/> in the import settings.
        ///         Otherwise, <see cref="Play(BgmAudioClip, float, float)"/> will be a blocking call.)
        ///     </para>
        ///     <para>
        ///         Introloop can't guarantee that the playback will be instant,
        ///         but your game can continue while it is loading. By using this method before actually calling
        ///         <see cref="Play(BgmAudioClip, float, float)"/> it will instead be instant.
        ///     </para>
        ///     <para>
        ///         This function is special even songs with <see cref="AudioClip.loadInBackground"/>
        ///         can be loaded in a blocking fashion. (You can put <see cref="Play(BgmAudioClip, float, float)"/> immediately
        ///         in the next line expecting a fully loaded audio.)
        ///     </para>
        ///     <para>
        ///         However be aware that memory is managed less efficiently in the following case :
        ///         Normally Introloop immediately unloads the previous track to minimize memory.
        ///         But if you use <see cref="Preload(BgmAudioClip)"/> then did not call
        ///         <see cref="Play(BgmAudioClip, float, float)"/> with the same <see cref="BgmAudioClip"/> afterwards,
        ///         the loaded memory will be unmanaged.
        ///     </para>
        ///     <para>
        ///         (Just like if you tick <see cref="AudioClip.preloadAudioData"/> on your clip and have them
        ///         in a hierarchy somewhere, then did not use it.)
        ///     </para>
        ///     <para>
        ///         Does not work with <see cref="AudioClipLoadType.Streaming"/> audio loading type.
        ///     </para>
        /// </remarks>
        public void Preload(BgmAudioClip introloopAudio)
        {
            introloopAudio.Preload();
        }

        /// <summary>
        ///     This interpretation of a play time could decrease when it goes over
        ///     looping boundary back to intro boundary. Conceptually Introloop audio has infinite length,
        ///     so this time is a bit different from normal sense.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Think as it as not "elapsed time" but rather the position of the actual playhead,
        ///         expressed in time as if the pitch is 1.
        ///     </para>
        ///     <para>
        ///         For example with pitch enabled, the play head will move slowly,
        ///         and so the time returned from this method respect that slower play head.
        ///     </para>
        ///     <para>
        ///         It is usable with <see cref="Play(BgmAudioClip, float, float)"/> as a start time
        ///         to "restore" the play from remembered time. With only 1 <see cref="BgmPlayer"/> you can stop and
        ///         unload previous song then continue later after reloading it.
        ///     </para>
        ///     <para>
        ///         Common use case includes battle music which resumes the field music afterwards.
        ///         If the battle is memory consuming unloading the field music could help.
        ///     </para>
        /// </remarks>
        public float GetPlayheadTime()
        {
            return _twoTracks[_currentTrack].PlayheadPositionSeconds;
        }

        /// <summary>
        ///     Assign a different audio mixer group to all underlying <see cref="AudioSource"/>.
        /// </summary>
        public void SetMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            foreach (var aSource in InternalAudioSources)
            {
                aSource.outputAudioMixerGroup = audioMixerGroup;
            }
        }

        ///// <summary>
        /////     Call this before the first use of <see cref="Instance"/> to have the singleton instance
        /////     copy <see cref="AudioSource"/> fields from <paramref name="templateSource"/>.
        ///// </summary>
        ///// <remarks>
        /////     <para>
        /////         Singleton instance is convenient but you cannot pre-connect <see cref="TemplateSource"/> like
        /////         a regular instance because it does not exist until runtime.
        /////     </para>
        /////     <para>
        /////         If you had already used the singleton instance before calling this, you can still call
        /////         <see cref="ApplyAudioSource"/> on the singleton instance to apply different
        /////         settings of <see cref="AudioSource"/>.
        /////     </para>
        ///// </remarks>
        //public static void SetSingletonInstanceTemplateSource(AudioSource templateSource)
        //{
        //    _singletonInstanceTemplateSource = templateSource;
        //}

        /// <summary>
        ///     <para>
        ///         Copy fields from <paramref name="applyFrom"/> to all 4 underlying <see cref="AudioSource"/>.
        ///         Make it as if they had <paramref name="applyFrom"/> as a <see cref="TemplateSource"/> from
        ///         the beginning. (Or you can think this method as a way to late-assign a <see cref="TemplateSource"/>.)
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     The <see cref="AudioSource"/> does not need to be <see cref="MonoBehaviour.enabled"/> since it just
        ///     need to read the fields out for copy. Also it does not need to be anywhere on the scene,
        ///     it can come from a prefab with <see cref="AudioSource"/> in your project.
        /// </remarks>
        public void ApplyAudioSource(AudioSource applyFrom)
        {
            foreach (var aSource in InternalAudioSources)
            {
                CopyAudioSourceFields(aSource, applyFrom);
            }
        }

        /// <summary>
        /// 各プレーヤーは4つの <see cref="AudioSource"/> を含み、このメソッドはデバッグのために最初のペアの現在の情報を返します。
        /// </summary>
        public string[] GetDebugStringsTrack1()
        {
            return _twoTracks[0].DebugInformation;
        }

        /// <summary>
        /// 各プレーヤーは4つの <see cref="AudioSource"/> を含み、このメソッドはデバッグのために2番目のペアの現在の情報を返します。
        /// </summary>
        public string[] GetDebugStringsTrack2()
        {
            return _twoTracks[1].DebugInformation;
        }

        //
        // Non Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        internal void ApplyVolumeSettingToAllTracks()
        {
            _twoTracks[0].ApplyVolume();
            _twoTracks[1].ApplyVolume();
        }

        //protected static T InstantiateSingletonPlayer<T>(AudioSource templateOfTemplateSource) where T : BgmPlayer
        //{
        //    var type = typeof(T);
        //    var typeString = type.Name;
        //
        //    var newIntroloopPlayerObject = new GameObject(typeString);
        //    var playerComponent = newIntroloopPlayerObject.AddComponent<T>();
        //    var templateAudioSource = newIntroloopPlayerObject.AddComponent<AudioSource>();
        //    playerComponent.TemplateSource = templateOfTemplateSource;
        //    SetupDefaultAudioSourceForIntroloop(templateAudioSource);
        //    if (templateOfTemplateSource != null)
        //    {
        //        CopyAudioSourceFields(templateAudioSource, templateOfTemplateSource);
        //    }
        //
        //    DontDestroyOnLoad(newIntroloopPlayerObject);
        //
        //    playerComponent.SetupChildComponents();
        //
        //    return playerComponent;
        //}

        // 子要素を生成してプレイヤーを使用可能な状態にする
        void SetupChildComponents()
        {
            if (_isSetupChildComponent)
            {
                return;
            }

            // These are all the components that make this plugin works. Basically 4 AudioSources with special control script
            // to juggle music file carefully, stop/pause/resume gracefully while Introloop-ing.

            var musicPlayerTransform = transform;
            var musicTrack1 = new GameObject();
            musicTrack1.AddComponent<IntroloopTrack>();
            musicTrack1.name = "IntroloopTrack 1";
            musicTrack1.transform.parent = musicPlayerTransform;
            musicTrack1.transform.localPosition = Vector3.zero;
            _twoTracks[0] = musicTrack1.GetComponent<IntroloopTrack>();
            //_twoTracks[0].introloopSettings = _introloopSettings;

            var musicTrack2 = new GameObject();
            musicTrack2.AddComponent<IntroloopTrack>();
            musicTrack2.name = "IntroloopTrack 2";
            musicTrack2.transform.parent = musicPlayerTransform;
            musicTrack2.transform.localPosition = Vector3.zero;
            _twoTracks[1] = musicTrack2.GetComponent<IntroloopTrack>();
            //_twoTracks[1].introloopSettings = _introloopSettings;

            _isSetupChildComponent = true;
        }

        static void CopyAudioSourceFields(AudioSource copyTo, AudioSource copyFrom)
        {
            // Pitch is NOT inherited, that could destroy scheduling!
            // Pitch can only be specified via the IntroloopAudio asset file.

            copyTo.outputAudioMixerGroup = copyFrom.outputAudioMixerGroup;

            copyTo.SetCustomCurve(AudioSourceCurveType.CustomRolloff,
                copyFrom.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            copyTo.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix,
                copyFrom.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            copyTo.SetCustomCurve(AudioSourceCurveType.SpatialBlend,
                copyFrom.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
            copyTo.SetCustomCurve(AudioSourceCurveType.Spread,
                copyFrom.GetCustomCurve(AudioSourceCurveType.Spread));

            // Spatial blend, reverb zone mix, and spread must NOT be copied
            // since it will overwrite the curve copy above.

            copyTo.ignoreListenerVolume = copyFrom.ignoreListenerVolume;
            copyTo.ignoreListenerPause = copyFrom.ignoreListenerPause;
            copyTo.velocityUpdateMode = copyFrom.velocityUpdateMode;
            copyTo.panStereo = copyFrom.panStereo;
            // applyTarget.spatialBlend = applyFrom.spatialBlend;
            copyTo.spatialize = copyFrom.spatialize;
            copyTo.spatializePostEffects = copyFrom.spatializePostEffects;
            // applyTarget.reverbZoneMix = applyFrom.reverbZoneMix;
            copyTo.bypassEffects = copyFrom.bypassEffects;
            copyTo.bypassListenerEffects = copyFrom.bypassListenerEffects;
            copyTo.bypassReverbZones = copyFrom.bypassReverbZones;
            copyTo.dopplerLevel = copyFrom.dopplerLevel;
            // applyTarget.spread = applyFrom.spread;
            copyTo.priority = copyFrom.priority;
            copyTo.mute = copyFrom.mute;
            copyTo.minDistance = copyFrom.minDistance;
            copyTo.maxDistance = copyFrom.maxDistance;
        }

        static void SetupDefaultAudioSourceForIntroloop(AudioSource audioSource)
        {
            audioSource.spatialBlend = 0;
            audioSource.priority = 0;
        }

        void FadeUpdate()
        {
            //For two main tracks
            for (var i = 0; i < 2; i++)
            {
                var towardsVolumeBgmVolumeApplied = _towardsVolume[i];
                if (!(Math.Abs(_twoTracks[i].FadeVolume - towardsVolumeBgmVolumeApplied) > 0.0001f))
                {
                    continue;
                }

                // Handles the fade in/out
                if (_fadeLength[i] == 0)
                {
                    _twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                }
                else
                {
                    if (_twoTracks[i].FadeVolume > towardsVolumeBgmVolumeApplied)
                    {
                        _twoTracks[i].FadeVolume -= Time.unscaledDeltaTime / _fadeLength[i];
                        if (_twoTracks[i].FadeVolume <= towardsVolumeBgmVolumeApplied)
                        {
                            //Stop the fade
                            _twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                        }
                    }
                    else
                    {
                        _twoTracks[i].FadeVolume += Time.unscaledDeltaTime / _fadeLength[i];
                        if (_twoTracks[i].FadeVolume >= towardsVolumeBgmVolumeApplied)
                        {
                            //Stop the fade
                            _twoTracks[i].FadeVolume = towardsVolumeBgmVolumeApplied;
                        }
                    }
                }

                //Stop check
                if (_willStop[i] && _twoTracks[i].FadeVolume == 0)
                {
                    _willStop[i] = false;
                    _willPause[i] = false;
                    _twoTracks[i].Stop();
                    UnloadTrack(i);
                }

                //Pause check
                if (_willPause[i] && _twoTracks[i].FadeVolume == 0)
                {
                    _willStop[i] = false;
                    _willPause[i] = false;
                    _twoTracks[i].Pause();
                    //don't unload!
                }
            }
        }

        void UnloadTrack(int trackNumber)
        {
            //Have to check if other track is using the music or not?

            //If playing the same song again,
            //the loading of the next song might come earlier, then got immediately unloaded by this.

            //Also check for when using different IntroloopAudio with the same source file.
            //In this case .Music will be not equal, but actually the audioClip inside is the same song.

            //Note that load/unloading has no effect on "Streaming" audio type.

            var musicEqualCurrent = _twoTracks[trackNumber].Music == _twoTracks[(trackNumber + 1) % 2].Music;
            var clipEqualCurrent = _twoTracks[trackNumber].Music != null &&
                                   _twoTracks[(trackNumber + 1) % 2].Music != null &&
                                   _twoTracks[trackNumber].Music.AudioClip ==
                                   _twoTracks[(trackNumber + 1) % 2].Music.AudioClip;

            //As = AudioSource
            var isSameSongAsCurrent = musicEqualCurrent || clipEqualCurrent;

            var musicEqualNext = _twoTracks[trackNumber].Music == _twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay;
            var clipEqualNext = _twoTracks[trackNumber].Music != null &&
                                _twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay != null &&
                                _twoTracks[trackNumber].Music.AudioClip ==
                                _twoTracks[(trackNumber + 1) % 2].MusicAboutToPlay.AudioClip;

            var isSameSongAsAboutToPlay = musicEqualNext || clipEqualNext;

            var usingAndPlaying = _twoTracks[(trackNumber + 1) % 2].IsPlaying && isSameSongAsCurrent;

            if (!usingAndPlaying && !isSameSongAsAboutToPlay)
            {
                //If not, it is now safe to unload it
                //Debug.Log("Unloading");
                _twoTracks[trackNumber].Unload();
            }
        }

        /// <summary>
        /// ゼロの長さは、ポップ除去の小さなフェードタイムに等しい特別な値です。
        /// 負の長さは、（実際の）0に等しい特別な値です。
        /// </summary>
        static float TranslateFadeLength(float fadeLength)
        {
            return fadeLength > 0 ? fadeLength : fadeLength < 0 ? 0 : _popRemovalFadeTime;
        }

        /// <summary>
        /// セットアップされていなければ例外を投げます。
        /// </summary>
        void ThrowsExceptionIfNotSetup()
        {
            if (!_isSetupChildComponent)
            {
                throw new InvalidOperationException("IntroloopPlayer の子ゲームオブジェクトがまだ初期化されていません。Awake 時に内部 AudioSource にアクセスするのは避けてください。");
            }
        }
    }
}