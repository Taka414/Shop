//
// (C) 2022 Takap.
//

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// タイルレイヤー全体を制御します。
    /// </summary>
    public class TileLayer : MonoBehaviour
    {
        public IEnumerable<ZIndexUpdaterTile_V2> GetChildsZi() => this.GetChilds<ZIndexUpdaterTile_V2>();

        [SerializeField, LabelText("テンプレート表示"), OnValueChanged(nameof(OnIsDisplayTemplateChanged)),]
        private bool _isDisplayTemplate;

        /// <summary>
        /// <see cref="_isDisplayTemplate"/> を更新したときに発生するイベント
        /// </summary>
        private void OnIsDisplayTemplateChanged()
        {
            foreach (ZIndexUpdaterTile_V2 c in this.GetChilds<ZIndexUpdaterTile_V2>())
            {
                c.IsDisplayTemplate = _isDisplayTemplate;
            }
        }

        [Button("名前を更新")]
        public void Rename()
        {
            foreach (ZIndexUpdaterTile_V2 c in this.GetChilds<ZIndexUpdaterTile_V2>())
            {
                Transform t = c.transform;
                c.gameObject.name = $"Tile_({t.GetLocalPosX()}, {t.GetLocalPosY()})";
            }
        }

        [Button("ランダム化")]
        public void RandomOffset()
        {
            GetChildsZi().ForEach(c => c.Random());
        }

        [Button("オフセットを全てリセットする")]
        public void ResetOffset()
        {
            GetChildsZi().ForEach(c => c.ResetOffset());
        }
    }
}