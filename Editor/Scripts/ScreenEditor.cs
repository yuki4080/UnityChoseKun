﻿namespace Utj.UnityChoseKun
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public class ScreenEditor
    {
        ScreenKun m_screenKun;
        ScreenKun screenKun {
            get {
                if(m_screenKun == null){
                    m_screenKun = new ScreenKun();
                }
                return m_screenKun;
            }            
            set {
                m_screenKun = value;
            }
        }

        bool resolutionFoldout = false;

        public void OnGUI()
        {
            EditorGUILayout.LabelField("Screen");
            EditorGUI.indentLevel++;

            screenKun.autorotateToLandscapeLeft = EditorGUILayout.ToggleLeft("autorotateToLandscapeLeft", screenKun.autorotateToLandscapeLeft);
            screenKun.autorotateToLandscapeRight = EditorGUILayout.ToggleLeft("autorotateToLandscapeRight", screenKun.autorotateToLandscapeRight);
            screenKun.autorotateToPortrait = EditorGUILayout.ToggleLeft("autorotateToPortrait", screenKun.autorotateToPortrait);
            screenKun.autorotateToPortraitUpsideDown = EditorGUILayout.ToggleLeft("autorotateToPortraitUpsideDown", screenKun.autorotateToPortraitUpsideDown);
            screenKun.orientation = (ScreenOrientation)EditorGUILayout.EnumPopup("orientation", screenKun.orientation);

            EditorGUILayout.Space();

            screenKun.fullScreen = EditorGUILayout.ToggleLeft("fullScreen", screenKun.fullScreen);
            screenKun.sleepTimeout = EditorGUILayout.IntField("sleepTimeout", screenKun.sleepTimeout);
            screenKun.brightness = EditorGUILayout.FloatField("brightness", screenKun.brightness);

            EditorGUILayout.Space();

            EditorGUILayout.FloatField("dpi", screenKun.dpi);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("currentResolution");
            EditorGUI.indentLevel++;
            EditorGUILayout.IntField("width", screenKun.currentResolutionWidth);
            EditorGUILayout.IntField("height", screenKun.currentResolutionHeight);
            EditorGUILayout.IntField("refreshRate", screenKun.currentResolutionRefreshRate);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("cutouts");
            if (screenKun.cutouts != null)
            {
                EditorGUI.indentLevel++;
                for (var i = 0; i < screenKun.cutouts.Length; i++)
                {
                    EditorGUILayout.RectField(" [" + i + "]", screenKun.cutouts[i]);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            
            if (screenKun.resolutions == null)
            {
                EditorGUILayout.LabelField("resolutions");
            }
            else
            {
                resolutionFoldout = EditorGUILayout.Foldout(resolutionFoldout,"resolutions");
                if(resolutionFoldout){
                    EditorGUI.indentLevel++;
                    for (var i = 0; i < screenKun.resolutions.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("width       " + screenKun.resolutions[i].width);
                        EditorGUILayout.LabelField("height      " + screenKun.resolutions[i].height);
                        EditorGUILayout.LabelField("refreshRate " + screenKun.resolutions[i].refreshRate);                        
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.RectField("safeArea", screenKun.safeArea);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("SetScreen");
            EditorGUI.indentLevel++;
            screenKun.width = EditorGUILayout.IntField("width", screenKun.width);
            screenKun.height = EditorGUILayout.IntField("height", screenKun.height);
            screenKun.fullScreenMode = (FullScreenMode)EditorGUILayout.EnumPopup("fullScreenMode", screenKun.fullScreenMode);
            screenKun.preferredRefreshRate = EditorGUILayout.IntField("preferredRefreshRate", screenKun.preferredRefreshRate);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull"))
            {                
                UnityChoseKunEditor.SendMessage<ScreenKun>(UnityChoseKun.MessageID.ScreenPull, screenKun);
            }

            if (GUILayout.Button("Push"))
            {                
                UnityChoseKunEditor.SendMessage<ScreenKun>(UnityChoseKun.MessageID.ScreenPush,screenKun);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = 0;
        }


        public void OnMessageEvent(string json)
        {
            screenKun = JsonUtility.FromJson< ScreenKun>(json);            
        }
    }


    

}