using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;

        private float _xPtoLevelUp;
        private Expirience _currentExperience;
        private float _experienceValue;

        private void Awake()
        {
            _currentExperience = GetComponent<Expirience>();
            
        }
        
        public float GetStat(Stat stat)
        {
            Debug.Log(GetLevel());
            return progression.GetStat(stat, characterClass, GetLevel());
        }
        public int GetLevel()
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
