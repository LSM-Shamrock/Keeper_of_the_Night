using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EventType
{
    PointerClick,
    PointerDown, 
    PointerUp,
}

public class EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private Dictionary<EventType, ActionEx> events = new();

    public ActionEx GetEvent(EventType type)
    {
        if (events.ContainsKey(type) == false)
            events.Add(type, new ActionEx());

        return events[type];
    }

    private void Call(EventType type)
    {
        if (events.TryGetValue(type, out ActionEx action))
            action?.Call();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Call(EventType.PointerClick);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Call(EventType.PointerDown);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Call(EventType.PointerUp);
    }
}
