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

        [SerializeField] private Transform followTarget;

        private void Awake()
        {
            ViewUIEventChannel.AddListener<AgentOnUI>(HandleAgentOnUI);
            ViewUIEventChannel.AddListener<AgentInfoUI>(HandleAgentInfoUI);
            FollowCam.Follow = followTarget;
        }

        private void OnDestroy()
        {
            ViewUIEventChannel.RemoveListener<AgentOnUI>(HandleAgentOnUI);
            ViewUIEventChannel.AddListener<AgentInfoUI>(HandleAgentInfoUI);
        }

        private void HandleAgentOnUI(AgentOnUI evt)
        {
            if (evt.NextAgent == null) //Default로 돌아가줘야댐.
            {
                FollowCam.Priority = 0;
            }
            else
            {
                Vector3 pos = new Vector3(evt.NextAgent.transform.position.x, 3.5f, evt.NextAgent.transform.position.z);
                followTarget.transform.position = pos;
                FollowCam.Priority = 20;
            }
        }
        private void HandleAgentInfoUI(AgentInfoUI evt)
        {
            FollowCam.Priority = 0;
        }
    }
}
