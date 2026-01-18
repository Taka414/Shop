//
// (C) 2022 Takap.
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Dictionary{TKey, TValue}"/> をシリアライズ可能にするための定義
    /// </summary>
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < keyData.Count && i < valueData.Count; i++)
            {
                this[keyData[i]] = valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> item in this)
            {
                keyData.Add(item.Key);
                valueData.Add(item.Value);
            }
        }
    }

    [Serializable] public class CharSpriteDictionary : SerializableDictionary<char, Sprite> { }
    [Serializable] public class StringSpriteDictionary : SerializableDictionary<string, Sprite> { }
    [Serializable] public class StringPlayableAssetDictionary : SerializableDictionary<string, PlayableAsset> { }
}