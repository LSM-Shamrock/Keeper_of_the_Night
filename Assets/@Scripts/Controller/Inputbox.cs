using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputbox : BaseController
{
    Canvas _canvas;
    InputField _inputField;
    public void Init()
    {
        _canvas = GetComponent<Canvas>();
        _inputField = GetComponentInChildren<InputField>();

        _canvas.worldCamera = Camera.main;
    }

    public IEnumerator ShowAndWaitInput(Action<string> action)
    {
        _inputField.gameObject.SetActive(true);

        _inputField.Select();

        yield return WaitUntil(() => Manager.Input.IsPressedEnter);

        action?.Invoke(_inputField.text);

        _inputField.gameObject.SetActive(false);
    }
}
