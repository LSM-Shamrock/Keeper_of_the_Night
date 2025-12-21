using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<(Type, Enum), Component> _components = new Dictionary<(Type, Enum), Component>();
    protected void Bind<TComponent>(Enum childName) where TComponent : Component
    {
        Transform child = Utility.FindChild(transform, childName.ToString());
        if (child == null)
        {
            Debug.Log($"바인딩 실패! {transform}에게서 {childName}이름의 자식을 찾지 못함");
            return;
        }

        TComponent component = child.GetComponent<TComponent>();
        if (component == null)
        {
            Debug.Log($"바인딩 실패! {child}에게서 {typeof(TComponent)}컴포넌트를 찾지 못함");
            return;
        }

        _components.Add((typeof(TComponent), childName), component);
    }
    protected void Bind<TComponent, TEnum>() where TComponent : Component where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));

        foreach (TEnum value in values)
        {
            Bind<TComponent>(value);
        }
    }
    protected TComponent Get<TComponent>(Enum childName) where TComponent : Component
    {
        if (_components.TryGetValue((typeof(TComponent), childName), out Component component))
        {
            return (TComponent)component;
        }
        else
        {
            Debug.Log($"에러! {typeof(TComponent)}타입의 {name}가 바인딩 되지 않음");
            return null;
        }
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
