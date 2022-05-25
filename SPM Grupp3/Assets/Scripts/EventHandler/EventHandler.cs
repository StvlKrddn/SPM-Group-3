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
            // "Lazy loading" to prevent Unity load order error
            if (instance == null)
            {
                instance = FindObjectOfType<EventHandler>();
            }
            return instance; 
        } 
    }

    public void RegisterListener<TEventType>(Action<TEventType> listener) where TEventType : Event
    {
        // Get the type of Event
        Type eventType = typeof(TEventType);

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
        EventListener wrapper = (eventInfo) => { listener((TEventType)eventInfo); };

        // Add listener to the Event
        eventListeners[eventType].Add(wrapper);
    }

    public void UnregisterListener<TEventType>(Action<TEventType> listener) where TEventType : Event
    {
        // Get the type of event passed in
        Type eventType = typeof(TEventType);

        // If the event is not in the dictionary or the list of listeners is empty, there's nothing to unregister
        if (!eventListeners.ContainsKey(eventType) || eventListeners[eventType].Count == 0)
        {
            return;
        }

        // Wrap listener call to return an EventListener
        EventListener wrapper = (eventInfo) => { listener((TEventType)eventInfo); };

        // Remove listener from the list in dictionary
        eventListeners[eventType].Remove(wrapper);
    }

    public void InvokeEvent(Event eventInfo)
    {
        // Get the type of event
        Type eventClass = eventInfo.GetType();

        if (eventListeners.ContainsKey(eventClass))
        {
            // Shout at anyone listening
            foreach (EventListener listener in eventListeners[eventClass])
            {
                listener(eventInfo);
            }
        }

    }
}
