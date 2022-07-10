using System.Collections;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;


namespace RPG.SceneManagment
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] public Transform Spawnpoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float FadeInTime = 0.2f;
        [SerializeField] private float FadeOutTime = 0.2f;
        [SerializeField] private float FadeWaitTime = 0.1f;
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(FadeOutTime);
            
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            savingWrapper.Load();
            
            Portal otherPortal = GetOtherPortal();
            
            UpdatePlayer(otherPortal);
            
            savingWrapper.Save();
            
            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            Destroy(gameObject);
        }
        
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.Spawnpoint.position);
            player.transform.rotation = otherPortal.Spawnpoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }

        Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            Debug.Log("No portlas were found");
            return null;
        }
        
    }
}
