using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Agent.Tower
{
    public class TowerSpawner : MonoBehaviour, ITowerSpawner
    {
        public List<TowerSpawnInfo> towerSpawnInfos = new List<TowerSpawnInfo>();
        private Queue<TowerSpawnInfo> _towerPool;
        
        private readonly List<Tower> _towers = new List<Tower>();
        
        private void Awake()
        {
            towerSpawnInfos.Sort((a, b) => a.wave.CompareTo(b.wave));
            _towerPool = new Queue<TowerSpawnInfo>();
            
            foreach (TowerSpawnInfo info in towerSpawnInfos)
            {
                _towerPool.Enqueue(info);
            }
        }

        public void SpawnTower(int wave)
        {
            //Upgrade 구간.
            if (_towers.Count != 0)
            {
                int towersCount = _towers.Count;
                for (int i = towersCount - 1; i >= 0; --i)
                {
                    Tower tower = _towers[i];
                    if (tower.upgradePrefab == null)
                    {
                        tower.ShutDownThreeSecond();
                        continue;
                    }
                    Debug.Log("Upgrade!!");
                    _towers.Remove(tower);
                    GameObject newTower = tower.TowerUpgrade();
                    _towers.Add(newTower.GetComponent<Tower>());
                }
            }
            
            Queue<TowerSpawnInfo> spawnInfos = new Queue<TowerSpawnInfo>();
            int count = _towerPool.Count;
            for(int i = 0; i < count; i++)
            {
                if (_towerPool.Peek().wave == wave)
                {
                    spawnInfos.Enqueue(_towerPool.Dequeue());
                }
                else
                    break;
            }

            while (spawnInfos.Count > 0)
            {
                TowerSpawnInfo info = spawnInfos.Dequeue();
                Tower tower = Instantiate(info.towerPrefab, info.spawnPos, Quaternion.identity)
                    .GetComponent<Tower>();
                _towers.Add(tower);
                tower.ShutDownThreeSecond();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.gold;
            foreach (TowerSpawnInfo info in towerSpawnInfos)
            {
                Gizmos.DrawSphere(info.spawnPos, 0.5f);
            }
        }
    }

    [Serializable]
    public class TowerSpawnInfo
    {
        public Vector3 spawnPos;
        public GameObject towerPrefab;
        public int wave;
    }
}
