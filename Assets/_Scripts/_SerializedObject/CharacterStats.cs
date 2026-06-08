using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName = "New Hero";
    [Range(0, 100)] public int health = 100;
    public Color teamColor = Color.white;
}