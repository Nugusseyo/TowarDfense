using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Script.Environmental
{
    public class SceneMover : MonoBehaviour
    {
        [SerializeField] private int index = 0;

        public void MoveScene()
        {
            SceneManager.LoadScene(index);
        }
    }
}
