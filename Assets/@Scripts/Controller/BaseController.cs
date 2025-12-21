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


}