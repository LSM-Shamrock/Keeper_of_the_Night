using UnityEngine;

public enum ControlType
{
    Application,
    PC,
    Mobile,
}

public class InputManager 
{
    public readonly ActionEx onControlTypeChange = new();
    private ControlType _controlType = ControlType.Application;
    public ControlType controlType
    {
        get { return _controlType; }
        set { _controlType = value; onControlTypeChange.Call(); }
    }

    public bool isMobileControl => 
        controlType == ControlType.Mobile || 
        controlType == ControlType.Application && Application.isMobilePlatform;

    public bool isPressedS => Input.GetKey(KeyCode.S);

    public bool isPressedJumpButton;
    public bool isPressedJumpKey => Input.GetKey(KeyCode.W);
    public bool isPressedJump => isPressedJumpButton || isPressedJumpKey;

    public bool isPressedLeftButton;
    public bool isPressedLeftKey => Input.GetKey(KeyCode.A);
    public bool isPressedLeft => isPressedLeftButton || isPressedLeftKey;
    
    public bool isPressedRightButton;
    public bool isPressedRightKey => Input.GetKey(KeyCode.D);
    public bool isPressedRight => isPressedRightButton || isPressedRightKey;
    
    public bool isPressedN => Input.GetKey(KeyCode.N);
    public bool isPressedT => Input.GetKey(KeyCode.T);
    public bool isPressedEnter => Input.GetKey(KeyCode.Return);

    public Vector3 attackJoystickDirection;
    public Vector3 mousePosition => Manager.Object.MainCamera.ScreenToWorldPoint(Input.mousePosition);
    public Vector3 mouseDirection => (mousePosition - Manager.Object.Character.position).normalized;
    public Vector3 attackDirection => isMobileControl ? attackJoystickDirection : mouseDirection;

    public bool isPressedAttackJoystick;
    public bool isPressedMouse => Input.GetMouseButton(0);
    public bool isPressedAttack => isMobileControl ? isPressedAttackJoystick : isPressedMouse;
    public bool isDragAttack => isPressedAttack && attackDirection != Vector3.zero;
}
