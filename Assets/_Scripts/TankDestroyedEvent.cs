using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankDestroyedEvent", menuName = "Scriptable Objects/TankDestroyedEvent")]
public class TankDestroyedEvent : ScriptableObject
{
    private readonly List<TankDestroyedEventListener> _listeners = new();

    public void RegisterListener(TankDestroyedEventListener listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void UnregisterListener(TankDestroyedEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Raise(BaseTankUnit destroyedTank)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventRaised(destroyedTank);
        
        /*
            리스트를 뒤에서부터 앞으로 거꾸로 순환하는 이유는, 루프 반복 중에 리스너가 
            자신을 리스트에서 제거하더라도 인덱스 오류나 누락이 발생하지 않도록 하기위함
        */
    }
}