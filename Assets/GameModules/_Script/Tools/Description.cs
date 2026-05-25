using UnityEngine;

namespace _Script.Tools
{
    public class Description : MonoBehaviour // 객체를 설명하기 위해 사용하는 Dummy Class
    {
        [TextArea][SerializeField] public readonly string description;
    }
}