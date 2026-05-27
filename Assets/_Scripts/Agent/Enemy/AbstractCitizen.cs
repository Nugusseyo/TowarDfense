using UnityEngine;

namespace _Scripts.Agent.Enemy
{
    public class AbstractCitizen : Agent
    {
        public override bool TryCasting()
        {
            return true;
        }
    }
}
