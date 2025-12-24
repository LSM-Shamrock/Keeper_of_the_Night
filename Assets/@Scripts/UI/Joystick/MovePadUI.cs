using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovePadUI : UIBase, IPointerDownHandler, IPointerUpHandler
{
    ChildKey<Transform> MovePadBoddy = new(nameof(MovePadBoddy));
    ChildKey<Transform> JumpButton = new(nameof(JumpButton));
    ChildKey<Transform> LeftButton = new(nameof(LeftButton));
    ChildKey<Transform> RightButton = new(nameof(RightButton));



    private Vector3 _defaultPosition;

    private Camera _mainCamera;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Bind(
        MovePadBoddy,
        JumpButton,
        LeftButton,
        RightButton);

        _mainCamera = Camera.main;

        _defaultPosition = Get(MovePadBoddy).position;

        


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);

        Get(MovePadBoddy).position = worldPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Get(MovePadBoddy).position = _defaultPosition;
    }
}
