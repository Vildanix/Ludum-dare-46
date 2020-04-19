using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Unity Learn event manager base
 */
public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, UnityEvent<int>> intEventDictionary;
    private Dictionary<string, UnityEvent<string>> stringEventDictionary;
    private Dictionary<string, UnityEvent<UnityAction<int>>> callbackEventDictionary;

    [System.Serializable]
    private class StringtEvent : UnityEvent<string>
    {
    }

    [System.Serializable]
    private class IntEvent : UnityEvent<int>
    {
    }

    [System.Serializable]
    private class CallbackEvent : UnityEvent<UnityAction<int>>
    {
    }

    // keep single instance
    private static EventManager eventManager;

    public static EventManager Instance {
        get {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }

        if (intEventDictionary == null)
        {
            intEventDictionary = new Dictionary<string, UnityEvent<int>>();
        }

        if (stringEventDictionary == null)
        {
            stringEventDictionary = new Dictionary<string, UnityEvent<string>>();
        }

        if (callbackEventDictionary == null)
        {
            callbackEventDictionary = new Dictionary<string, UnityEvent<UnityAction<int>>>();
        }
    }

    public static void RegisterListener(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListener(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void RegisterListenerText(string eventName, UnityAction<string> listener)
    {
        if (Instance.stringEventDictionary.TryGetValue(eventName, out UnityEvent<string> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new StringtEvent();
            thisEvent.AddListener(listener);
            Instance.stringEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListenerText(string eventName, UnityAction<string> listener)
    {
        if (eventManager == null) return;
        UnityEvent<string> thisEvent = null;
        if (Instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void RegisterListenerNumber(string eventName, UnityAction<int> listener)
    {
        if (Instance.intEventDictionary.TryGetValue(eventName, out UnityEvent<int> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new IntEvent();
            thisEvent.AddListener(listener);
            Instance.intEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListenerNumber(string eventName, UnityAction<int> listener)
    {
        if (eventManager == null) return;
        UnityEvent<int> thisEvent = null;
        if (Instance.intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void RegisterListenerCallback(string eventName, UnityAction<UnityAction<int>> listener)
    {
        if (Instance.callbackEventDictionary.TryGetValue(eventName, out UnityEvent<UnityAction<int>> thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new CallbackEvent();
            thisEvent.AddListener(listener);
            Instance.callbackEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void RemoveListenerCallback(string eventName, UnityAction<UnityAction<int>> listener)
    {
        if (eventManager == null) return;
        UnityEvent<UnityAction<int>> thisEvent = null;
        if (Instance.callbackEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void DispatchEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
        else {
            Debug.Log("Triggered event without any registred listenres. Event name: " + eventName);
        }
    }

    public static void DispatchEventWithText(string eventName, string textValue)
    {
        UnityEvent<string> thisEvent = null;

        if (!Instance.stringEventDictionary.ContainsKey(eventName)) return;

        if (Instance.stringEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(textValue);
        }
        else
        {
            Debug.Log("Triggered event without any registred listenres. Event name: " + eventName);
        }
    }

    public static void DispatchEventWithNumber(string eventName, int value)
    {
        UnityEvent<int> thisEvent = null;
        if (Instance.intEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
        else
        {
            Debug.Log("Triggered event without any registred listenres. Event name: " + eventName);
        }
    }

    public static void DispatchEventWithCallback(string eventName, UnityAction<int> callback)
    {
        UnityEvent<UnityAction<int>> thisEvent = null;
        if (Instance.callbackEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(callback);
        }
        else
        {
            Debug.Log("Triggered event without any registred listenres. Event name: " + eventName);
        }
    }
}
