using System;
using System.Collections;
using RPG.Saving;
using UnityEditorInternal;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.2f;
        private Fader _fade;
        private SavingSystem _savingSystem;
        private IEnumerator _enumerator;
        
        private void Awake()
        {
            _enumerator = _savingSystem.LoadLastScene(defaultSaveFile);
            _savingSystem = GetComponent<SavingSystem>();
            _fade = FindObjectOfType<Fader>();
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            _fade.FadeOutImidiate();
            yield return _enumerator;
            yield return _fade.FadeIn(fadeInTime);
        }

        private void Update()
        { 
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}