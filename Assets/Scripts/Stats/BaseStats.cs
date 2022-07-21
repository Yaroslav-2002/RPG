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
        [SerializeField] public int level;
        
        private float XPtoLevelUp { get; set; }
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

        private void Update()
        {
            level = GetLevel();
        }

        private int GetLevel()
        {
            if (_currentExperience == null)
            {
                return startingLevel;
            }
            _experienceValue = _currentExperience.GetExperience();
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass).Length;
            XPtoLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
            
            for (int level = 1; level <= maxLevel; level++)
            {
                Debug.Log($"XPtoLevelUp {XPtoLevelUp}, _experienceValue {_experienceValue}, level {level}");
                
                
                
                if (_experienceValue < XPtoLevelUp)
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }

        
    }
}
