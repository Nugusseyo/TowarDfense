using UnityEngine;

namespace _Scripts.UI
{
    public class QuitButton : MonoBehaviour
    {
        public void GameExit()
        {
            Application.Quit();
        }
    }
}
