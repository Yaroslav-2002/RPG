using System.Runtime.InteropServices;
using RPG.Combat;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class AttributesDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] hudBar;
        private float EnemyHealth { get; set; }
        private float PlayerHealth { get; set; }
        private GameObject _player;
        private Expirience _expirience;
        private Health _healthPlayer;
        private Health _healthEnemy;
        private Fighter _fighter;
        private BaseStats _baseStats;
        
        private void Awake()
        {
            print("init");
            _player = GameObject.FindWithTag("Player");
            _healthPlayer = _player.GetComponent<Health>();
            _expirience = _player.GetComponent<Expirience>();
            _fighter = _player.GetComponent<Fighter>();
            _baseStats = _player.GetComponent<BaseStats>();
        }

        private void OnEnable()
        {
            _healthPlayer.OnTakeDamage += UpdateAttributes;
            _expirience.OnExpirienceChange += UpdateAttributes;
            _fighter.OnTargetChange += UpdateAttributes;

        }

        private void OnDisable()
        {
            _healthPlayer.OnTakeDamage -= UpdateAttributes;
            _expirience.OnExpirienceChange -= UpdateAttributes;
            _fighter.OnTargetChange -= UpdateAttributes;
        }
        
        private void UpdateAttributes()
        {
            print("Updated");
            _healthEnemy = _fighter.GetTarget();
            PlayerHealth = _healthPlayer.GetHealthpoints();
            Debug.Log(PlayerHealth + " " + _healthEnemy);
            if (_healthEnemy == null)
            {
                hudBar[1].text = "N/A";
            }
            else
            {
                EnemyHealth = _healthEnemy.GetHealthpoints();
            }
            hudBar[0].text = $"{_healthPlayer.GetMaxHealth():0}/{PlayerHealth:0}"; // 0 - player healthBar
            hudBar[1].text = $"{EnemyHealth:0}"; // 1 - enemy healthBar
            hudBar[2].text = $"{_expirience.GetExperience():0}"; // 2 - exp
            hudBar[3].text = $"{_baseStats.GetLevel():0}"; // 2 - level

        }
        
    }
}