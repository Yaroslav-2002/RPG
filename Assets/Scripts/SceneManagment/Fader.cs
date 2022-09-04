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
            _canvasGroup.alpha = 0;
        }
        
        private Coroutine _currrentlyActiveFade;
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }
        
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }
        
        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha , target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public Coroutine Fade(float target, float time)
        {
            if (_currrentlyActiveFade != null)
            {
                StopCoroutine(_currrentlyActiveFade);
            }
            _currrentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            return _currrentlyActiveFade;
        }

    }

}
