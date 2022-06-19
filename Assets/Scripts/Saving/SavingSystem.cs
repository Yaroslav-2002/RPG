using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int)state["lastSceneBuildIndex"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
           
            RestoreState(state);
        }
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, Dictionary<string, object> capturedState)
        {
            string pathFromSaveFile = GetPathFromSaveFile(saveFile);
            print("Saving to " + pathFromSaveFile);
            using (FileStream stream = File.Open(pathFromSaveFile, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, capturedState);
            }
        }


        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string pathFromSaveFile = GetPathFromSaveFile(saveFile);
            if (!File.Exists(pathFromSaveFile))
            {
                return new Dictionary<string, object>();
            }
            print("Loading from " + pathFromSaveFile);
            using (FileStream stream = File.Open(pathFromSaveFile, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return  (Dictionary<string, object>)formatter.Deserialize(stream);
                
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if(state.ContainsKey(id)) 
                    saveable.RestoreState(state[id]);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
           
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()]=saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }
 
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}