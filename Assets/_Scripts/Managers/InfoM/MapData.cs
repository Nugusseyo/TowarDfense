using _Scripts.Managers.Board;
using UnityEngine;

namespace _Scripts.Managers.InfoM
{
    [CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
    public class MapData : ScriptableObject
    {
        [field: SerializeField] public Sprite Map { get; private set; }
        [field: SerializeField] public string Stage { get; private set; }
        [field: SerializeField] public int Index { get; private set; }
        [field: SerializeField] public string SubTitle { get; private set; }
        [field: SerializeField] public int WaveCount { get; private set; }
        [field: SerializeField] public int EnemyCount { get; private set; }
        [field: SerializeField] public int LifeCount { get; private set; }
        [field: SerializeField] public int BigStageNumber { get; private set; }
        [field: SerializeField] public int SmallStageNumber { get; private set; }
        [field: SerializeField] public HoldOperListSO HoldOperatorList { get; private set; }
        [field: SerializeField] public MapData NextMapData { get; private set; }
    }
}
