using System;
using UnityEngine;
using UnityEngine.UI;

public class MovePadButton : MonoBehaviour
{
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isLeft;
    [SerializeField] private bool _isRight;

    public bool isJump => _isJump;
    public bool isLeft => _isLeft;
    public bool isRight => _isRight;

    private Image _image; 
    public Image image
    {
        get
        {
            if (_image == null) 
                _image = GetComponentInChildren<Image>();
            return _image;
        }
    }
}
