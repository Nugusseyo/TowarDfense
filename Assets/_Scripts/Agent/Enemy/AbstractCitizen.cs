using UnityEngine;

namespace _Scripts.Agent.Enemy
{
    public class AbstractCitizen : Agent
    {
        public override void TakeHeal(int healAmount)
        {
            base.TakeHeal(healAmount);
            Debug.Log("나 힐받았어!!!");
        }
    }
}
