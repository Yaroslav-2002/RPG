using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Expirience:MonoBehaviour, ISavable
    {
        [SerializeField] private float expirience;

        public void GainExpirience(float expirience)
        {
            this.expirience += expirience;
        }

        public float GetExperience()
        {
            return expirience;
        }

        public object CaptureState()
        {
            return expirience;
        }

        public void RestoreState(object state)
        {
            expirience = (float)state;
        }
    }
}