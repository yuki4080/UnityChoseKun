﻿namespace Utj.UnityChoseKun
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEditor;

    
    public class InspectorView
    {
        public sealed class Settings{
            private static class Styles {
                public static GUIContent gameObject = new GUIContent("", (Texture2D)EditorGUIUtility.Load("d_GameObject Icon"));                
            }
            [SerializeField] bool isDraw;
            [SerializeField] bool isActive;
            [SerializeField] string name;
            [SerializeField] bool isStatic;
            [SerializeField] string tag;
            [SerializeField] int layer;
            
            public Settings(){}
            public void Set(GameObjectKun gameObjectKun){
                if(gameObjectKun == null){
                    isDraw = false;
                } else {
                    isDraw = true;
                    isActive = gameObjectKun.activeSelf;
                    name = gameObjectKun.name;
                    isStatic = gameObjectKun.isStatic;
                    tag = gameObjectKun.tag;
                    layer = gameObjectKun.layer;
                }
            }

            public void Writeback(GameObjectKun gameObjectKun){
                gameObjectKun.activeSelf = isActive;
                gameObjectKun.name = name;
                gameObjectKun.isStatic = isStatic;
                gameObjectKun.tag = tag;
                gameObjectKun.layer = layer;
            }

            public void DrawGameObject(){
                if(isDraw == false){
                    return;
                }
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));            
                EditorGUILayout.BeginHorizontal();                     
                Styles.gameObject.text = name;
                isActive = EditorGUILayout.ToggleLeft(Styles.gameObject,isActive);                
                GUILayout.FlexibleSpace();
                isStatic = EditorGUILayout.ToggleLeft("Static",isStatic);
                EditorGUILayout.EndHorizontal();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));            

                EditorGUI.indentLevel += 1;
                tag = EditorGUILayout.TagField("Tag",tag);
                layer = EditorGUILayout.LayerField("Layer",layer);
                EditorGUI.indentLevel --;
            }

            
        }


        [SerializeField] Settings m_settings;
        Settings settings {
            get{if(m_settings == null){m_settings = new Settings();}return m_settings;}
            set{m_settings = value;}
        }
        [SerializeField] private  List<ComponentView> m_componentViews;            
        List<ComponentView> componentViews{
            get {if(m_componentViews == null){m_componentViews = new List<ComponentView>();}return m_componentViews;}
            set {m_componentViews = value;}
        }                
        [SerializeField] Dictionary<int,GameObjectKun> m_gameObjectKuns;
        Dictionary<int,GameObjectKun> gameObjectKuns {
            get {if(m_gameObjectKuns == null){m_gameObjectKuns = new Dictionary<int, GameObjectKun>();}return m_gameObjectKuns;}
        }                
        [SerializeField] int m_selectGameObujectKunID = -1;

        [SerializeField] SceneKun sceneKun;

        public InspectorView() {
            if(PlayerHierarchyWindow.window != null){
                PlayerHierarchyWindow.window.selectionChangedCB = SelectionChangedCB;
            }
        }

        void BuildComponentView(GameObjectKun gameObjectKun)
        {
            componentViews.Clear();
            if(gameObjectKun!=null) {
                m_selectGameObujectKunID = gameObjectKun.instanceID;
                for(var i = 0; i < gameObjectKun.componentDataJsons.Length; i++)
                {
                    var type = ComponentView.GetComponentViewSyetemType(gameObjectKun.componentKunTypes[i]);
                    var componentView = System.Activator.CreateInstance(type) as ComponentView;
                    componentView.SetJson(gameObjectKun.componentDataJsons[i]);
                    componentViews.Add(componentView);
                }
            }else{
                m_selectGameObujectKunID = -1;
            }
        }

        

        public void OnGUI() {        
            settings.DrawGameObject();
            foreach(var componentView in componentViews)
            {
                componentView.OnGUI();
            }                        
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(4));
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Pull")){   
                UnityChoseKunEditor.SendMessage(UnityChoseKun.MessageID.GameObjectPull);
            }
            if(GUILayout.Button("Push")){
                if(m_gameObjectKuns.ContainsKey(m_selectGameObujectKunID)){
                    var gameObjectKun = m_gameObjectKuns[m_selectGameObujectKunID];
                    settings.Writeback(gameObjectKun);                    
                    for(var i = 0; i < gameObjectKun.componentDataJsons.Length; i++)
                    {                        
                        gameObjectKun.componentDataJsons[i]=m_componentViews[i].GetJson();
                    }
                    UnityChoseKunEditor.SendMessage<GameObjectKun>(UnityChoseKun.MessageID.GameObjectPush,gameObjectKun);                                     
                }
            }
            EditorGUILayout.EndHorizontal();                        
        }
        
        public void OnMessageEvent(string json)
        {
            Debug.Log("OnMessageEvent");
            gameObjectKuns.Clear();
            sceneKun = JsonUtility.FromJson<SceneKun>(json);
            
            for(var i = 0; i < sceneKun.gameObjectKuns.Length; i++){
                gameObjectKuns.Add(sceneKun.gameObjectKuns[i].instanceID,sceneKun.gameObjectKuns[i]);
            }  
            if(PlayerHierarchyWindow.window == null){
                PlayerHierarchyWindow.Create();
            }
            if(PlayerHierarchyWindow.window != null){                
                PlayerHierarchyWindow.window.selectionChangedCB = SelectionChangedCB;
                PlayerHierarchyWindow.window.sceneKun = sceneKun;                
                PlayerHierarchyWindow.window.Reload();
            }
        }
        
        void SelectionChangedCB(IList<int> selectedIds)
        {            
            var id = PlayerHierarchyWindow.window.lastClickedID;
            if(gameObjectKuns.ContainsKey(id)){
                var gameObjectKun = gameObjectKuns[id];
                settings.Set(gameObjectKun);
                BuildComponentView(gameObjectKun);   
            } else {
                settings.Set(null);
                BuildComponentView(null);
            }            
            if(UnityChoseKunEditorWindow.window != null){
                UnityChoseKunEditorWindow.window.Repaint();
            }
        }
    }
}