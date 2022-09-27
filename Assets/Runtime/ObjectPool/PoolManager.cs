//------------------------------------------------------------
// 脚本名:		PoolManager.cs
// 作者:			海星
// 创建时间:		2022/9/27 14:47:46
// 描述:			#对象池管理器
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarFramework.Runtime
{
    public class PoolManager : SingletonAutoMono<PoolManager>
    {
        public Dictionary<string, BasePool> poolDic = new Dictionary<string, BasePool>();

        public PoolObject<T> GetPool<T>(string name)
        {
            if (!poolDic.ContainsKey(name))
            {
                var pool = new PoolObject<T>(20);
                poolDic.Add(name, pool);
                return pool;
            }

            return poolDic[name] as PoolObject<T>;
        }

        public void AddObject<T>(string name, T obj)
        {
            GetPool<T>(name).AddObject(obj);
        }

        public T GetObject<T>(string name)
        {
            var pool = GetPool<T>(name);
            if (pool.objectList.Count <= 0)
            {
                poolDic.Remove(name);
                return default;
            }

            return pool.GetObject();
        }

        private void OnDestroy()
        {
            poolDic.Clear();
        }
    }
}
