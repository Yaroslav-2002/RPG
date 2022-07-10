using System;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        
        [SerializeField] private TextMeshProUGUI[] healthBar;
        private float EnemyHealth { get; set; }
        private float PlayerHealth { get; set; }
        private float OldEntityHealth { get; set; }
        
        private Health _healthPlayer;
        private Health _healthEnemy;
        private Fighter _fighter;
        
        private void Start()
        {
            _healthPlayer = GameObject.FindWithTag("Player").GetComponent<Health>();
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        private void Update()
        {
            PlayerHealth = _healthPlayer.GetPecentage();
            _healthEnemy = _fighter.GetTarget();
            
            
            if (_healthEnemy == null)
            {
                healthBar[1].text = "N/A";
            }
            else
            {
                EnemyHealth = _healthEnemy.GetPecentage();
                Debug.Log("wwow"+EnemyHealth);
                healthBar[0].text = $"{PlayerHealth:0}%"; // 0 - player healthBar
                healthBar[1].text = $"{EnemyHealth:0}%"; // 1 - enemy healthBar
                OldEntityHealth = PlayerHealth;
            }

        }
        
    }
}