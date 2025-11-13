using GameSpecific.Tank.Data;
using UnityEngine;
using UnityEngine.Events;

namespace GameSpecific.Tank
{
    public class RogueTanksGameManager : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private LevelData levelData;
        [SerializeField] private int currentEnemyCount;
        [SerializeField] private GameAction onStartAction;
        [SerializeField] private UnityEvent onStart, levelClearedEvent, failedEvent, gameClearedEvent;
        
        private void Awake()
        {
            currentEnemyCount = levelData.GetLevelInfo(levelData.currentLevelIndex.Value).EnemySpawns.Count;
        }
        private void Start()
        {
            onStartAction.RaiseAction();
            onStart?.Invoke();
        }
        public void Cleared()
        {
            if (!ClearedLevel()) return;
            levelData.MarkLevelCompleted(levelData.currentLevelIndex.Value);
            
            if (levelData.currentLevelIndex.Value <= levelData.LevelsCount)
            {
                levelData.LevelProgression(levelData.currentLevelIndex.Value);
                levelClearedEvent?.Invoke();
            }
            else
            {
                gameClearedEvent?.Invoke();
            }
            
            
        }
        private bool ClearedLevel()
        {
            currentEnemyCount--;
            if (levelData == null || levelData.currentLevelIndex == null) return false;
            switch (currentEnemyCount)
            {
                case > 0:
                    return false;
                case <= 0:
                    levelData.MarkLevelCompleted(levelData.currentLevelIndex.Value);
                    break;
            }
            return true;
        }
        
        public void Failed()
        {
            failedEvent?.Invoke();
        }
    }
}
