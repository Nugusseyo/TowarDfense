using UnityEngine;

namespace GameModules.Operator.BowWow
{
    public class PlayBowArrowAnim : MonoBehaviour
    {
        [SerializeField] private Animator bowAnimator;
        [SerializeField] private Animator arrowAnimator;
        
        private readonly int ATTACK = Animator.StringToHash("ATTACK");
        
        public void PlayAnimation()
        {
            bowAnimator.Play(ATTACK);
            arrowAnimator.Play(ATTACK);
        }
    
    }
}
