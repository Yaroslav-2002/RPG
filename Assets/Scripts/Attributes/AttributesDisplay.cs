using System;
using RPG.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Attributes
{
    public class AttributesDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] hudBar;
        private float EnemyHealth { get; set; }
        private float PlayerHealth { get; set; }
        private float OldEntityHealth { get; set; }
       
        private Expirience _expirience;
        private Health _healthPlayer;
        private Health _healthEnemy;
        private Fighter _fighter;
        private GameObject _player;
        
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _healthPlayer = _player.GetComponent<Health>();
            _expirience = _player.GetComponent<Expirience>();
            _fighter = _player.GetComponent<Fighter>();
        }
        private void Update()
        {
            PlayerHealth = _healthPlayer.GetPecentage();
            _healthEnemy = _fighter.GetTarget();
            
            if (_healthEnemy == null)
            {
                hudBar[1].text = "N/A";
            }
            else
            {
                EnemyHealth = _healthEnemy.GetPecentage();
            }
            hudBar[0].text = $"{PlayerHealth:0}%"; // 0 - player healthBar
            hudBar[1].text = $"{EnemyHealth:0}%"; // 1 - enemy healthBar
            hudBar[2].text = $"{_expirience.GetExperience():0}"; // 2 - exp
            
            OldEntityHealth = PlayerHealth;
            
        }
        
    }
}