using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/ New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, float level)
        {
            BuildLookup();
            return _lookupTable[characterClass][stat][(int)(level - 1)];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            return _lookupTable[characterClass][stat].Length;
        }

        private void BuildLookup()
        {
            if(_lookupTable != null) return;
            _lookupTable = new();
            
            
            foreach (var progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }
                _lookupTable[progressionClass.characterClass] = statLookUpTable;
            }
        }

        [System.Serializable]
        struct ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            // public float[] health;
        }
        [System.Serializable]
        struct ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    } 

}

