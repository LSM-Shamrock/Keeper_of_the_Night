using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ChildKey
{
    public readonly string name;
    public readonly Type type;
    public ChildKey(string name, Type type)
    {
        this.name = name;
        this.type = type;
    }
    public override string ToString()
    {
        return name;
    }
}
public class ChildKey<T> : ChildKey where T : UnityEngine.Object
{
    public ChildKey(string name) : base(name, typeof(T)) { }
}


public class UIBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<ChildKey, UnityEngine.Object> _childs = new();

    protected void BindChild(ChildKey key) 
    {
        Transform child = Util.FindChild(transform, key.name);
        if (child == null)
        {
            Debug.Log($"바인딩 실패! {transform}에게서 {key.name}이름의 자식을 찾지 못함");
            return;
        }

        if (key.type == typeof(GameObject))
        {
            _childs.Add(key, child.gameObject);
            return;
        }

        Component component = child.GetComponent(key.type);
        if (component == null)
        {
            Debug.Log($"바인딩 실패! {child}에게서 {key.type}컴포넌트를 찾지 못함");
            return;
        }
        _childs.Add(key, component);
    }
    protected void BindChild(params ChildKey[] keys)
    {
        foreach (ChildKey key in keys) 
            BindChild(key);
    }
    protected T GetChild<T>(ChildKey<T> key) where T : UnityEngine.Object
    {
        if (_childs.TryGetValue(key, out var obj))
        {
            return (T)obj;
        }
        else
        {
            Debug.Log($"에러! {typeof(T)}타입의 {key.name}가 바인딩 되지 않음");
            return null;
        }
    }

    protected void BindEvent(GameObject go, EventType eventType, Action action)
    {
        EventHandler handler = go.GetOrAddComponent<EventHandler>();
        handler.GetEvent(eventType).Add(this, action);
    }
    protected void BindEvent(Component component, EventType eventType, Action action)
    {
        BindEvent(component.gameObject, eventType, action);
    }
    protected void BindEvents(GameObject go, Action<EventType> action)
    {
        EventHandler handler = go.GetOrAddComponent<EventHandler>();
        EventType[] eventTypes = (EventType[])Enum.GetValues(typeof(EventType));
        foreach (EventType eventType in eventTypes)
        {
            EventType type = eventType;
            handler.GetEvent(type).Add(this, () => action?.Invoke(type));
        }
    }
    protected void BindEvents(Component component, Action<EventType> action)
    {
        BindEvents(component.gameObject, action);
    }


    protected bool IsContactMousePointer { get; private set; }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        IsContactMousePointer = true;
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        IsContactMousePointer = false;
    }
}
