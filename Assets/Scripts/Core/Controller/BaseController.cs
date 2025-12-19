using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;

public class BaseController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected float DistanceTo(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance;
    }

    // 흐름
    protected float FixedDeltaTime => Time.fixedDeltaTime;
    protected readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected WaitForSeconds WaitForSeconds(float seconds)
    {
        return new WaitForSeconds(seconds);
    }
    protected WaitUntil WaitUntil(Func<bool> predicate)
    {
        return new WaitUntil(predicate);
    }
    protected WaitWhile WaitWhile(Func<bool> predicate)
    {
        return new WaitWhile(predicate);
    }
    protected IEnumerable<int> Count(int count)
    {
        for (int i = 0; i < count; i++)
            yield return i;
    }

    // 입력 
    protected bool IsContactMousePointer { get; private set; }
    public virtual void OnPointerEnter(PointerEventData eventData) 
    { 
        IsContactMousePointer = true; 
    }
    public virtual void OnPointerExit(PointerEventData eventData) 
    { 
        IsContactMousePointer = false; 
    }


    private Transform FindChild(Transform root, string name)
    {
        if (root == null)
            return null;

        foreach (Transform child in root)
        {
            if (child.name == name) 
                return child;

            Transform rec = FindChild(child, name);
            if (rec != null) 
                return rec;
        }
        return null;
    }

    public void BindChild<TComponent, TEnum>(Transform root) where TComponent : Component where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
        
        foreach (TEnum value in values)
        {
            Transform t = FindChild(root, value.ToString());
            if (t != null)
            {
                TComponent component = t.GetComponent<TComponent>();
                if (component != null)
                {

                }
                else
                {
                    Debug.Log($"바인딩 실패!\n{root}의 자식{value}에서 {typeof(TComponent).Name} 컴포넌트를 찾을 수 없음");
                }
            }
            else
            {
                Debug.Log($"바인딩 실패!\n{root}의 자식에서 {value}을 찾을 수 없음");
            }
        }
    }

}