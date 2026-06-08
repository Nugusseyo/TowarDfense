using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    // 툴 인스펙터에서 입력받을 임시 변수들
    public string newItemName = "New Item";
    public int newItemID = 1000;
    public ItemData.ItemType newItemType = ItemData.ItemType.Weapon;
    public int newValue = 10;
}