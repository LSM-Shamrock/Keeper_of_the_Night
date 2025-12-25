using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystickUI : UIBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ChildKey<RectTransform> Boddy = new(nameof(Boddy));
    private ChildKey<RectTransform> Handle = new(nameof(Handle));

    private Camera _mainCamera;
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

        _mainCamera = Camera.main;
        _defaultPosition = GetChild(Boddy).position;
        _joystickRadius = GetChild(Boddy).sizeDelta.y / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Boddy).position = worldPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Boddy).position = _defaultPosition;
        GetChild(Handle).localPosition = Vector3.zero;

        Manager.Input.isPressedAttackJoystick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 dragVec = worldPosition - GetChild(Boddy).position;
        Vector3 dir = dragVec.normalized;
        float dist = Mathf.Min(dragVec.magnitude, _joystickRadius);

        GetChild(Handle).localPosition = dir * dist;

        Manager.Input.attackDirection = dir;
        if (dist > 0) Manager.Input.isPressedAttackJoystick = true;
    }
}
