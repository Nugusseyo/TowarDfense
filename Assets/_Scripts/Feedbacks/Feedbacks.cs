using System;
using System.Collections.Generic;
using System.Linq;
using _Script.Agent.Modules;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Feedbacks
{
    public class Feedbacks : MonoBehaviour, IModule
    {
        private Dictionary<FeedbackType, FeedbackPlayer> FeedbackPlayers = new Dictionary<FeedbackType, FeedbackPlayer>();
        public void Initialize(ModuleAgent moduleAgent)
        {
        }

        private void Start()
        {
            FeedbackPlayers = GetComponentsInChildren<FeedbackPlayer>().ToDictionary(x => x.FeedbackType, x => x);
        }

        public FeedbackPlayer GetFeedbackPlayer(FeedbackType feedbackType)
        {
            if (!FeedbackPlayers.ContainsKey(feedbackType))
            {
                Debug.LogError("안에 대상이 없는데요?");
            }
            return FeedbackPlayers.GetValueOrDefault(feedbackType);
        }
        
    }

    [BlackboardEnum][Serializable]
    public enum FeedbackType
    {
        LANDING,
        UPGRADE
    }
}
