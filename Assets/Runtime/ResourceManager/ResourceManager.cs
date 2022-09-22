//------------------------------------------------------------
// 脚本名:		ResourceManager.cs
// 作者:			海星
// 创建时间:		2022/9/15 21:31:31
// 描述:			运行时资源管理器
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StarFramework.Runtime
{
    public class ResourceManager : SingletonAutoMono<ResourceManager>
    {
        //AB包不能重复加载 所以这里用字典来存储加载过的AB包
        private static Dictionary<string, AssetBundle> ABCache = new Dictionary<string, AssetBundle>();

        private static string packPath = "Assets/StreamingAssets/";
        public AssetBundle mainAB = null;
        public AssetBundleManifest manifest = null;

        public void LoadDependencies(string ABName)
        {
            AssetBundle ab = null;

            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(packPath + "StreamingAssets");
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            //加载依赖包
            string[] strs = manifest.GetAllDependencies(ABName);
            for (int i = 0; i < strs.Length; i++)
            {
                if (!ABCache.ContainsKey(strs[i]))
                {
                    AssetBundle abDepend = AssetBundle.LoadFromFile(packPath + strs[i]);
                    ABCache.Add(strs[i], abDepend);
                }
            }

            //加载目标资源包
            if (!ABCache.ContainsKey(ABName))
            {
                ab = AssetBundle.LoadFromFile(packPath + ABName);
                ABCache.Add(ABName, ab);
            }
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ABName"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public T LoadRes<T>(string ABName, string resName) where T : Object
        {
            LoadDependencies(ABName);

            T obj = ABCache[ABName].LoadAsset<T>(resName);
            if (typeof(T) == typeof(GameObject))
            {
                return Instantiate(obj);
            }
            else
            {
                return obj;
            }
        }
    }
}
