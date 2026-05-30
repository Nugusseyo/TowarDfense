using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Agent UI data",  menuName = "Agent/UI data")]
public class AgentUIDataSO : ScriptableObject
{
    [Header("Left UI")] 
    public Sprite positionTypeIcon;
    public string agentName;
    
    [Header("Skills")]
    public Sprite skillIcon;
    public string skillDesc;
}
