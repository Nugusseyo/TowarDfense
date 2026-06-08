using UnityEngine.InputSystem;

public class TankUnit : BaseTankUnit
{
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) Attack();
    }
}