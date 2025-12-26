using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputbox : MonoBehaviour
{
    Canvas _canvas;
    InputField _inputField;

    public void Init()
    {
        _canvas = GetComponent<Canvas>();
        _inputField = GetComponentInChildren<InputField>();

        _canvas.worldCamera = Manager.Object.MainCamera;
    }

    public IEnumerator ShowAndWaitInput(Action<string> action)
    {
        _inputField.gameObject.SetActive(true);

        _inputField.Select();

        yield return new WaitUntil(() => Manager.Input.isOnKeyEnter);

        action?.Invoke(_inputField.text);

        _inputField.gameObject.SetActive(false);
    }
}
