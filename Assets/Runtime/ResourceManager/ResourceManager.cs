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
    public class AssetObject
    {
        private int refCount;
        public AssetBundle asset;

        public AssetObject(AssetBundle asset)
        {
            refCount = 1;
            this.asset = asset;
        }

        public static AssetObject LoadFromFile(string path)
        {
            AssetObject newAsset = new AssetObject(AssetBundle.LoadFromFile(path));
            return newAsset;
        }

        public T LoadAsset<T>(string name) where T : Object
        {
            refCount++;
            return asset.LoadAsset<T>(name);
        }

        public AssetBundleRequest LoadAssetAsync<T>(string name) where T : Object
        {
            refCount++;
            return asset.LoadAssetAsync<T>(name);
        }

        public void Unload()
        {
            refCount--;
            if (refCount == 0)
            {
                //这边还要加上缓存机制
                asset.Unload(true);
            }
        }

    }

    public class ResourceManager : SingletonAutoMono<ResourceManager>
    {
        //AB包不能重复加载 所以这里用字典来存储加载过的AB包
        private static Dictionary<string, AssetObject> ABCache = new Dictionary<string, AssetObject>();

        private static string packPath = "Assets/StreamingAssets/";
        private AssetObject mainAB = null;
        private AssetBundleManifest manifest = null;

        private void LoadDependencies(string ABName)
        {
            AssetObject ab = null;

            if (mainAB == null)
            {
                mainAB = AssetObject.LoadFromFile(packPath + "StreamingAssets");
                manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            //加载依赖包
            string[] strs = manifest.GetAllDependencies(ABName);
            for (int i = 0; i < strs.Length; i++)
            {
                if (!ABCache.ContainsKey(strs[i]))
                {
                    AssetObject abDepend = AssetObject.LoadFromFile(packPath + strs[i]);
                    ABCache.Add(strs[i], abDepend);
                }
            }

            //加载目标资源包
            if (!ABCache.ContainsKey(ABName))
            {
                ab = AssetObject.LoadFromFile(packPath + ABName);
                ABCache.Add(ABName, ab);
            }
        }

        public T LoadResSync<T>(string ABName, string resName) where T : Object
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

        public void LoadResAsync<T>(string ABName, string resName, UnityAction<T> callBack) where T : Object
        {
            StartCoroutine(ReallyLoadResAsync<T>(ABName, resName, callBack));
        }

        private IEnumerator ReallyLoadResAsync<T>(string ABName, string resName, UnityAction<T> callBack) where T : Object
        {
            LoadDependencies(ABName);

            AssetBundleRequest abRequest = ABCache[ABName].LoadAssetAsync<T>(resName);
            yield return abRequest;

            if (abRequest.asset.GetType() == typeof(GameObject))
            {
                callBack(Instantiate(abRequest.asset) as T);
            }
            else
            {
                callBack(abRequest.asset as T);
            }
        }

        public void UnLoad(string ABName)
        {
            if (ABCache.ContainsKey(ABName))
            {
                ABCache[ABName].Unload();
                ABCache.Remove(ABName);
            }
        }

        public void ClearABCache()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            ABCache.Clear();
            mainAB = null;
            manifest = null;
        }
    }
}

