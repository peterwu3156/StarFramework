//------------------------------------------------------------
// 脚本名:    ScriptGenerator.cs
// 作者:      海星
// 创建时间:  2022/8/31 20:41:46
// 描述:      脚本生成器 脚本生成相关
//------------------------------------------------------------

using UnityEditor;
using System.IO;
using UnityEngine;
using System.Text;

namespace StarFramework.Editor
{
    /// <summary>
    /// 脚本生成相关
    /// </summary>
    public class ScriptGenerator : EditorWindow
    {
        private static string monoPath = "Assets/StarEditor/CreateScripts/NewMonoScript.cs.txt";
        private static string editorPath = "Assets/StarEditor/CreateScripts/NewEditorScript.cs.txt";
        private static string runtimePath = "Assets/StarEditor/CreateScripts/NewRuntimeScript.cs.txt";

        private static string nowPath = string.Empty;
        private static string nowTemplate = string.Empty;

        private static string fileName = string.Empty;
        private static readonly string authorName = "海星";

        [MenuItem("Assets/从模板创建脚本/继承Mono")]
        public static void CreateMonoScript()
        {
            nowPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]) + "\\NewMonoScript.cs";
            nowTemplate = monoPath;
            ShowWindow("创建继承Mono脚本");
        }

        [MenuItem("Assets/从模板创建脚本/编辑器拓展")]
        public static void CreateEditorScript()
        {
            nowPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]) + "\\NewEditorScript.cs";
            nowTemplate = editorPath;
            ShowWindow("创建编辑器拓展脚本");
        }

        [MenuItem("Assets/从模板创建脚本/框架运行时")]
        public static void CreateRuntimeScript()
        {
            nowPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]) + "\\NewRuntimeScript.cs";
            nowTemplate = runtimePath;
            ShowWindow("创建框架运行时脚本");
        }

        private static void ShowWindow(string name)
        {
            var window = GetWindow<ScriptGenerator>(name);
            window.position = new Rect((Screen.width / 2) - 150, (Screen.height / 2) + 50, 300, 50);
            window.maxSize = new Vector2(300, 50);
            window.maximized = true;
            window.Show();
        }

        private static void CloseWindow()
        {
            var window = GetWindow<ScriptGenerator>();
            window.Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("给文件命名", GUILayout.Width(100f));
            fileName = EditorGUILayout.TextField(fileName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("创建"))
            {
                CreateScript(nowTemplate, nowPath, fileName);
                CloseWindow();
            }

            if (GUILayout.Button("取消"))
            {
                CloseWindow();
            }
            EditorGUILayout.EndHorizontal();

        }

        public static void CreateScript(string path, string targetPath, string fileName)
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                content = content.Replace("#AUTHORNAME#", authorName);
                content = content.Replace("#SCRIPTNAME#", fileName);
                content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString());
                using (var newFile = File.Create(targetPath))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(content);
                    newFile.Write(buffer, 0, buffer.Length);
                }
                AssetDatabase.Refresh();
                AssetDatabase.RenameAsset(targetPath, fileName + ".cs");
            }
            else
            {
                Debug.LogError("脚本模板文件丢失");
            }
        }
    }
}
