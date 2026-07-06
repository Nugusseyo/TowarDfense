using UnityEngine;

namespace _Scripts.UI
{
    public class URLGuideUI : MonoBehaviour
    {
        public void GuideURL(string targetURL)
        {
            Application.OpenURL(targetURL);
        }
    }
}
