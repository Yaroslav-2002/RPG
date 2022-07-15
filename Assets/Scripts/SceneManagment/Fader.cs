using System.Collections;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class Fader : MonoBehaviour
    {
         CanvasGroup _canvasGroup;

         private void Awake()
        {
            if(_canvasGroup== null)
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImidiate()
        {
            _canvasGroup.alpha = 1;
        }
        

        
        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha<1)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha>0)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
   
    }

}
