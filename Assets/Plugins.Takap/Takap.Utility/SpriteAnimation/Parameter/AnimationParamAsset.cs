//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="AnimationParamAsset"/> を <see cref="ScriptableObject"/> にしたクラス
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/AnimationParameter")]
    public class AnimationParamAsset : ScriptableObject, IAnimationParam
    {
        //
        // Inspectors
        // - - - - - - - - - - - - - - - - - - - -

        // アセットの名前を使用するかどうかのフラグ
        // true: ScriptableObject.name を使用する / false: AnimationParam.Name を使用する
        //[SerializeField]
        //bool _useAlias = true;

        [SerializeField, InlineProperty, HideLabel]
        AnimationParam _param;

        //
        // Operator
        // - - - - - - - - - - - - - - - - - - - -

        public static implicit operator AnimationParam(AnimationParamAsset value)
        {
            //if (!value._useAlias)
            //{
            //    value._param.Name = value.name;
            //}
            return value._param;
        }

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        //public bool UseAlias => _useAlias;

        //public string Name => _useAlias ? _param.Name : name;
        public string Name => _param.Name;

        public int LoopsCount { get => _param.LoopsCount; set => _param.LoopsCount = value; }

        public float FrameSpeed { get => _param.FrameSpeed; set => _param.FrameSpeed = value; }

        public bool FlipX { get => _param.FlipX; set => _param.FlipX = value; }

        public bool FlipY { get => _param.FlipY; set => _param.FlipY = value; }

        public List<Sprite> SpriteList => _param.SpriteList;
    }
}