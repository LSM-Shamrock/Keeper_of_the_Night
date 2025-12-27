using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystickUI : UIBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ChildKey<RectTransform> Body = new(nameof(Body));
    private ChildKey<RectTransform> Handle = new(nameof(Handle));

    private Vector3 _defaultPosition;
    private float _joystickRadius;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindChild(
        Body,
        Handle);

        _defaultPosition = GetChild(Body).position;
        _joystickRadius = GetChild(Body).sizeDelta.y / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Body).position = worldPosition;

        Manager.Input.isOnAttackJoystick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Body).position = _defaultPosition;
        GetChild(Handle).localPosition = Vector3.zero;

        Manager.Input.isOnAttackJoystick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Transform body = GetChild(Body);
        Transform handle = GetChild(Handle);

        Vector3 worldPos = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 dragVec = worldPos - body.position;
        Vector3 dir = dragVec.normalized;
        float dist = Mathf.Min(dragVec.magnitude, _joystickRadius);

        handle.localPosition = dir * dist;
        Manager.Input.attackJoystickVector = handle.localPosition / _joystickRadius;
    }
}
