using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        private static readonly int Die1 = Animator.StringToHash("die");
        public float health = -1f;
        private float _maxHealth;
        private bool _isDead = false;
        private BaseStats _baseStats;
        public object CaptureState() => health;
        public float GetMaxHealth() => _maxHealth;
        public float GetPecentage() => (health / _maxHealth) * 100;
        public float GetHealthpoints() => health;
        public bool IsDead() => _isDead;
        
        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            _baseStats.OnLevelUp += SetMaxHealth;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= SetMaxHealth;
        }

        private void Start()
        {
            if (health < 0)
            {
                health = _baseStats.GetStat(Stat.Health);
            }
            _maxHealth = health;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage: " + damage);
            health = Mathf.Max(health- damage,0 );

            if (health == 0)
            {
                Die();
                AwardExpirience(instigator);
            }
        }
        
        private void SetMaxHealth()
        {
            health = _maxHealth;
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
        
        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0) 
                Die();
        }
    }
}

