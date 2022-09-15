//------------------------------------------------------------
// 脚本名:		ABPackWindow.cs
// 作者:			海星
// 创建时间:		2022/9/4 23:24:03
// 描述:			AB包打包工具
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System;


namespace StarFramework.Editor
{
    public class ABPackWindow : EditorWindow
    {
        [MenuItem("星框架/AB包打包工具")]
        static void OpenWindow()
        {
            var window = GetWindow<ABPackWindow>("AB包打包工具");
            window.maximized = true;
            window.maxSize = new Vector2(600, 600);
            window.position = new Rect(100, 100, 600, 600);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("运行时需要加载的资源需要分类好放在ArtRes文件夹下，框架会自动读取" + Environment.NewLine +
                "统计单文件夹内所有资源大小时，meta文件也是被计算在内的" + Environment.NewLine +
                "ArtRes文件夹目前最多不能超过一级 也就是说文件夹里只允许存放文件", MessageType.Info);
            EditorGUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("优化手册"))
            {
                System.Diagnostics.Process.Start("https://starfishpeter.cn/archives/8620");
            }

            if (GUILayout.Button("快速打包"))
            {
                string resPath = "Assets/ArtRes";
                if (Directory.Exists(resPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(resPath);
                    DirectoryInfo[] folders = directoryInfo.GetDirectories();

                    foreach (var folder in folders)
                    {
                        RapidPack(folder.FullName);
                    }

                    EditorUtility.DisplayDialog("打包完成", $"已打包{folders.Length}个文件夹下的文件", "确定");
                    AssetDatabase.Refresh();
                }

            }
            EditorGUILayout.EndHorizontal();

            LoadArtResAssets();
        }

        private void LoadArtResAssets()
        {
            string resPath = "Assets/ArtRes";
            if (Directory.Exists(resPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(resPath);
                DirectoryInfo[] folders = directoryInfo.GetDirectories();

                foreach (var folder in folders)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(folder.Name);

                    FileInfo[] files = new DirectoryInfo(folder.FullName).GetFiles("*", SearchOption.AllDirectories);
                    int filesNumber = 0;
                    float filesSize = 0;
                    int outSizeNumber = 0;
                    foreach (var file in files)
                    {
                        filesNumber++;
                        if (file.Length >= 5 * 1000 * 1000)
                        {
                            outSizeNumber++;
                        }
                        filesSize += file.Length;
                    }

                    filesSize /= 1000;
                    if (filesSize / 1000 < 1)
                    {
                        EditorGUILayout.LabelField($"共有{filesNumber}个文件,大小合计为{Math.Round(filesSize, 2)}KB");
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"共有{filesNumber}个文件,大小合计为{Math.Round(filesSize / 1000, 2)}MB");
                    }
                    EditorGUILayout.EndHorizontal();

                    if (outSizeNumber > 0)
                    {
                        EditorGUILayout.HelpBox($"检测到该文件夹下有{outSizeNumber}个文件超过5MB 最好进行优化", MessageType.Warning);
                    }
                }
            }
        }

        private void RapidPack(string path)
        {
            if (Directory.Exists(path))
            {
                AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
                string nowFolderName = path.Substring(path.LastIndexOf('\\') + 1);
                buildMap[0].assetBundleName = nowFolderName;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);

                string[] assetNames = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    string fullPath = files[i].FullName;
                    string relativePath = fullPath.Substring(fullPath.IndexOf("Assets"));
                    relativePath = relativePath.Replace('\\', '/');
                    assetNames[i] = relativePath;
                }

                buildMap[0].assetNames = assetNames;

                BuildPipeline.BuildAssetBundles("Assets/ABPack", buildMap, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
            }
        }
    }
}
