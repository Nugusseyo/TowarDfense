using _Scripts.Agent.Player;
using UnityEngine;

namespace _Scripts.Agent.Tower
{
    [CreateAssetMenu(fileName = "new Tower Info", menuName = "Agent/TowerInfo", order = 0)]
    public class TowerLevelInfoSO : ScriptableObject
    {
        public GameObject[] levelVisuals;
    }
}