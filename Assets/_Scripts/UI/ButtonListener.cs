using UnityEngine;

namespace _Scripts.UI
{
    public class ButtonListener : MonoBehaviour
    {
        public Sprite map;
        public string stage;
        public int index;
        
        public void ShowMe()
        {
            //이름 너무 대충 지었나;
            ButtonInfo.Instance.ButtonViewer(this);
        }
    }
}
