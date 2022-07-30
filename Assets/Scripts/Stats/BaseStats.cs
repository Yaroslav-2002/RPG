using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticle;
        
        private float _xPtoLevelUp;
        private Expirience _currentExperience;
        private float _experienceValue;
        private int? _currentLevel;
        public event Action OnLevelUp;
        
        public float GetStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());
        
        private void Awake()
        {
            _currentExperience = GetComponent<Expirience>();
            _currentLevel = CalculateLevel();
            if (_currentExperience != null) _currentExperience.OnExpirienceChange += UpdateLevel;
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
