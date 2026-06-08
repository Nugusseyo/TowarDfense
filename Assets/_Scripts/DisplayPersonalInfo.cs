using UnityEngine;

public class DisplayPersonalInfo : MonoBehaviour
{
    public PersonalData personalData;
    
    void Start()
    {
        if (personalData != null)
        {
            Debug.Log("이름: " + personalData.myName);
            Debug.Log("나이: " + personalData.myAge);
        }
        else
        {
            Debug.LogError("PersonalData가 할당되지 않았습니다.");
        }
    }
}
