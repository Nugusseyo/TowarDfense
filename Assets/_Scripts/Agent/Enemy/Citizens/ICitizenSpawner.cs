using UnityEngine;

namespace _Scripts.Agent.Enemy.Citizens
{
    public interface ICitizenSpawner
    {
        GameObject SummonCitizen(Agent citizen);
    }
}