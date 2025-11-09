using System.Collections.Generic;
using Core.Primitives;
using UnityEngine;

namespace GameSpecific.Tank.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Tank/Levels/LevelData")]
    public class LevelData : ScriptableObject
    {
         public IntData currentLevelIndex;
         public BoolData bossLevelActive;
         [System.Serializable]
         private struct LoadLevel
         {
             public LevelInfoData levelInfo;
             public bool completed;
         }
         
         [SerializeField] private List<LoadLevel> levels;
         
         public int LevelsCount => levels != null ? levels.Count : 0;
         
         public LevelInfoData GetLevelInfo(int index)
         {
             if (levels == null || index < 0 || index >= levels.Count)
             {
                 Debug.LogError($"GetLevelInfo: Invalid index {index}.");
                 return null;
             }
             return levels[index].levelInfo;
         }
         
         public LevelInfoData GetCurrentLevelInfo()
         {
             if (currentLevelIndex == null) return null;
             return GetLevelInfo(currentLevelIndex.Value);
         }
         
         public void MarkLevelCompleted(int index)
         {
             if (levels == null || index < 0 || index >= levels.Count) return;
             var entry = levels[index];
             entry.completed = true;
             levels[index]= entry;
         }

         private bool IsLevelCompleted(int index)
        {
            if (levels == null || index < 0 || index >= levels.Count) return false;
            return levels[index].completed;
        }

        public void LevelProgression(int index)
        {
            if (!IsLevelCompleted(currentLevelIndex.Value)) return;
            if (levels == null || index < 0 || index >= levels.Count) return;
            currentLevelIndex.Value++;
        }
        public void ResetAllLevels()
        {
            if (levels == null) return;
            for (var i = 0; i < levels.Count; i++)
            {
                var entry = levels[i];
                entry.completed = false;
                levels[i] = entry;
            }
        }
    }
}