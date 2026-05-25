using System;
using System.Collections;
using System.Collections.Generic;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSaver : MonoBehaviour
{
    [field:SerializeField] public EventChannelSO DataSaveEventChannel;

    private TextMeshProUGUI inputText;

    public void EndEditData(string prefKey)
    {
        Debug.Log(prefKey);
        DataSaveEventChannel.RaiseEvent(DataEvents.DataSaveEvent);
    }
}
