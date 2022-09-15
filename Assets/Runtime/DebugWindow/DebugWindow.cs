//------------------------------------------------------------
// 脚本名:		DebugWindow.cs
// 作者:			海星
// 创建时间:		2022/9/4 23:26:55
// 描述:			游戏运行的debug窗口
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StarFramework.Runtime
{
    public class DebugWindow : MonoBehaviour
    {
        private Vector2 scrollPosition;
        private bool windowOpen = false;

        private float fpsCounter = 0;
        private float interval = 0;
        private float fps = 0;

        private int firthMenuIndex = 0;
        private int secondMenuIndex = 0;

        List<string> logList = new List<string>(50);


        void Start()
        {
            Debug.Log("加载");
            Application.logMessageReceived += HandleLog;
        }

        void Update()
        {

            //这块逻辑还是要拿出去 
            fpsCounter++;
            interval += Time.deltaTime;
            if (interval > 0.5f)
            {
                fps = fpsCounter / interval;
                Debug.Log("当前帧率" + fps);
                interval = 0;
                fpsCounter = 0;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width * 2 / 3, 0, 80, 60));
            if (GUILayout.Button("fps:" + Mathf.Ceil(fps).ToString()))
            {
                windowOpen = !windowOpen;
            }
            GUILayout.EndArea();

            if (windowOpen)
            {
                GUILayout.Window(0, new Rect(10, 10, 640, 360), ConsoleWindow, "调试窗口");
            }
        }

        void ConsoleWindow(int windowID)
        {
            string[] testMenu = new string[] { "日志窗口", "GM窗口" };
            firthMenuIndex = GUILayout.SelectionGrid(firthMenuIndex, testMenu, 4);

            switch (firthMenuIndex)
            {
                case 0:
                    ShowLogs();
                    break;
                case 1:
                    break;
            }
        }

        void ShowLogs()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (string log in logList)
            {
                GUILayout.Label(log);
            }

            GUILayout.EndScrollView();
        }

        void HandleLog(string logText, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    logList.Add($"【错误】[{DateTime.Now:T}] {logText}");
                    break;
                case LogType.Warning:
                    logList.Add($"【警告】[{DateTime.Now:T}] {logText}");
                    break;
                case LogType.Log:
                    logList.Add($"【日志】[{DateTime.Now:T}] {logText}");
                    break;
            }
            scrollPosition += new Vector2(0, 20);
        }
    }
}
