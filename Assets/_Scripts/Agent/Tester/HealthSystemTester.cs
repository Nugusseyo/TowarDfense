using System;
using _Scripts.Agent;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthSystemTester : MonoBehaviour
{
    [SerializeField] private Agent agent;
    [SerializeField] private int damage = 10;

    private void Update()
    {
        if (agent != null)
        {
            if(Keyboard.current.hKey.wasPressedThisFrame)
                agent.TakeDamage(damage);
        }
    }
    
    public void TakeDam()
    {
        agent.TakeDamage(damage);
    }
}
