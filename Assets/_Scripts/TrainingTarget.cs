using UnityEngine;

public class TrainingTarget : MonoBehaviour
{
    public ElementData elementData;
    
    private void OnCollisionEnter(Collision collision)
    {
        BulletController bulletController = collision.gameObject.GetComponent<BulletController>();
        
        if (bulletController == null) return;

        // Debug.Log(bulletController.elementData == elementData
        //             ? "두 객체의 ElementData가 같습니다."
        //             : "두 객체의 ElementData가 다릅니다.");
        
        // 1
        // // 약점 속성 비교 코드 추가
        // ElementData bulletElement = bulletController.elementData;
        //
        // if (bulletElement.weakness == elementData)
        // {
        //     Debug.Log("탄환보다 내가 더 강함. 저항력이 반영됩니다.");
        // }
        // else if (bulletElement == elementData.weakness)
        // {
        //     Debug.Log("탄환 보다 내가 더 약함. 공격력이 증가합니다.");
        // }
        
        // 2
        ElementData bulletElement = bulletController.elementData;
        elementData.CompareElement( bulletElement);
    }
}
