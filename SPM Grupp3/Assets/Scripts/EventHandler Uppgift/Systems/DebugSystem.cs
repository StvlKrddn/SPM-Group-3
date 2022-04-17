using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSystem : MonoBehaviour
{
    private void Start()
    {
        // If any DebugEvent is invoked, call the PrintMessage-method
        EventHandler.Instance.RegisterListener<DebugEvent>(PrintMessage);
    }

    void PrintMessage(DebugEvent eventInfo)
    {
        Debug.Log(eventInfo.Description + eventInfo.Message);
    }
}
