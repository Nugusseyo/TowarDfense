using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.UI
{
    public class SceneChanger : MonoBehaviour
    {
        public int index;
        [SerializeField] private float second;

        public void NextScene()
        {
            StopAllCoroutines();
            StartCoroutine(SceneChange());
        }

        private IEnumerator SceneChange()
        {
            float elapsedTime = 0;
            while (elapsedTime < second)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            SceneManager.LoadScene(index);
        }
    }
}
