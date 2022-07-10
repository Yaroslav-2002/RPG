using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEditorInternal;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] public float health;
        private bool _isDead = false;
        private static readonly int Die1 = Animator.StringToHash("die");
        private float _maxHealth;

        private void Awake()
        {
            health = GetComponent<BaseStats>().GetHealth();
            _maxHealth = health;
        }

        public float GetPecentage()
        {
            print((health / _maxHealth) * 100);
            return (health / _maxHealth) * 100;
        }
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

