using System.Collections.Generic;
using UnityEngine;

namespace GameSpecific.Tank.Data
{
    [CreateAssetMenu(fileName = "LoadLevelData", menuName = "Tank/Levels/LevelInfoData")]
    public class LevelInfoData : ScriptableObject
    {
        [System.Serializable]
        private struct EnemySpawnData
        {
            public GameObject prefab;
            public Vector3 position;
            public Quaternion rotation;
        }
        [System.Serializable]
        private struct LevelObjects
        {
            public GameObject prefab;
            public Vector3 position;
        }
        public bool levelCompleted;
        public GameObject levelPrefab;
        [SerializeField] private List<EnemySpawnData> enemySpawns;
        [SerializeField] private List<LevelObjects> levelObjects;
    }
}