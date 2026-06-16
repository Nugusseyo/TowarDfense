using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Agent.Tower
{
    public class TowerSpawner : MonoBehaviour, ITowerSpawner
    {
        public List<TowerSpawnInfo> towerSpawnInfos = new List<TowerSpawnInfo>();
        private Queue<TowerSpawnInfo> _towerPool;
        
        private readonly List<TowerSlot> _activeSlots = new List<TowerSlot>();
        
        private class TowerSlot
        {
            public TowerSpawnInfo spawnInfo;
            public Tower currentTower;
        }

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
            foreach (TowerSlot slot in _activeSlots)
            {
                if (slot.currentTower == null)
                {
                    Tower restoredTower = Instantiate(slot.spawnInfo.towerPrefab, slot.spawnInfo.spawnPos, Quaternion.identity)
                        .GetComponent<Tower>();
                    
                    slot.currentTower = restoredTower;
                    restoredTower.ShutDownThreeSecond();
                    
                    Debug.Log($"[TowerSpawner] {slot.spawnInfo.spawnPos} 위치의 부서진 타워를 복구했습니다.");
                }
                else
                {
                    if (slot.currentTower.upgradePrefab != null)
                     {
                         GameObject upgradedTowerObj = slot.currentTower.TowerUpgrade(); 
                         slot.currentTower = upgradedTowerObj.GetComponent<Tower>();
                         Debug.Log("Upgrade!!");
                     }
                    else
                    {
                        slot.currentTower.ShutDownThreeSecond();
                    }
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
                Tower newTower = Instantiate(info.towerPrefab, info.spawnPos, Quaternion.identity)
                    .GetComponent<Tower>();
                
                _activeSlots.Add(new TowerSlot 
                { 
                    spawnInfo = info, 
                    currentTower = newTower 
                });
                
                newTower.ShutDownThreeSecond();
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