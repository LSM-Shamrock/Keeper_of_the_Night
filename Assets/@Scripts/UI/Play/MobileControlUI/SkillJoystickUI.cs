using UnityEngine;
using UnityEngine.EventSystems;

public class SkillJoystickUI : UIBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ChildKey<RectTransform> Boddy = new(nameof(Boddy));
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
        Boddy,
        Handle);

        _defaultPosition = GetChild(Boddy).position;
        _joystickRadius = GetChild(Boddy).sizeDelta.y / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Boddy).position = worldPosition;

        Manager.Input.isOnSkillJoystick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Boddy).position = _defaultPosition;
        GetChild(Handle).localPosition = Vector3.zero;

        Manager.Input.isOnSkillJoystick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Transform body = GetChild(Boddy);
        Transform handle = GetChild(Handle);

        Vector3 worldPos = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 dragVec = worldPos - body.position;
        Vector3 dir = dragVec.normalized;
        float dist = Mathf.Min(dragVec.magnitude, _joystickRadius);

        handle.localPosition = dir * dist;
        Manager.Input.skillJoystickVector = handle.localPosition / _joystickRadius;
    }
}
