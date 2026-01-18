//
// (C) 2022 Takap.
//

using System;
using Takap.Utility;
using Takap.Utility.Sound;

namespace UnityEngine.AddressableAssets
{
    //
    // 説明:
    // インスペクターに設定したい Addressables の AssetReference の独自の型を設定した場合
    // ここに固有の型を記述する
    //

    [Serializable]
    public class AssetRefAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetRefAudioClip(string guid) : base(guid)
        {
        }
    }

    [Serializable]
    public class AssetRefIntroAudioClip : AssetReferenceT<BgmAudioClip>
    {
        public AssetRefIntroAudioClip(string guid) : base(guid)
        {
        }
    }
}
