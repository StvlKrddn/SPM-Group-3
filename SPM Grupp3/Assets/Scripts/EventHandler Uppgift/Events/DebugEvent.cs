using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEvent : Event
{
    public string Message;

    public DebugEvent(string description, string message) : base(description)
    {
        Message = message;
    }
}
