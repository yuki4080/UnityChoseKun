﻿namespace Utj.UnityChoseKun
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    [System.Serializable]
    public class TimeView
    {
        [SerializeField]TimeKun m_timeKun;
        TimeKun timeKun {
            get {if(m_timeKun == null){m_timeKun = new TimeKun();}return m_timeKun;}
            set {m_timeKun = value;}
        }

        bool isDone = false;

        public void OnGUI()
        {            
            if(isDone == false){
                EditorGUILayout.HelpBox("Please Pull Request.",MessageType.Info);
            } else {
                using (new EditorGUI.IndentLevelScope()){
                    EditorGUILayout.FloatField("deltaTime", timeKun.deltaTime);
                    EditorGUILayout.FloatField("fixedUnscaledDeltaTime", timeKun.fixedUnscaledDeltaTime);
                    EditorGUILayout.FloatField("fixedUnscaledTime", timeKun.fixedUnscaledTime);
                    EditorGUILayout.IntField("frameCount", timeKun.frameCount);
                    EditorGUILayout.FloatField("frameCount", timeKun.frameCount);
                    EditorGUILayout.FloatField("realtimeSinceStartup", timeKun.realtimeSinceStartup);
                    EditorGUILayout.FloatField("smoothDeltaTime", timeKun.smoothDeltaTime);
                    EditorGUILayout.FloatField("time", timeKun.time);
                    EditorGUILayout.FloatField("timeSinceLevelLoad", timeKun.timeSinceLevelLoad);
                    EditorGUILayout.FloatField("unscaledDeltaTime", timeKun.unscaledDeltaTime);
                    EditorGUILayout.FloatField("unscaledTime", timeKun.unscaledTime);
                    EditorGUILayout.Toggle("inFixedTimeStep", timeKun.inFixedTimeStep);

                    EditorGUILayout.Space();

                    timeKun.captureFramerate = EditorGUILayout.IntField("captureFramerate", timeKun.captureFramerate);
                    timeKun.fixedDeltaTime = EditorGUILayout.FloatField("fixedDeltaTime", timeKun.fixedDeltaTime);
                    timeKun.maximumDeltaTime = EditorGUILayout.FloatField("maximumDeltaTime", timeKun.maximumDeltaTime);
                    timeKun.timeScale = EditorGUILayout.FloatField("timeScale", timeKun.timeScale);

                    EditorGUILayout.Space();            
                }
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull"))
            {                
                UnityChoseKunEditor.SendMessage<TimeKun>(UnityChoseKun.MessageID.TimePull,timeKun);
            }
            if(isDone){
                if (GUILayout.Button("Push"))
                {                
                    UnityChoseKunEditor.SendMessage<TimeKun>(UnityChoseKun.MessageID.TimePush,timeKun);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public void OnMessageEvent(string json)
        {
            timeKun = JsonUtility.FromJson<TimeKun>(json);
            isDone = true;
        }

    }
}