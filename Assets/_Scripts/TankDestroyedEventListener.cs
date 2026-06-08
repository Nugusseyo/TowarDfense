using UnityEngine;
using UnityEngine.Events;

public class TankDestroyedEventListener : MonoBehaviour
{
    public TankDestroyedEvent EventChannel;
    public UnityEvent<BaseTankUnit> Response;

    private void OnEnable()
    {
        if (EventChannel != null)
            EventChannel.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (EventChannel != null)
            EventChannel.UnregisterListener(this);
    }

    public void OnEventRaised(BaseTankUnit destroyedTank)
    {
        if (Response != null)
            Response.Invoke(destroyedTank);
    }
}