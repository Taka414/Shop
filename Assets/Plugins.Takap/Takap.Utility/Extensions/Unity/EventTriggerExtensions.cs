//
// (C) 2022 Takap.
//

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="EventTrigger"/> の機能を拡張します。
    /// </summary>
    public static class EventTriggerExtensions
    {
        /// <summary>
        /// 指定したイベントタイプでコールバックを登録します。
        /// </summary>
        public static void Add(this EventTrigger self, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry()
            {
                eventID = type,
            };
            entry.callback.AddListener(callback);
            self.triggers.Add(entry);
        }

        /// <summary>
        /// 現在設定しているすべてのコールバックをクリアします。
        /// </summary>
        public static void Clear(this EventTrigger self) => self.triggers.Clear();
    }
}