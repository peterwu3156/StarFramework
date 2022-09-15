//------------------------------------------------------------
// 脚本名:		MonoAutoSingleton.cs
// 作者:			海星
// 创建时间:		2022/9/15 21:21:47
// 描述:			继承mono的自动生成的单例
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFramework
{
    public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T GetInstance()
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                GameObject.DontDestroyOnLoad(obj);
                //自动创建一个空对象 加一个单例模式脚本
                //切换场景 对象会被删除 所以存在问题
                //所以过场景 要确保不被移除
                //单例模式对象往往是存在整个程序生命周期的
                instance = obj.AddComponent<T>();
            }
            return instance;
        }

    }
}

