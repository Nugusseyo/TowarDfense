using UnityEngine;

// 이 속성이 있으면 원래 유니티 메뉴에서 생성이 가능하지만, 
// 우리는 툴을 통해 만들 것이므로 주석 처리하거나 빼도 됩니다.
public class ItemData : ScriptableObject
{
    public string itemName;
    public int itemID;
    public enum ItemType { Weapon, Armor, Potion }
    public ItemType type;
    public int value;
}