using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI
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
        private BaseStats _baseStats;
        
        
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _healthPlayer = _player.GetComponent<Health>();
            _expirience = _player.GetComponent<Expirience>();
            _fighter = _player.GetComponent<Fighter>();
            _baseStats = _player.GetComponent<BaseStats>();
        }
        private void Update()
        {
            PlayerHealth = _healthPlayer.GetHealthpoints();
            _healthEnemy = _fighter.GetTarget();
            
            if (_healthEnemy == null)
            {
                hudBar[1].text = "N/A";
            }
            else
            {
                EnemyHealth = _healthEnemy.GetHealthpoints();
            }
            hudBar[0].text = $"{PlayerHealth:0}/{_healthPlayer.GetMaxHealth():0}"; // 0 - player healthBar
            hudBar[1].text = $"{EnemyHealth:0}%"; // 1 - enemy healthBar
            hudBar[2].text = $"{_expirience.GetExperience():0}"; // 2 - exp
            hudBar[3].text = $"{_baseStats.GetLevel():0}"; // 2 - level
            OldEntityHealth = PlayerHealth;
            
        }
        
    }
}