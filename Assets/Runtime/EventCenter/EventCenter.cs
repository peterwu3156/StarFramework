//------------------------------------------------------------
// 脚本名:		EventCenter.cs
// 作者:			海星
// 创建时间:		2022/9/29 8:58:46
// 描述:			#
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFramework.Runtime
{
    public class EventCenter : SingletonAutoMono<EventCenter>
    {
        private Dictionary<string, I_EventBase> eventDic = new Dictionary<string, I_EventBase>();

        public void AddEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventBase<T>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventBase<T>(action));
            }
        }

        public void RemoveEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventBase<T>).actions -= action;
            }
        }

        public void EventTrigger<T>(string name, T info)
        {
            if (eventDic.ContainsKey(name) && (eventDic[name] as EventBase<T>).actions != null)
            {
                (eventDic[name] as EventBase<T>).actions.Invoke(info);
            }
        }

        public void Clear()
        {
            eventDic.Clear();
        }
    }
}
