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
        public float health = -1f;
        private bool _isDead = false;
        private static readonly int Die1 = Animator.StringToHash("die");
        private float _maxHealth;

        private void Awake()
        {
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
            _maxHealth = health;
        }

        public float GetPecentage()
        {
            return (health / _maxHealth) * 100;
        }
        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health- damage,0 );

            if (health == 0)
            {
                Die();
                AwardExpirience(instigator);
            }
            
        }

        private void AwardExpirience(GameObject instigator)
        {
            Expirience expirience = instigator.GetComponent<Expirience>();
            if(expirience == null) return;
            
            expirience.GainExpirience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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

