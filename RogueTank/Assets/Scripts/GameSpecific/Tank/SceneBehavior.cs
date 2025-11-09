using System.Collections;
using GameSpecific.Tank.Data;
using UnityEngine;
using UnityEngine.Events;


namespace GameSpecific.Tank
{
    public class SceneBehavior : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        //[SerializeField] private ScreenFader fader;
        [SerializeField] private float fadeDuration = 1f;
        public UnityEvent onLevelLoaded;

        private GameObject levelRoot;
        
        private void Start()
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
            //if (fader != null) yield return StartCoroutine(fader.FadeOut(fadeDuration));
            var info = levelData.GetCurrentLevelInfo();
            if (info == null || info.levelPrefab == null)
            {
                Debug.LogError("LevelInfoData or levelPrefab is null for the current level.");
                //if (fader != null) yield return StartCoroutine(fader.FadeIn(fadeDuration));
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
            
            //if (fader != null) yield return StartCoroutine(fader.FadeIn(fadeDuration));
            
            onLevelLoaded?.Invoke();

            StartCoroutine(LevelCompletionClenUp(info));
        }

        private IEnumerator LevelCompletionClenUp(LevelInfoData info)
        {
            while (!info.levelCompleted)
            {
                yield return null;
            }
            
            if (levelData.currentLevelIndex != null)
            {
                levelData.MarkLevelCompleted(levelData.currentLevelIndex.Value);
            }
            
            //if (fader != null) yield return StartCoroutine(fader.FadeOut(fadeDuration));
            
            if (levelRoot != null) Destroy(levelRoot);
            
            //if (fader != null) yield return StartCoroutine(fader.FadeOut(fadeDuration));
        }
    }
}