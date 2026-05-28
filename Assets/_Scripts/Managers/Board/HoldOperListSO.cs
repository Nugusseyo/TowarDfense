using System.Collections.Generic;
using _Scripts.Agent.Player;
using UnityEngine;

namespace _Scripts.Managers.Board
{
    [CreateAssetMenu(fileName = "new Hold Op List SO", menuName = "System/Map System/Hold Op List")]
    public class HoldOperListSO : ScriptableObject
    {
        [SerializeField] private List<AbstractOperator> operators = new List<AbstractOperator>();

        public AbstractOperator GetOperator(int index)
        {
            return operators[index];
        }
    }
}
