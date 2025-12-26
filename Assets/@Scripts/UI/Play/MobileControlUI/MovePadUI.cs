using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovePadUI : UIBase, IPointerDownHandler, IPointerUpHandler
{
    ChildKey<Transform> Boddy = new(nameof(Boddy));
    
    private Vector3 _defaultPosition;

    private MovePadButton[] _movePadButtons;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindChild(
        Boddy);

        _defaultPosition = GetChild(Boddy).position;

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
        GetChild(Boddy).position = worldPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetChild(Boddy).position = _defaultPosition;
    }
}
