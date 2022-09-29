//------------------------------------------------------------
// 脚本名:		EventBase.cs
// 作者:			海星
// 创建时间:		2022/9/29 8:53:56
// 描述:			事件基类
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFramework.Runtime
{
    public interface I_EventBase
    {

    }

    public class EventBase<T> : I_EventBase
    {
        public Action<T> actions;

        public EventBase(Action<T> action)
        {
            actions += action;
        }
    }


}
