using System.Collections.Generic;
using UnityEngine;

namespace GameSpecific.Tank.Data
{
    [CreateAssetMenu(fileName = "LoadLevelData", menuName = "Tank/Levels/LevelInfoData")]
    public class LevelInfoData : ScriptableObject
    {
        [System.Serializable]
        public struct EnemySpawnData
        {
            public GameObject prefab;
            public Vector3 position;
            public Quaternion rotation;
        }
        [System.Serializable]
        public struct LevelObjects
        {
            public GameObject prefab;
            public Vector3 position;
        }
        public bool levelCompleted;
        public GameObject levelPrefab;
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;
        [SerializeField] private List<EnemySpawnData> enemySpawns;
        [SerializeField] private List<LevelObjects> levelObjects;
        
        public IReadOnlyList<EnemySpawnData> EnemySpawns => enemySpawns;
        public IReadOnlyList<LevelObjects> LevelObjectsList => levelObjects;
    }
}