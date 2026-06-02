using UnityEngine;

namespace _Scripts.Agent
{
    [CreateAssetMenu(fileName = "Enter you tag name!", menuName = "Agent/Tag")]
    public class TagSO : ScriptableObject { [TextArea] [SerializeField] private string description; }
}
