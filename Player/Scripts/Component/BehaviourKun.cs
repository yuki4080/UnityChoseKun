﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utj.UnityChoseKun {

    /// <summary>
    /// Behaviourをシリアライズ・デシリアライズする為のClass
    /// Programed by Katsumasa.Kimura
    /// </summary>
    [System.Serializable]
    public class BehaviourKun : ComponentKun
    {
        /// <summary>
        /// 有効/無効
        /// </summary>
        [SerializeField] bool m_enabled;
        public bool enabled{
            get {return m_enabled;}
            set {m_enabled = value;}
        }
        
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BehaviourKun():this(null){}
        
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="component">Beviourオブジェクト</param>
        public BehaviourKun(Component component):base(component)
        {
            componentKunType = ComponentKunType.Behaviour;
            var behaviour = component as Behaviour;
            if(behaviour != null){
                enabled = behaviour.enabled;
                name = behaviour.GetType().Name;
            }
        }


        /// <summary>
        /// Behaviourに内容を書き戻す
        /// </summary>
        /// <param name="component">書き戻すBehaviour</param>
        /// <returns>結果 true:書き戻しを行った</returns>
        public override bool WriteBack(Component component)
        {
            if(base.WriteBack(component))
            {
                var behaviour = component as Behaviour;
                if (behaviour)
                {
                    behaviour.enabled = enabled;
                    return true;
                }
            }
            return false;
        } 
    }
}