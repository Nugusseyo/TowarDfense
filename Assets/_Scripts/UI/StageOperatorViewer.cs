using System;
using System.Collections.Generic;
using _Script.Tools.Utility;
using UnityEngine;
using UnityEngine.Video;

namespace _Scripts.UI
{
    public class StageOperatorViewer : MonoSingleton<StageOperatorViewer>
    {
        [SerializeField] private UnityEngine.Video.VideoPlayer videoPlayer;
        [SerializeField] private GameObject videoPopUp;

        private void Start()
        {
            StopTutorialVideo();
        }

        public void PopUpVideos(VideoClip clip)
        {
            if (clip == null || videoPopUp == null || videoPlayer == null)
            {
                Debug.Log("필수 값 누락됨");
                if(clip == null) Debug.Log("클립 재조정 필요");
                if(videoPopUp == null) Debug.Log("팝업 재조정 필요");
                if(videoPlayer == null) Debug.Log("비디오 플레이어 재조정 필요");
                return;
            }
            
            videoPlayer.Stop();
            if (videoPlayer.targetTexture != null)
            {
                videoPlayer.targetTexture.Release();
            }
            
            videoPopUp.SetActive(true);
            videoPlayer.isLooping = true;
            videoPlayer.clip = clip;
            videoPlayer.Play();
        }
        
        public void StopTutorialVideo()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }
            videoPopUp.SetActive(false);
        }
    }
}
