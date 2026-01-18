//
// (C) 2022 Takap.
// 

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Takap.Utility.Sound
{
    //
    // 1フレームに再生する効果音の数を1種類1つまでに制限する
    //  → 同じタイミングで同じSEを大量にPlayOneShotして爆音になるのを避ける
    //
    // すごく短い時間に要求された効果音はスケジュールして別のフレームで再生する
    //  → それでも最大数が存在する
    //

    /// <summary>
    /// 効果音を再生するためのクラス
    /// </summary>
    //public class SEPlayer : SingletonMonobehaviour<SEPlayer>
    public class SEPlayer : MonoBehaviour, ISePlayer
    {
        //
        // Const
        // - - - - - - - - - - - - - - - - - - - -

        // デフォルトのチャンネル名
        public const string KEY_DEFAULT = "Any";

        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        [ShowInInspector] float _delayTime = 0.034f;
        [ShowInInspector] int _maxQueudItemCount = 3;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // 再生するコンポーネント
        AudioSource _audioSource;

        // SEごとの管理テーブル
        readonly Dictionary<string, Queue<_SEInfo>> _table = new Dictionary<string, Queue<_SEInfo>>();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 効果音の音量を設定または取得します。
        /// </summary>
        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = Mathf.Clamp01(value);
        }

        /// <summary>
        /// 同じ音が同時再生数以上リクエストされてキューされた時に次に効果音の再生を遅延させるフレーム数を設定または取得します。
        /// </summary>
        public float DelayFrame { get => _delayTime; set => _delayTime = value; }

        /// <summary>
        /// 再生予定キューに登録できる最大数を設定または取得します。
        /// </summary>
        /// <remarks>
        /// これ以上登録すると再生されずに無視されます。
        /// </remarks>
        public int MaxQueueItemCount { get => _maxQueudItemCount; set => _maxQueudItemCount = value; }

        /// <summary>
        /// このオブジェクトが使用している<see cref="AudioSource"/> を取得します。
        /// </summary>
        public AudioSource AudioSource => _audioSource;

        /// <summary>
        /// オーディオミキサーを設定します。
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get => _audioSource.outputAudioMixerGroup;
            set => _audioSource.outputAudioMixerGroup = value;
        }

        //
        // Runtimes
        // - - - - - - - - - - - - - - - - - - - -

        void Awake()
        {
            this.SetComponent(ref _audioSource);
        }

        void Update()
        {
            foreach (Queue<_SEInfo> queue in _table.Values)
            {
                if (queue.Count == 0)
                {
                    continue;
                }

                while (true)
                {
                    if (queue.Count == 0)
                    {
                        break;
                    }

                    if (queue.Peek().IsDone)
                    {
                        _SEInfo _ = queue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }

                if (queue.Count == 0)
                {
                    continue;
                }

                // キューの先頭で未再生の効果音を再生する
                _SEInfo info = queue.Peek();
                info.Elapsed += Time.deltaTime;
                if (info.Elapsed > _delayTime)
                {
                    _audioSource.PlayOneShot(info.AudioClip);
                    queue.Dequeue();
                }
            }

            // 全部再生し終わったらループを停止する
            if (GetSECount() == 0)
            {
                _table.Clear();
                enabled = false;
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// デフォルトチャンネルで効果音を再生します。
        /// </summary>
        public void PlaySE(AudioClip clip)
        {
            PlaySE(KEY_DEFAULT, clip);
        }

        /// <summary>
        /// 効果音を再生します。
        /// </summary>
        public void PlaySE(string name, AudioClip clip)
        {
            enabled = true;

            var info = new _SEInfo(name, clip);

            if (!_table.ContainsKey(name))
            {
                _audioSource.PlayOneShot(clip);
                info.IsDone = true;

                var q = new Queue<_SEInfo>();
                q.Enqueue(info);
                _table[name] = q;
            }
            else
            {
                Queue<_SEInfo> queue = _table[name];
                if (queue.Count == 0)
                {
                    _audioSource.PlayOneShot(clip);
                    info.IsDone = true;
                    queue.Enqueue(info);
                }
                if (queue.Count < _maxQueudItemCount)
                {
                    queue.Enqueue(info);
                }
                else
                {
                    Log.Debug($"効果音の最大登録数を超えています。name={name}");
                }
            }
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 有効な要素数を取得します。
        /// </summary>
        int GetSECount()
        {
            int num = 0;
            foreach (Queue<_SEInfo> list in _table.Values)
            {
                num += list.Count;
            }
            return num;
        }

        //
        // InnerTypes
        // - - - - - - - - - - - - - - - - - - - -

        // 管理対象の効果音の情報
        public class _SEInfo
        {
            // クリップの名前
            public readonly string Name;
            // 再生する音
            public readonly AudioClip AudioClip;

            // 再生済みかどうかのフラグ。true : 再生済み / false : まだ
            public bool IsDone { get; set; }
            // 再生候補になってからの経過時間
            public float Elapsed { get; set; }

            public _SEInfo(string name, AudioClip clip)
            {
                Name = name;
                AudioClip = clip;
            }
        }
    }
}