using _Scripts.Managers.InfoM;
using UnityEngine;

namespace _Scripts.UI
{
    public class ButtonListener : MonoBehaviour
    {
        public MapData mapData;
        
        public void ShowMe()
        {
            //이름 너무 대충 지었나;
            ButtonInfo.Instance.ButtonViewer(mapData);
            MapDataHolder.Instance.SetMapData(mapData);
            Debug.Log("데이터 전달 완료");
        }
    }
}
