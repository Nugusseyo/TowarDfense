using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    public void ShowDestroyedTank(BaseTankUnit destroyedTank)
    {
        if (destroyedTank != null)
        {
            Debug.Log($"테스트: {destroyedTank.gameObject.name}이(가) 파괴되었습니다!");
        }
    }
}