using System;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    public class Expirience:MonoBehaviour, ISavable
    {
        public event Action OnExpirienceChange;
        [SerializeField] private float _expirience;

        public void GainExpirience(float expirience)
        {
            _expirience += expirience;
            if (OnExpirienceChange != null) OnExpirienceChange();
        }

        public float GetExperience()
        {
            return _expirience;
        }

        public object CaptureState()
        {
            return _expirience;
        }

        public void RestoreState(object state)
        {
            _expirience = (float)state;
        }
    }
}