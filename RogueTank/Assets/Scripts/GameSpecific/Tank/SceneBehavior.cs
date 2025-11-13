using System.Collections;
using Core.Primitives;
using GameSpecific.Tank.Data;
using UnityEngine;
using UnityEngine.Events;
using Utilities;


namespace GameSpecific.Tank
{
    public class SceneBehavior : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private ScreenFader fader;
        [SerializeField] private IntData enemyCount;
        public UnityEvent onLevelLoaded, beforeFadeInEvent;

        private GameObject levelRoot;
        
        public void StartLevelLoading()
        {
            if (levelData == null)
            {
                Debug.LogError("LevelData is not assigned in SceneBehavior.");
                return;
            }
            StartCoroutine(LoadCurrentLevel());
        }

        private IEnumerator LoadCurrentLevel()
        {
            if (levelRoot != null) Destroy(levelRoot);
            if (fader != null) yield return StartCoroutine(fader.FadeOut());
            var info = levelData.GetCurrentLevelInfo();
            enemyCount.Value = levelData.GetLevelInfo(levelData.currentLevelIndex.Value).EnemySpawns.Count;
            if (info == null || info.levelPrefab == null)
            {
                Debug.LogError("LevelInfoData or levelPrefab is null for the current level.");
                if (fader != null) yield return StartCoroutine(fader.FadeIn());
                yield break;
            }

            if (info.levelPrefab != null)
            {
                levelRoot = Instantiate(info.levelPrefab, info.spawnPosition, Quaternion.identity);
                levelRoot.name = info.levelPrefab.name + "_Instance";
            }
            
            if (info.LevelObjectsList != null)
            {
                foreach (var obj in info.LevelObjectsList)
                {
                    if (obj.prefab == null) continue;
                    var instance = Instantiate(obj.prefab, obj.position, Quaternion.identity);
                    if (levelRoot != null) instance.transform.SetParent(levelRoot.transform, worldPositionStays: true);
                }
            }
            
            if (info.EnemySpawns != null)
            {
                foreach (var enemy in info.EnemySpawns)
                {
                    if (enemy.prefab == null) continue;
                    var enemyInstance = Instantiate(enemy.prefab, enemy.position, enemy.rotation);
                    if (levelRoot != null) enemyInstance.transform.SetParent(levelRoot.transform, worldPositionStays: true);
                }
            }
            
            levelData.bossLevelActive.value = info.levelPrefab != null && info.levelPrefab.name.ToLower().Contains("boss");
            
            beforeFadeInEvent?.Invoke();
            
            if (fader != null) yield return StartCoroutine(fader.FadeIn());
            
            onLevelLoaded?.Invoke();

            //StartCoroutine(LevelCompletionClenUp(info));
        }

        /*private IEnumerator LevelCompletionClenUp(LevelInfoData info)
        {
            while (!info.levelCompleted)
            {
                yield return null;
            }
            
            //if (fader != null) yield return StartCoroutine(fader.FadeOut(fadeDuration));
            
            if (levelRoot != null) Destroy(levelRoot);
            
            //if (fader != null) yield return StartCoroutine(fader.FadeOut(fadeDuration));
        }*/
    }
}