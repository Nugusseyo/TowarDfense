using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Scriptable Objects/TankData")]
public class TankData : ScriptableObject
{
    public Material tankMaterial;
    public float firePower;
    public GameObject projectilePrefab;
}
