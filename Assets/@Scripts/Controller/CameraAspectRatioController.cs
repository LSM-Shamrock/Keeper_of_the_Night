using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatioController : MonoBehaviour
{
    [SerializeField] 
    private float _targetAspectRatio = 16f / 9f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.clearFlags = CameraClearFlags.SolidColor;
    }

    private void Update()
    {
        SetAspectRatio(_targetAspectRatio);
    }

    private void SetAspectRatio(float targetAspectRatio)
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;

        Rect rect = new Rect();
        if (screenAspectRatio < targetAspectRatio)
        {
            // 가로에 맞춰 세로를 줄임
            rect.width = 1f;
            rect.height = screenAspectRatio / targetAspectRatio;
        }
        else
        {
            // 세로에 맞춰 가로를 줄임
            rect.height = 1f;
            rect.width = targetAspectRatio / screenAspectRatio;
        }
        rect.center = Vector2.one / 2f;
        _camera.rect = rect;
    }
}