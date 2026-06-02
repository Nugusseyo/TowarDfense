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
            FeedbackPlayers = GetComponentsInChildren<FeedbackPlayer>().ToDictionary(x => x.FeedbackType, x => x);
        }

        public FeedbackPlayer GetFeedbackPlayer(FeedbackType feedbackType)
        {
            return FeedbackPlayers.GetValueOrDefault(feedbackType);
        }
        
    }

    [BlackboardEnum][Serializable]
    public enum FeedbackType
    {
        LANDING
    }
}
