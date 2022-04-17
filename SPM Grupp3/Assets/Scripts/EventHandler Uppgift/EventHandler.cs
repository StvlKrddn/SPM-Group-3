using System;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler instance;

    delegate void EventListener(Event info);
    Dictionary<Type, List<EventListener>> eventListeners = new Dictionary<Type, List<EventListener>>();

    public static EventHandler Instance { 
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<EventHandler>();
            }
            return instance; 
        } 
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            print("More than one EventHandler! GameObject: " + gameObject.name);
        }
    }

    public void RegisterListener<T>(Action<T> listener) where T : Event
    {
        // Get the type of Event
        Type eventType = typeof(T);

        // If the dictionary hasn't been declared yet
        if (eventListeners == null)
        {
            eventListeners = new Dictionary<Type, List<EventListener>>();
        }

        // If the event hasn't been registered before
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<EventListener>();
        }

        // Wrap listener call to return an EventListener
        EventListener wrapper = (eventInfo) => { listener((T)eventInfo); };

        // Add listener to the Event
        eventListeners[eventType].Add(wrapper);
    }

    public void UnregisterListener<TGenericEventType>(Action<TGenericEventType> listener) where TGenericEventType : Event
    {
        // Get the type of event passed in
        Type eventType = typeof(TGenericEventType);

        // If the event is not in the dictionary or the list of listeners is empty, there's nothing to unregister
        if (!eventListeners.ContainsKey(eventType) || eventListeners[eventType].Count == 0)
        {
            return;
        }

        // Wrap listener call to return an EventListener
        EventListener wrapper = (eventInfo) => { listener((TGenericEventType)eventInfo); };

        // Remove listener from the list in dictionary
        eventListeners[eventType].Remove(wrapper);
    }

    public void InvokeEvent(Event eventInfo)
    {
        // Get the type of event
        Type eventClass = eventInfo.GetType();

        if (eventListeners == null || eventListeners[eventClass] == null)
        {
            // No one to shout at
            return;
        }

        // Shout at anyone listening
        foreach (EventListener listener in eventListeners[eventClass])
        {
            listener(eventInfo);
        }
    }
}
