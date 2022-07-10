using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cinemachine;
using RPG.Core;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity: MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = string.Empty;
        private static Dictionary<string, SaveableEntity> gloabalLookUp = new Dictionary<string, SaveableEntity>();
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISavable savable in GetComponents<ISavable>())
            {
                string typeString = savable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    savable.RestoreState(stateDict[typeString]);
                }
                
                
            }
            
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;
            if(string.IsNullOrEmpty(gameObject.scene.path)) return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            gloabalLookUp[property.stringValue] = this;

        }

        private bool IsUnique(string candidate)
        {
            if(!gloabalLookUp.ContainsKey(candidate)) return true;
            if (gloabalLookUp[candidate] == this) return true;
            if (gloabalLookUp[candidate] == null)
            {
                gloabalLookUp.Remove(candidate);
                return true;
            }

            if (gloabalLookUp[candidate].GetUniqueIdentifier() != candidate)
            {
                gloabalLookUp.Remove(candidate);
                return true;
            }
            return false;
        }
#endif
    }
}