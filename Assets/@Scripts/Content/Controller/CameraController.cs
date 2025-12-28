using UnityEngine;

public class CameraController : BaseController
{
    private Camera _camera;

    protected override void Start()
    {
        base.Start();

        _camera = GetComponent<Camera>();
    }

    protected override void Update()
    {
        base.Update();

        float camHalfW = _camera.orthographicSize * _camera.aspect;
        float xLimit = Define.CharacterXLimit - camHalfW;

        Vector3 pos = transform.position;
        pos.x = Manager.Object.Character.position.x;
        pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
        transform.position = pos;
    }
}
