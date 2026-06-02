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
                for (int i = towersCount - 1; i > 0; --i)
                {
                    Tower tower = _towers[i];
                    if (tower.upgradePrefab == null) continue;
                    Debug.Log("Upgrade!!");
                    _towers.Remove(tower);
                    _towers.Add(Instantiate(tower.upgradePrefab, tower.transform.position, Quaternion.identity).GetComponent<Tower>());
                    Destroy(tower.gameObject);
                }
            }
            
            Queue<TowerSpawnInfo> towerSpawnInfos = new Queue<TowerSpawnInfo>();
            int count = _towerPool.Count;
            for(int i = 0; i < count; i++)
            {
                if (_towerPool.Peek().wave == wave)
                {
                    _towers.Add(_towerPool.Peek().towerPrefab.GetComponent<Tower>());
                    towerSpawnInfos.Enqueue(_towerPool.Dequeue());
                }
                else
                    break;
            }

            while (towerSpawnInfos.Count > 0)
            {
                TowerSpawnInfo info = towerSpawnInfos.Dequeue();
                _towers.Add(
                    Instantiate(info.towerPrefab, info.spawnPos, Quaternion.identity)
                    .GetComponent<Tower>());
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
