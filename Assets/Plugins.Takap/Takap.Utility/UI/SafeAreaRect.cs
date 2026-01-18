//
// (C) 2022 Takap.
//

using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// アタッチしたUI要素を常に SafeArea と同じ大きさに維持する機能を表します。
    /// </summary>
    [RequireComponent(typeof(RectTransform)), ExecuteAlways]
    public class SafeAreaRect : MonoBehaviour, IAwake
    {
        //
        // 使い方:
        // このコンポーネントを Canvas 直下に配置した UI 要素に追加するだけ
        //
        // 参考:
        // https://tatsuya-koyama.com/articles/gamedev/mobile-game-displays/#%E4%BD%99%E8%AB%87-unity-%E3%81%AE-device-simulator
        // 
        // カスみたいなゴミシュールブログの馬鹿の記事ばっかり引っかかるけど
        // こっちの方が億倍有益なので何かあったらここを参考にすること
        // 

        private RectTransform _rect;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

#if UNITY_EDITOR
        private bool _isUpdateAlways = true;
#else
        private bool _isUpdateAlways = false;
#endif

        //
        // Rintime impl
        // - - - - - - - - - - - - - - - - - - - -

        public void LocalAwake()
        {
            this.SetComponent(ref _rect);
            enabled = true;
        }

        private void Update()
        {
            UpdateSafeArea();

            if (!_isUpdateAlways)
            {
                enabled = false;
            }
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// Odin からのデバッグ用。一回だけ Update を呼び出すようにしてサイズを更新します。
        /// たまに表示がおかしくなるのでそうなった時に押す。
        /// </summary>
        [Button]
        public void Fit()
        {
            _lastSafeArea = new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// このスクリプトがアタッチされている要素の大きさをセーフエリアの大きさに変更します。
        /// </summary>
        public void UpdateSafeArea()
        {
            Rect safeArea = Screen.safeArea;
            if (safeArea == _lastSafeArea)
            {
                return;
            }

            _lastSafeArea = safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            if (_rect is null)
            {
                this.SetComponent(ref _rect);
            }
            _rect.anchorMin = anchorMin;
            _rect.anchorMax = anchorMax;
        }
    }
}