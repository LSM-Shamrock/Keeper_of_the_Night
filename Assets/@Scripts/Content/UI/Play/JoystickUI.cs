using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickUI : UIBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ChildKey<Image> Body = new(nameof(Body));
    private ChildKey<Image> Handle = new(nameof(Handle));
    private float _joystickRadius;
    private Vector3 _defaultLocalPos;

    public Action onPointerDownAction { get; set; }
    public Action onPointerUpAction { get; set; }
    public Action<Vector3> onDragAction { get; set; }

    public bool isDragable { get; set; } = true;
    public bool isBodyFollowHandle { get; set; } = false;

    public Image bodyImage => GetChild(Body);
    public Image handleImage => GetChild(Handle);

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindChild(Body, Handle);
        _defaultLocalPos = GetChild(Body).transform.localPosition;
        _joystickRadius = GetChild(Body).rectTransform.sizeDelta.y / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Body).transform.position = worldPosition;

        onPointerDownAction?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Body).transform.localPosition = _defaultLocalPos;
        GetChild(Handle).transform.localPosition = Vector3.zero;
        
        onPointerUpAction?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragable == false)
            return;

        Transform body = GetChild(Body).transform;
        Transform handle = GetChild(Handle).transform;

        Vector3 worldPos = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 dragVec = worldPos - body.position;
        Vector3 dir = dragVec.normalized;
        float dist = dragVec.magnitude;

        handle.localPosition = dir * Mathf.Min(dist, _joystickRadius);
        if (isBodyFollowHandle) body.position += worldPos - handle.position;

        Vector3 joystickVector = handle.localPosition / _joystickRadius;
        onDragAction?.Invoke(joystickVector);
    }
}
