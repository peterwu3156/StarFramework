//------------------------------------------------------------
// 脚本名:		ResourceManager.cs
// 作者:			海星
// 创建时间:		2022/9/15 21:31:31
// 描述:			#
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StarFramework.Runtime
{
    public class ResourceManager
    {
        //AB包不能重复加载 所以这里用字典来存储加载过的AB包
        private Dictionary<string, AssetBundle> ABCache = new Dictionary<string, AssetBundle>();


    }
}
