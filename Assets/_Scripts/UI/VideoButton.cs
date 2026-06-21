using UnityEngine;
using UnityEngine.Video;

namespace _Scripts.UI
{
    public class VideoButton : MonoBehaviour
    {
        [SerializeField] private VideoClip clip;
        public void PlayMyVideo()
        {
            StageOperatorViewer.Instance.PopUpVideos(clip);
        }
    }
}
