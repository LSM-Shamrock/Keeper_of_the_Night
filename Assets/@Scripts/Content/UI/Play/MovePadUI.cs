using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovePadUI : UIBase, IPointerDownHandler, IPointerUpHandler
{
    ChildKey<Transform> Body = new(nameof(Body));
    
    private Vector3 _defaultLocalPos;

    private MovePadButton[] _movePadButtons;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindChild(
        Body);

        _defaultLocalPos = GetChild(Body).localPosition;

        _movePadButtons = GetComponentsInChildren<MovePadButton>();

        foreach (var button in _movePadButtons)
        {
            button.image.alphaHitTestMinimumThreshold = 0.1f;
            BindEvent(button.image, EventType.PointerEnter, () =>
            {
                button.image.transform.localScale *= 1.5f;
                if (button.isJump) Manager.Input.onJumpButton = true;
                if (button.isLeft) Manager.Input.isOnLeftButton = true;
                if (button.isRight) Manager.Input.isOnRightButton = true;
            });
            BindEvent(button.image, EventType.PointerExit, () =>
            {
                button.image.transform.localScale /= 1.5f;
                if (button.isJump) Manager.Input.onJumpButton = false;
                if (button.isLeft) Manager.Input.isOnLeftButton = false;
                if (button.isRight) Manager.Input.isOnRightButton = false;
            });
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = Manager.Object.MainCamera.ScreenToWorldPoint(eventData.position);
        GetChild(Body).position = worldPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Body).localPosition = _defaultLocalPos;
    }
}
