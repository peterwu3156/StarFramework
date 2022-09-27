//------------------------------------------------------------
// 脚本名:		PoolObject.cs
// 作者:			海星
// 创建时间:		2022/9/27 13:59:18
// 描述:			#对象池实体
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFramework.Runtime
{
    public class BasePool
    {

    }

    public class PoolObject<T> : BasePool
    {
        public Queue<T> objectList;

        public PoolObject(int size)
        {
            objectList = new Queue<T>(size);
        }

        public void AddObject(T obj)
        {
            objectList.Enqueue(obj);
        }

        public T GetObject()
        {
            return objectList.Dequeue();
        }

        public void ClearPool()
        {
            objectList.Clear();
        }
    }
}
