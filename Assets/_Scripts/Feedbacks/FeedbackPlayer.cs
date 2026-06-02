using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Feedbacks
{
    public class FeedbackPlayer : MonoBehaviour
    {
        public UnityEvent StartFeedback;
        public UnityEvent StopFeeedback;
        public FeedbackType FeedbackType;
        
        public void FeedbackPlay() => StartFeedback?.Invoke();
        public void FeedbackStop() => StopFeeedback?.Invoke();
    }
}