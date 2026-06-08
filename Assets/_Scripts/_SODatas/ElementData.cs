using UnityEngine;

[CreateAssetMenu(fileName = "ElementData", menuName = "Scriptable Objects/ElementData")]
public class ElementData : ScriptableObject
{
    public ElementData weakness;
    
    public void CompareElement(ElementData targetElement)
    {
        if (targetElement.weakness == this)
        {
            Debug.Log("ElementData: 탄환보다 내가 더 강함. 저항력이 반영됩니다.");
        }
        else if (targetElement == this.weakness)
        {
            Debug.Log("ElementData: 탄환 보다 내가 더 약함. 공격력이 증가합니다.");
        }
    }
}
