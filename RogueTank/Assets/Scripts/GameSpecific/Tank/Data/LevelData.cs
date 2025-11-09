using System.Collections.Generic;
using Primitives;
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
    }
}