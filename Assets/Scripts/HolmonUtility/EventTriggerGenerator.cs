namespace HolmonUtility
{
    using System;
    using UnityEngine.EventSystems;

    public static class EventTriggerGenerator
    {
        /// <summary>
        /// EventTriggerにEventを登録する際に必要なEventTrigger.Entry型を返します
        /// </summary>
        /// <param name="eventType">登録したいイベントのタイプ</param>
        /// <returns></returns>
        public static EventTrigger.Entry Generate(EventTriggerType type, Action callback)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { callback(); });
            return entry;
        }

    }
}