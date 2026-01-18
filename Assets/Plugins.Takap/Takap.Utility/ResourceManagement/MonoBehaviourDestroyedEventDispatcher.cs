////
//// (C) 2022 Takap.
////

//using System;
//using UnityEngine;

//namespace Takap.Utility
//{
//    /// <summary>
//    /// ゲームオブジェクトが破棄された時のイベントを通知します。
//    /// </summary>
//    public class MonoBehaviourDestroyedEventDispatcher : MonoBehaviour, IEventDispatcher
//    {
//        public event Action OnDispatch;

//        public void OnDestroy()
//        {
//            OnDispatch?.Invoke();
//            OnDispatch = null;
//        }
//    }
//}
