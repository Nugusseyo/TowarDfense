using System;
using System.Collections.Generic;
using _Scripts.Agent.Player;
using UnityEngine;

namespace _Scripts.Managers.Board
{
    [CreateAssetMenu(fileName = "new Hold Op List SO", menuName = "System/Map System/Hold Op List")]
    public class HoldOperListSO : ScriptableObject
    {
        [SerializeField] private List<OperatorWrapper> operators = new List<OperatorWrapper>();

        public OperatorWrapper GetOperator(int index)
        {
            return operators[index];
        }
    }
    
    [Serializable]
    public class OperatorWrapper
    {
        public AbstractOperator operatorPrefab;
        public bool isMountain;
    }
}
