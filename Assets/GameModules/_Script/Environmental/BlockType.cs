using UnityEngine;

namespace _Script.Environmental
{
    public class BlockType : MonoBehaviour
    {
        [field:SerializeField] public BlockTypeEnum BlockTypeEnum { get; private set; }
    }

    public enum BlockTypeEnum
    {
        FLOOR,
        HILL,
        BARRIER
    }
}
