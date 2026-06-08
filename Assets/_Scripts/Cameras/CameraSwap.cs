using System;
using _Script.ScriptableObject.Event;
using _Scripts.UI;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Cameras
{
    public class CameraSwap : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ViewUIEventChannel { get; private set; }
        [field: SerializeField] public CinemachineCamera DefaultCam { get; private set; }
        [field: SerializeField] public CinemachineCamera FollowCam { get; private set; }

        private void Awake()
        {
            ViewUIEventChannel.AddListener<AgentOnUI>(HandleAgentOnUI);
        }

        private void OnDestroy()
        {
            ViewUIEventChannel.RemoveListener<AgentOnUI>(HandleAgentOnUI);
        }

        private void HandleAgentOnUI(AgentOnUI evt)
        {
            if (evt.NextAgent == null) //Default로 돌아가줘야댐.
            {
                FollowCam.Priority = 0;
            }
            else
            {
                FollowCam.Follow = evt.NextAgent.transform;
                FollowCam.Priority = 20;
            }
        }
        private void HandleAgentInfoUI(AgentInfoUI evt)
        {
            if (evt.Agent == null || evt.IsActive == false)
            {
                FollowCam.Priority = 0;
            }
            else
            {
                FollowCam.Follow = evt.Agent.transform;
                FollowCam.Priority = 20;
            }
        }
    }
}
