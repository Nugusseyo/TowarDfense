using _Script.Tools.Utility;
using UnityEngine;

namespace _Scripts.Managers.InfoM
{
    public class MapDataHolder : MonoSingleton<MapDataHolder>
    {
        [field: SerializeField] public MapData HoldData { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void SetMapData(MapData mapData)
        {
            if (mapData != null && HoldData != mapData)
                HoldData = mapData;
            else if(mapData == null)
            {
                HoldData = null;
                Debug.Log("데이터 없음");
            }
            else
                Debug.Log("데이터 미확인");
        }

        public void ChangeNextData()
        {
            Debug.Log("Data Change!!");
            if (HoldData.NextMapData == null)
            {
                return;
            }
            HoldData = HoldData.NextMapData;
        }
    }
}
