using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.Agent.Tower;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Agent.Enemy.Citizens
{
    public class CreatureService : MonoSingleton<CreatureService>
    {
        [SerializeField] private GameObject citizenSpawnerGameObject;
        [SerializeField] private GameObject towerSpawnerGameObject;
        [field:SerializeField] public CreatureListSO creatureListSO { get; private set; }
        public List<CreatureInfo> Creatures = new List<CreatureInfo>();
        public Action<int> OnWaveChanged;

        [Header("List Generate To SO")] 
        [SerializeField] private string soName;
        
        private Queue<CreatureInfo> sortedCreatures = new Queue<CreatureInfo>();
        private List<GameObject> _activeCreatures = new List<GameObject>();
        private ICitizenSpawner _citizenSpawner;
        private ITowerSpawner _towerSpawner;

        protected override void Awake()
        {
            base.Awake();
            if (!citizenSpawnerGameObject.TryGetComponent(out ICitizenSpawner _))
            {
                Debug.LogError("Citizen Spawner가 없습니다.");
                return;
            }

            if (!towerSpawnerGameObject.TryGetComponent(out ITowerSpawner _))
            {
                Debug.LogError("TowerSpawner가 없습니다.");
                return;
            }
            if (creatureListSO != null && Creatures.Count > 0)
            {
                Debug.LogWarning("CreatureManager에서, SO와 List 둘 다 존재합니다. List만을 사용하겠습니다.");
                return;
            }
            if (creatureListSO != null)
            {
                //새로 안만들어주면 리스트가 손상된다.
                //알고싶지 않았음.
                Creatures = new List<CreatureInfo>(creatureListSO.CreatureInfos);
            }
            _citizenSpawner = citizenSpawnerGameObject.GetComponent<ICitizenSpawner>();
            _towerSpawner = towerSpawnerGameObject.GetComponent<ITowerSpawner>();
        }

        private void Start()
        {
            StartSummon();
        }

        public void StartSummon()
        {
            List<CreatureInfo> creatures = new List<CreatureInfo>(Creatures);
            creatures = creatures.OrderBy(a => a.wave)
                .ThenBy(a => a.spawnTime)
                .ToList();
            
            //wave로 1차 정렬, spawnTime으로 2차 정렬
            //이걸 Queue로 받아오면? 싹싹김치 딱딱 야르

            sortedCreatures = new Queue<CreatureInfo>(creatures);
            SummonEnemyWave(0);
            _towerSpawner.SpawnTower(0);
            OnWaveChanged?.Invoke(0);
        }

        private void SummonEnemyWave(int wave)
        {
            if (sortedCreatures.Count <= 0) return;
            Queue<CreatureInfo> creatures = new Queue<CreatureInfo>();
            while (sortedCreatures.Count > 0 && sortedCreatures.Peek().wave == wave)
            {
                creatures.Enqueue(sortedCreatures.Dequeue());
            }
            if(creatures.Count > 0)
                StartCoroutine(CreatureSpawn(creatures, wave));
        }

        private IEnumerator CreatureSpawn(Queue<CreatureInfo> q, int wave)
        {
            float startTime = Time.time;
            CreatureInfo creatureInfo = q.Peek();
            while (q.Count > 0)
            {
                creatureInfo = q.Peek();
                if (startTime + creatureInfo.spawnTime <= Time.time) //소환에 성공했다.
                {
                    //그럼 다음 애로 넘겨줘야 함.
                    if (creatureInfo.creature.TryGetComponent(out Agent agent))
                    {
                        GameObject creature = _citizenSpawner.SummonCitizen(agent);
                        if(creature != null)
                            _activeCreatures.Add(creature);
                    }
                    else
                        Debug.LogWarning("Agent가 아닌데 CreatureService에서 스폰하려고 시도했습니다!");
                    
                    q.Dequeue();
                }

                yield return null;
            }

            while (_activeCreatures.Count > 0)
            {
                _activeCreatures.RemoveAll(x => x == null);
                yield return null;
            }

            int nextWave = wave + 1;
            _towerSpawner.SpawnTower(nextWave);
            OnWaveChanged?.Invoke(nextWave);
            yield return new WaitForSeconds(2f);
            SummonEnemyWave(nextWave);
        }


#if UNITY_EDITOR
        [ContextMenu("Generate To SO")]
        public void GenerateListToSO()
        {
            if (soName == null)
            {
                Debug.LogError("SO의 Name이 없습니다!");
                return;
            }
            CreatureListSO newListSO = ScriptableObject.CreateInstance<CreatureListSO>();
            newListSO.CreatureInfos = Creatures;
            //일단 하나 꼬@롬하게 인스턴스 만들어주고.
            
            newListSO.name = soName;

            string folderPath = @"Assets/GameModules/Map data/CreatureInfo";
            string finalPath = $"{folderPath}/{soName}.asset";
            
            finalPath = AssetDatabase.GenerateUniqueAssetPath(finalPath);
            //Unique한 에셋 Path로 고쳐줌.
            //만약 같은 이름이 있으면 1, 2를 뒤에 붙여주는것처럼.
            
            AssetDatabase.CreateAsset(newListSO, finalPath);
            //파일 생성
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            creatureListSO = newListSO;
            Creatures = new List<CreatureInfo>();
        }
        #endif
    }

    [Serializable]
    public class CreatureInfo
    {
        public GameObject creature;
        public int wave;
        
        [Tooltip("웨이브가 시작된 이후로, 해당 시간이 되면 스폰됩니다.")]
        public float spawnTime;
    }
}
