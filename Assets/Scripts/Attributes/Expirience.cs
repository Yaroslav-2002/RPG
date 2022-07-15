using RPG.Combat;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Expirience:MonoBehaviour, ISavable
    {
        [SerializeField] private float expirience {get; set; }

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
            expirience = (int)state;
        }
    }
}