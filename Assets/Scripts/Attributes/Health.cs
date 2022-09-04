using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISavable
    {
        [SerializeField] private TakeDamageEvent takeDamage;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            
        }
        private static readonly int Die1 = Animator.StringToHash("die");
        private float HealthPoints { get; set; } = -1f;
        private float _maxHealth;
        private bool _isDead = false;
        private BaseStats _baseStats;
        public object CaptureState() => HealthPoints;
        public float GetMaxHealth() => _maxHealth;
        public float GetPecentage() => GetFraction() * 100;
        public float GetFraction() => HealthPoints / _maxHealth;
        public float GetHealthpoints() => HealthPoints;
        public bool IsDead() => _isDead;
        public event Action OnTakeDamage;
        
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
            if (HealthPoints < 0)
            {
                HealthPoints = _baseStats.GetStat(Stat.Health);
            }
            _maxHealth = HealthPoints;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (OnTakeDamage != null) OnTakeDamage();
            takeDamage.Invoke(damage);
            print(gameObject.name + "took damage: " + damage);
            HealthPoints = Mathf.Max(HealthPoints- damage,0 );
            
            if (HealthPoints == 0)
            {
                Die();
                AwardExpirience(instigator); 
            }
        }
        
        private void SetMaxHealth()
        {
            HealthPoints = _baseStats.GetStat(Stat.Health);
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
            HealthPoints = (float)state;
            if (HealthPoints <= 0) 
                Die();
        }
    }
}

