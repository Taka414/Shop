//
// (C) 2023 Takap.
//

using TMPro;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayingLabelUpdater : MonoBehaviour
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
            //this.SubscriveGlobalSafe<ChangeBgm>(OnChangeText);
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void OnChangeText(string name)
        {
            _text.text = name;
        }
    }
}
