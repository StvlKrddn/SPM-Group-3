using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event
{
    public string Description;

    public Event(string description)
    {
        Description = description;
    }
}
