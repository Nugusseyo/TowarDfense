using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Script.ScriptableObject.Datas.OperatorData
{
    [CreateAssetMenu(fileName = "new Level Data", menuName = "Save/Level Data", order = 0)]
    public class LevelDataSO : UnityEngine.ScriptableObject
    {
        [Serializable]
        public struct LevelData
        {
            public int CurrentLevel;
            public int RequireExp;
        }

        public List<LevelData> levelDataList;

        public int GetRequiredExp(int currentLevel)
        {
            int targetIndex = levelDataList.FindIndex(levelData => levelData.CurrentLevel == currentLevel);
            return targetIndex < 0 ? -1 : levelDataList[targetIndex].RequireExp;
        }

        public bool IsMaxLevel(int currentLevel)
        {
            return levelDataList.Last().CurrentLevel == currentLevel;
        }

        private void OnValidate()
        {
            if (levelDataList == null) return;
            for (int i = 1; i < levelDataList.Count; i++)
            {
                if (levelDataList[i - 1].CurrentLevel + 1 != levelDataList[i].CurrentLevel)
                {
                    Debug.LogWarning("여기 공부 안해서 다시 해야한다. LevalDataSO의 Level은 반드시 오름차순이어야 함.");
                }
            }
        }
    }
    
}