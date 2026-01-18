#pragma warning disable IDE0009, IDE1006

//  
// (C) 2023 Takap.
// 

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Takap.Utility.Sound
{
    /// <summary>
    /// <see cref="BgmPlayer"/> で使用するアセットファイルです。
    /// プレーヤが前もって継ぎ目をスケジュールできるように、オーディオをループさせる場所の情報が含まれています。
    /// </summary>
    [CreateAssetMenu(menuName = "Introloop/Introloop Audio")]
    public class BgmAudioClip : ScriptableObject
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // アセットごとに設定できる音量
        [SerializeField] [Range(0, 1)]
        float _volume = 1;
        //[Tooltip(
        //    "通常のAudioClipでは、オーディオごとに音量を変えることはできず、AudioSourceのタスクとなります。" +
        //    "Introloopは4つのAudioSourceをコントロールし、音量もコントロールできる。\r\n" +
        //    "これは作曲の際にも便利で、ジャンルに関係なく、曲をマスターして音量を最大にし、後でミックスすることができます。" +
        //    "ジャンルに関係なく、曲をマスターして音量を最大にし、後でここで適度なレベルにミックスすることができます。")]

        // アセットごとに設定できるピッチ
        [SerializeField] [Range(0.1f, 3)]
        float _pitch = 1;
        //[Tooltip(
        //    "Introloop couldn't change pitch in real time because that will throw off the schedule, " +
        //    "however, by pre-specifying the pitch, it is possible to scales the schedule accordingly. " +
        //    "The audio stitching will still be on time.")]
        //// Trust me, even with non-realtime pitch change it is major PITA to finally
        //// get this working with everything else...
        //[Tooltip(
        //    "イントロループはリアルタイムでピッチを変更することができませんでした。" +
        //    "しかし、ピッチを事前に指定することで、それに応じてスケジュールをスケーリングすることが可能です。" +
        //    "しかし、ピッチを事前に指定することで、スケジュールをそれ に合わせて変更することができます。］
        //// 信じてください、非リアルタイムのピッチ変更でさえ、最終的に
        //// これを他のすべてと連動させるのは、とても大変なことです。


        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [SerializeField] AudioClip _audioClip;

        // Segno
#if UNITY_EDITOR
        [PositiveFloat]
#endif
        [SerializeField] float _introBoundary;

#if UNITY_EDITOR
        [PositiveFloat]
#endif
        [SerializeField] float _loopingBoundary;

        [SerializeField] bool _nonLooping;
        
        [SerializeField] bool _loopWholeAudio;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     このアセットが使用する基礎となる <see cref="AudioClip"/> 。
        /// </summary>
        public AudioClip AudioClip => _audioClip;

        /// <summary>
        ///     このオーディオをループさせずに再生した場合の長さは、アセットに設定されているピッチ修正を考慮済みです。
        ///     (Unityの "pitch "は実際のピッチシフトではなく、再生速度の変化でピッチに影響を与えるだけです)
        /// </summary>
        /// <remarks>
        /// <para>
        ///     <see cref="Length"/> と共に、もしオーディオの Loop が false なら、オーディオはその時間経過後に終了するはずだと予測できます。
        /// </para>
        /// <para>
        ///     これによって、ループしないオーディオを再生していて、
        ///     その後に何か別のオーディオを続けたい場合に、オーディオキューを実装することができます。
        ///     (例えば、寝ている間に宿の音楽を流し、その後、イントロループに設定された古い音楽を続ける)
        /// </para>
        /// <para>
        ///     IsPlaying プロパティのようなものが IntroloopPlayer に欠けているとは思わないかもしれません。
        ///     なぜなら、AudioSource の再生状態を壊す内部のスケジューリングメソッドは、100% 信頼できないからです。
        ///     例えば、スケジュールされたオーディオは、実際には再生されていないにもかかわらず、すでに "再生中 "とみなされます。
        /// </para>
        /// </remarks>
        public float Length => _audioClip.length / _pitch;

        /// <summary>
        ///     エディターでアセットがイントロループかループボタンに設定されている場合、これは `true` になります。
        /// </summary>
        /// <remarks>
        ///     これが `false` の場合、<see cref="Length"/> は再生後に音声がいつ終わるかを予測することができます。
        /// </remarks>
        public bool Loop => !_nonLooping;

        /// <summary>
        ///     再生時に、基礎となる <see cref="AudioSource"/> をこの音量に設定する。
        /// </summary>
        /// <remarks>
        ///     This allows a per-music volume adjustment. The composer can master/maximize the volume from his DAW
        ///     without worry about game's context. The developer can drop the volume down after importing on their own.
        ///     Resulting in a happy game studio.
        /// </remarks>
        public float Volume { get => _volume; set => _volume = value; }

        /// <summary>
        ///     <see cref="BgmAudioClip"/> アセットファイルで設定した読み取り専用のピッチ設定。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Introloop does not allow pitch change other than at the asset, since it will wreck the scheduling if
        ///         that happen while things are playing. Scheduling needs predictability to plan ahead of time and
        ///         pitch change will invalidates many assumptions.
        ///     </para>
        ///     <para>
        ///         (For instance, I schedule "halfway ahead of the seam" and that will not work if suddenly
        ///         you decided to change the pitch right in front of the seam.)
        ///     </para>
        ///     <para>
        ///         Also Unity's "pitch" wording in their <see cref="AudioSource"/> is technically incorrect.
        ///         It is rather "speed" that affects pitch in the end. It is not a real pitch shifting.
        ///         The wording here follows what Unity used rather than technical correctness.
        ///     </para>
        /// </remarks>
        public float Pitch => _pitch;

        /// <summary>
        /// このアセットのループ方法を取得します。
        /// </summary>
        public IntroLoopType LootType
        {
            get
            {
                if (NonLooping)
                {
                    return IntroLoopType.NonLooping;
                }
                else
                {
                    if (LoopWholeAudio)
                    {
                        return IntroLoopType.FullLoop;
                    }
                    else
                    {
                        return IntroLoopType.IntroLoop;
                    }
                }
            }
        }

        // ループするかどうか？
        internal bool NonLooping => _nonLooping;
        // 単純ループかどうか？
        internal bool LoopWholeAudio => _loopWholeAudio;

        /// <summary>
        /// 設定に応じたBGMの終端を取得します。
        /// </summary>
        /// <remarks>
        /// イントロループの時はループの境界、それ以外はBGMの終端を返す。
        /// </remarks>
        public float EndPositon
        {
            get
            {
                switch (LootType)
                {
                    case IntroLoopType.IntroLoop:
                    {
                        return _loopingBoundary / _pitch;
                    }
                    case IntroLoopType.FullLoop:
                    case IntroLoopType.NonLooping:
                    {
                        return Length;
                    }
                    case IntroLoopType.Unknown:
                    default:
                        throw new System.NotSupportedException($"Not supported type. type={LootType}");
                }
            }
        }

        internal float IntroLength => _introBoundary / _pitch;

        internal float LoopingLength => (_loopingBoundary - _introBoundary) / _pitch;

        /// <summary>
        ///     This is for timing the seam between intro and looping section instead of IntroLength on start playing.
        ///     It intentionally does not get divided by pitch. Unity's audio position is not affected by pitch.
        /// </summary>
        internal float UnscaledClipLength => _audioClip.length;

        /// <summary>
        ///     This is for timing the seam between intro and looping section instead of IntroLength on start playing.
        ///     It intentionally does not get divided by pitch. Unity's audio position is not affected by pitch.
        /// </summary>
        internal float UnscaledIntroLength => _introBoundary;

        /// <summary>
        ///     This is for timing the seam between intro and looping section instead of IntroLength on start playing.
        ///     It intentionally does not get divided by pitch. Unity's audio position is not affected by pitch.
        /// </summary>
        internal float UnscaledLoopingLength => _loopingBoundary - _introBoundary;

        internal void Preload() => _audioClip.LoadAudioData();

        internal void Unload() => _audioClip.UnloadAudioData();
    }

    public enum IntroLoopType
    {
        /// <summary>不明な状態(エラー)</summary>
        Unknown,
        /// <summary>イントロループ</summary>
        IntroLoop,
        /// <summary>単純ループ</summary>
        FullLoop,
        /// <summary>ループしない</summary>
        NonLooping,
    }

    #region...

#if UNITY_EDITOR

    /// <summary>
    /// プラスの値のみが設定できることを表す属性です。
    /// </summary>
    internal class PositiveFloatAttribute : PropertyAttribute
    {
        public readonly string Unit;
        public PositiveFloatAttribute() { }
        public PositiveFloatAttribute(string unit) => Unit = unit;
    }

    /// <summary>
    /// <see cref="BgmAudioClip"/> をインスペクターに描画する方法を定義するクラス
    /// </summary>
    [CustomEditor(typeof(BgmAudioClip))]
    internal class IntroloopAssetEditor : Editor
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        int _selected;

        //
        // Runtime impl
        // - - - - - - - - - - - - - - - - - - - -

        public override void OnInspectorGUI()
        {
            var so = serializedObject;
            var volume = so.FindProperty("_volume");
            var pitch = so.FindProperty("_pitch");
            var audioClip = so.FindProperty("_audioClip");
            var introBoundary = so.FindProperty("_introBoundary");
            var loopingBoundary = so.FindProperty("_loopingBoundary");
            var nonLooping = so.FindProperty("_nonLooping");
            var loopWholeAudio = so.FindProperty("_loopWholeAudio");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(audioClip);
            if (audioClip.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("AudioClip を設定してください", MessageType.Warning);
            }

            EditorGUILayout.PropertyField(volume);
            EditorGUILayout.PropertyField(pitch);
            EditorGUILayout.Space();
            if (loopWholeAudio.boolValue)
            {
                _selected = 1;
            }
            else if (nonLooping.boolValue)
            {
                _selected = 2;
            }
            else
            {
                _selected = 0;
            }

            _selected = GUILayout.SelectionGrid(_selected, new[] { "Introloop", "Loop", "Non looping" }, 3, EditorStyles.miniButton);
            switch (_selected)
            {
                case 0:
                {
                    loopWholeAudio.boolValue = false;
                    nonLooping.boolValue = false;
                    break;
                }
                case 1:
                {
                    loopWholeAudio.boolValue = true;
                    nonLooping.boolValue = false;
                    break;
                }
                case 2:
                {
                    loopWholeAudio.boolValue = false;
                    nonLooping.boolValue = true;
                    break;
                }
            }

            EditorGUI.BeginDisabledGroup(_selected != 0);
            EditorGUILayout.BeginHorizontal();
            var halfWidth = EditorGUIUtility.currentViewWidth / 2;
            EditorGUILayout.LabelField("Intro Boundary", GUILayout.Width(halfWidth - 12));
            EditorGUILayout.LabelField("Looping Boundary", GUILayout.Width(halfWidth + 12));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(introBoundary, new GUIContent(""));
            EditorGUILayout.PropertyField(loopingBoundary, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            if (_selected == 0)
            {
                if (loopingBoundary.floatValue - introBoundary.floatValue < 1)
                {
                    EditorGUILayout.HelpBox("2つの境界値が近すぎます。", MessageType.Error);
                }
            }

            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
            {
                if (introBoundary.floatValue > loopingBoundary.floatValue)
                {
                    loopingBoundary.floatValue = introBoundary.floatValue;
                }

                so.ApplyModifiedProperties();
            }
        }
    }

    /// <summary>
    /// プラスしか設定できない float 型のを定義します。
    /// </summary>
    [CustomPropertyDrawer(typeof(PositiveFloatAttribute))]
    internal class PositiveFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            var attr = (PositiveFloatAttribute)attribute;
            var modifiedLabel = label.text;
            if (attr.Unit != null)
            {
                modifiedLabel += " (" + attr.Unit + ")";
            }

            EditorGUI.BeginChangeCheck();
            var val = EditorGUI.FloatField(position, modifiedLabel, prop.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (val < 0)
                {
                    val = 0;
                }
                prop.floatValue = val;
            }
        }
    }

#endif

    #endregion
}