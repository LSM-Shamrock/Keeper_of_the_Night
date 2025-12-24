using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Transform _background;
    [SerializeField] private Transform _handler;

    private Camera _mainCamera;

    private float _joystickRadius;
    private Vector2 _defaultPosition;

    private Vector2 _moveDir;
    private Vector2 _touchPosition;
    
    private Vector2 TouchWorldPosition
    {
        get
        {
            return _mainCamera.ScreenToWorldPoint(_touchPosition);
        }
    }
    


    private void Start()
    {
        _mainCamera = Camera.main;
        _joystickRadius = ((RectTransform)_background).sizeDelta.y / 2;
        _defaultPosition = _background.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchPosition = eventData.position;
        _background.position = TouchWorldPosition;
        _handler.position = TouchWorldPosition;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;

        _background.position = _defaultPosition;
        _handler.localPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 eventWorldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
        Vector2 touchVec = eventWorldPosition - TouchWorldPosition;

        _moveDir = touchVec.normalized;
        _handler.localPosition = _moveDir * Mathf.Min(touchVec.magnitude, _joystickRadius);
    }
}
