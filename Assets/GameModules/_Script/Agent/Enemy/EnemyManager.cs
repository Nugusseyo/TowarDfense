using System;
using System.Collections;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Script.Agent.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO DataSaveEventChannel { get; private set; }
        public List<EnemyInformation> spawnTargetEnemy = new List<EnemyInformation>();

        public delegate void HandleEnemyAllDeath();

        public event HandleEnemyAllDeath OnEnemyAllDeath;

        [SerializeField] private Transform spawnPoint;
        
        private EnemyInformation _lastSpawnEnemyInfo;
        
        private float _lastSpawnTime;
        private int _currentIndex = 0;  

        public int EnemyCounter { get; private set; }

        public int DeathEnemyCounter
        {
            get => _deathEnemyCounter;
            set
            {
                _deathEnemyCounter = value;
                if (_deathEnemyCounter == EnemyCounter)
                {
                    OnEnemyAllDeath?.Invoke();
                }
            }
        }

        private int _deathEnemyCounter = 0;

        private void Awake()
        {
            _lastSpawnTime = Time.time;
            _lastSpawnEnemyInfo = spawnTargetEnemy[0];
            EnemyCounter = spawnTargetEnemy.Count;
            OnEnemyAllDeath += HandleOnEnemyAllDeath;
        }

        private void HandleOnEnemyAllDeath()
        {
            //DataSaveEventChannel.RaiseEvent(DataEvents.DataSaveEvent);
            StartCoroutine(WaitSecondSceneMover());
        }

        private IEnumerator WaitSecondSceneMover()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            OnEnemyAllDeath -= HandleOnEnemyAllDeath;
        }

        private void Update()
        {
            if (Time.time > _lastSpawnEnemyInfo.spawnDelay + _lastSpawnTime && spawnTargetEnemy.Count > _currentIndex) // 마지막 적을 스폰한 이후 딜레이만큼의 시간이 지났다면, 그리고 리스트가 아직 남아있다면
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            _lastSpawnTime = Time.time;
            _lastSpawnEnemyInfo = spawnTargetEnemy[_currentIndex];
            GameObject createdEnemy = Instantiate(_lastSpawnEnemyInfo.enemyObject);
            //콜라이더 켜주기
            Enemy spawnedEnemy = createdEnemy.GetComponent<Enemy>();
            spawnedEnemy.enemyRouteList = _lastSpawnEnemyInfo.enemyRoute;
            spawnedEnemy.spawnTrm = spawnPoint;
            spawnedEnemy.OnDeath.AddListener(HandleEnemyDeath);
            
            _currentIndex++;
        }

        private void HandleEnemyDeath()
        {
            DeathEnemyCounter++;
        }
    }
    
    [Serializable]
    public class EnemyInformation
    {
        public List<EnemyRoute> enemyRoute;
        public GameObject enemyObject;
        public float spawnDelay; // 이전의 적이 나온 후, [SpawnDelay]초 기다린 뒤 해당 적이 나오게 됩니다.
        public bool isFlagEnemy; // 해당 적까지 스폰하고 이후 모든 적의 스폰을 멈춥니다. 필드의 적이 모두 죽었다면, 다시 스폰을 시작합니다.
    }
}


