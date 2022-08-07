using System;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticle;
        [SerializeField] private bool shouldUseModifier = false;
        
        private float _xPtoLevelUp;
        private Expirience _currentExperience;
        private float _experienceValue;
        private int? _currentLevel;
        public event Action OnLevelUp;
        
        public float GetStat(Stat stat) => (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);

        private void Awake()
        {
            _currentExperience = GetComponent<Expirience>();
        }

        private void Start()
        {
            _currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (_currentExperience != null) 
                _currentExperience.OnExpirienceChange += UpdateLevel;
        }

        private void OnDisable()
        {
            if (_currentExperience != null) 
                _currentExperience.OnExpirienceChange -= UpdateLevel;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (var provider in providers)
            {
                foreach (var modifiers in provider.GetPercentageModifiers(stat))
                {
                    total += modifiers;
                }
            }

            return total;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (var provider in providers)
            {
                foreach (var modifiers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifiers;
                }
            }

            return total;
        }
        
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                print("Levelled up");
            }
            OnLevelUp?.Invoke();
            LevelUpEffect();
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticle, transform);
        }
        
        public int GetLevel()
        {
            if (_currentLevel == null) _currentLevel = CalculateLevel();
            return (int)_currentLevel;
        }

        private int CalculateLevel()
        {
            if (_currentExperience == null)
            {
                return startingLevel;
            }
            _experienceValue = _currentExperience.GetExperience();
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            
            for (int level = 1; level <= maxLevel; level++)
            {
                Debug.Log($"XPtoLevelUp {_xPtoLevelUp}, _experienceValue {_experienceValue}, level {level}");
                _xPtoLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (_xPtoLevelUp > _experienceValue)
                {
                    return level;
                }
            }
            return maxLevel;
        }
    }
}
