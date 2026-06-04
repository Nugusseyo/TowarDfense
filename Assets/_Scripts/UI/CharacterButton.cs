using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class CharacterButton : MonoBehaviour
    {
        [field: SerializeField] public Image Portrait { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TopText { get; private set; }
    }
}
