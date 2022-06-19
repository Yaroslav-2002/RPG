using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] public float health = 100f;
        private bool _isDead = false;
        private static readonly int Die1 = Animator.StringToHash("die");

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health- damage,0 );

            if (health == 0) Die();
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger(Die1);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0) 
                Die();
        }
    }
}

