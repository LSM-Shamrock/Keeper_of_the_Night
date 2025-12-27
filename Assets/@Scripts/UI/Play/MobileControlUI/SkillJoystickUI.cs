using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillJoystickUI : UIBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ChildKey<Image> Boddy = new(nameof(Boddy));
    private ChildKey<Image> Handle = new(nameof(Handle));

    private Vector3 _defaultPosition;
    private float _joystickRadius;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        bool isCooltime = Manager.Game.SpecialSkillCooltime > 0;

        GetChild(Boddy).enabled = !isCooltime;
        GetChild(Handle).color = isCooltime ? new Color(0.4f,0.4f,0f) : Color.yellow;
    }

    private void Init()
    {
        BindChild(
        Boddy,
        Handle);

        _defaultPosition = GetChild(Boddy).transform.position;
        _joystickRadius = GetChild(Boddy).rectTransform.sizeDelta.y / 2f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Boddy).transform.position = worldPosition;

        Manager.Input.isOnSkillJoystick = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Boddy).transform.position = _defaultPosition;
        GetChild(Handle).transform.localPosition = Vector3.zero;

        Manager.Input.isOnSkillJoystick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        bool isCooltime = Manager.Game.SpecialSkillCooltime > 0;
        if (isCooltime)
            return;

        Transform body = GetChild(Boddy).transform;
        Transform handle = GetChild(Handle).transform;

        Vector3 worldPos = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 dragVec = worldPos - body.position;
        Vector3 dir = dragVec.normalized;
        float dist = Mathf.Min(dragVec.magnitude, _joystickRadius);

        handle.localPosition = dir * dist;
        Manager.Input.skillJoystickVector = handle.localPosition / _joystickRadius;
    }
}
