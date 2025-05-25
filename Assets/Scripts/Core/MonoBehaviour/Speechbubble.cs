using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speechbubble : ObjectBase
{
    Text _text;
    Transform _master;
    public void Init(Transform master)
    { 
        _text = GetComponentInChildren<Text>(); 
        _master = master; 
    }
    public void Show(string text)
    {
        _text.text = text;
        UpdatePosition();
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false); 
    }
    protected void UpdatePosition()
    {
        if (_master == null)
            return;

        Vector3 half = ((RectTransform)transform).rect.size / 2f;

        transform.position = _master.position;
        Vector3 showDirection = Vector3.zero;
        showDirection.x = transform.position.x < 0 ? 1 : -1;
        showDirection.y = transform.position.y < 0 ? 1 : -1;
        transform.position += showDirection * 40f;
        transform.position += Vector3.Scale(showDirection, half);
    }
    private void FixedUpdate()
    {
        if (_master == null)
            Destroy(gameObject);
        UpdatePosition();
    }
}
