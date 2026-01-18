//
// (C) 2023 Takap.
//

using UnityEngine;
using TMPro;

namespace Takap.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayingTimeUpdater : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        TextMeshProUGUI _text;

        //
        // Props & Events
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Unity Impl
        // - - - - - - - - - - - - - - - - - - - -

        private void Awake()
        {
            this.SetComponent(ref _text);
            //this.SubscriveGlobalSafe<PlayHead>(OnChangeText);
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void OnChangeText(float playHeadTime, float totalTime)
        {
            _text.SetText("Time={0:2} / Total={1:2}", playHeadTime, totalTime);
        }
    }
}
