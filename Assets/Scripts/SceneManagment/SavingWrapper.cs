using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.2f;
        private Fader _fade;
        private SavingSystem _savingSystem;
        private IEnumerator _enumerator;
        
        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            _enumerator = _savingSystem.LoadLastScene(DefaultSaveFile);
            _fade = FindObjectOfType<Fader>();
        }

        private void Start()
        {
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
            _savingSystem.Load(DefaultSaveFile);
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }
        
        public void Delete()
        {
            _savingSystem.Delete(DefaultSaveFile);
        }
    }
}