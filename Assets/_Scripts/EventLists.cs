using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{
    public class EventLists : MonoBehaviour
    {
        public List<UnityEvent> eventList;

        public void InvokeEvent(int index)
        {
            if(eventList.Count > index)
                eventList[index]?.Invoke();
        }
    }
}
