//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Takap.Utility
{
    /// <summary>
    /// uGUIにFlip機能を追加するコンポーネント
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class FlipImage : UIBehaviour, IMeshModifier
    {
        // 左右を反転するかどうか
        [SerializeField] bool _flipX;
        // 上下を反転するかどうか
        [SerializeField] bool _flipY;

        readonly List<UIVertex> _vertexList = new();
        RectTransform _rectTransform;

#if UNITY_EDITOR
        public new void OnValidate()
        {
            GetComponent<Graphic>().SetVerticesDirty();
            Awake();
        }
#endif

        //[ShowInInspector]
        public bool FlipX
        {
            get => _flipX;
            set
            {
                if (_flipX == value) return;

                _flipX = value;
                GetComponent<Graphic>().SetVerticesDirty();
                //this.SetDirty();
            }
        }

        //[ShowInInspector]
        public bool FlipY
        {
            get => _flipY;
            set
            {
                if (_flipY == value) return;

                _flipY = value;
                GetComponent<Graphic>().SetVerticesDirty();
                //this.SetDirty();
            }
        }

        public void SetFlipXY(bool isFlip)
        {
            if (_flipX == _flipY && _flipX == isFlip) return;

            _flipX = isFlip;
            _flipY = isFlip;
            GetComponent<Graphic>().SetVerticesDirty();
        }

        protected override void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        public void ModifyMesh(Mesh mesh)
        {
            // nop
        }

        public void ModifyMesh(VertexHelper verts)
        {
            _vertexList.Clear();
            verts.GetUIVertexStream(_vertexList);
            for (var i = 0; i < _vertexList.Count; i++)
            {
                // pivotの位置によってずらす
                UIVertex vertex = _vertexList[i];
                if (_flipX)
                {
                    vertex.position.x += Mathf.Lerp(-_rectTransform.rect.width, _rectTransform.rect.width, _rectTransform.pivot.x);
                    vertex.position.x *= -1;
                }

                if (_flipY)
                {
                    vertex.position.y += Mathf.Lerp(-_rectTransform.rect.height, _rectTransform.rect.height, _rectTransform.pivot.y);
                    vertex.position.y *= -1;
                }

                _vertexList[i] = vertex;
            }

            verts.Clear();
            verts.AddUIVertexTriangleStream(_vertexList);
        }
    }
}
