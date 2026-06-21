using System.Collections;
using _Script.ScriptableObject.Event;
using _Scripts.Managers.InfoM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.UI
{
    public class SceneChanger : MonoBehaviour
    {
        public int index;
        [SerializeField] private float second;
        [SerializeField] private EventChannelSO uiEventChannel;
        [SerializeField] private bool isChangeNextData = true;

        public void NextScene()
        {
            StopAllCoroutines();
            if(isChangeNextData && MapDataHolder.Instance != null)
                MapDataHolder.Instance.ChangeNextData();
            
            if (MapDataHolder.Instance != null && MapDataHolder.Instance.HoldData == null)
            {
                uiEventChannel.RaiseEvent(UIEvents.AlarmUI.Init("다음 스테이지가 없습니다."));
                return;
            }
            
            StartCoroutine(SceneChange());
        }

        private IEnumerator SceneChange()
        {
            yield return null;
            float elapsedTime = 0;
            while (elapsedTime < second)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (isChangeNextData)
                SceneManager.LoadScene(MapDataHolder.Instance.HoldData.Index);
            else 
                SceneManager.LoadScene(index);
        }

        public void NextStage()
        {
            int curIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(curIdx + 1);
        }

        public void Restart()
        {
            int curIdx = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(curIdx);
        }
        public void SelectScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
