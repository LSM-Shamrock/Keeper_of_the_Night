using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<(Type, Enum), UnityEngine.Object> _childs = new Dictionary<(Type, Enum), UnityEngine.Object>();


    protected void BindChild<T>(Enum childName) where T : UnityEngine.Object
    {
        Transform child = Utility.FindChild(transform, childName.ToString());
        if (child == null)
        {
            Debug.Log($"바인딩 실패! {transform}에게서 {childName}이름의 자식을 찾지 못함");
            return;
        }

        if (typeof(T) == typeof(GameObject))
        {
            _childs.Add((typeof(GameObject), childName), child.gameObject);
            return;
        }
        if (typeof(T) == typeof(Transform))
        {
            _childs.Add((typeof(Transform), childName), child);
            return;
        }

        T component = child.GetComponent<T>();
        if (component == null)
        {
            Debug.Log($"바인딩 실패! {child}에게서 {typeof(T)}컴포넌트를 찾지 못함");
            return;
        }
        _childs.Add((typeof(T), childName), component);
    }
    protected void BindChildren<T, TEnum>() where T : UnityEngine.Object where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));

        foreach (TEnum value in values)
            BindChild<T>(value);
    }
    protected T GetChild<T>(Enum childName) where T : UnityEngine.Object
    {
        if (_childs.TryGetValue((typeof(T), childName), out UnityEngine.Object obj))
        {
            return (T)obj;
        }
        else
        {
            Debug.Log($"에러! {typeof(T)}타입의 {name}가 바인딩 되지 않음");
            return null;
        }
    }

    protected GameObject GetGameObject(Enum childName) => GetChild<GameObject>(childName);
    protected Transform GetTransform(Enum childName) => GetChild<Transform>(childName);
    protected Image GetImage(Enum childName) => GetChild<Image>(childName);
    protected Text GetText(Enum childName) => GetChild<Text>(childName);


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
