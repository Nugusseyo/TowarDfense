using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Agent UI data",  menuName = "Agent/UI data")]
public class AgentUIDataSO : ScriptableObject
{
    [Header("Button data")]
    public Sprite portrait;
    
    [Header("Left UI")] 
    public string agentName;
    
    [Header("Skills")]
    public Sprite skillIcon;
    [TextArea] public string skillDesc;

    [Header("Cost UI")] 
    public int cost;

    [Header("Other")]
    public string health;

    public PlayerType playerType;

}

public enum PlayerType
{
    HEALER,
    TANKER,
    SURPPORT
}
