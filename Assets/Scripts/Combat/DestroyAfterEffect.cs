using UnityEngine;

namespace RPG.Combat
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;
        private ParticleSystem _particleSystem;

        private void Awake() => _particleSystem = GetComponent<ParticleSystem>();
        
        void Update()
        {
            if (!_particleSystem.IsAlive())
            {
                if (targetToDestroy != null)
                {
                    Destroy(targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}