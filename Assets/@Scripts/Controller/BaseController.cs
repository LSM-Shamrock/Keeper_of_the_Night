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