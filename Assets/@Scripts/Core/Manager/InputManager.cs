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


    public bool isOnJumpButton;
    public bool isOnJumpKey => Input.GetKey(KeyCode.W);
    public bool isOnJump => isOnJumpButton || isOnJumpKey;

    public Vector3 moveJoystickDirection;
    public Vector3 moveKeyDirection
    {
        get
        {
            Vector3 result = Vector3.zero;
            if (Input.GetKey(KeyCode.A)) result += Vector3.left;
            if (Input.GetKey(KeyCode.D)) result += Vector3.right;
            return result;
        }
    }
    public Vector3 moveDirection => (moveJoystickDirection + moveKeyDirection).normalized;

    public bool isOnKeyN => Input.GetKey(KeyCode.N);
    public bool isOnKeyT => Input.GetKey(KeyCode.T);
    public bool isOnKeyEnter => Input.GetKey(KeyCode.Return);

    public Vector3 mousePosition => Manager.Object.MainCamera.ScreenToWorldPoint(Input.mousePosition);
    public Vector3 mouseDirection => (mousePosition - Manager.Object.Character.position).normalized;

    public Vector3 attackJoystickVector;
    public Vector3 attackJoystickDirection => attackJoystickVector.normalized;
    public Vector3 attackDirection => isMobileControl ? attackJoystickDirection : mouseDirection;
    public bool isOnAttackJoystick;
    public bool isOnMouse => Input.GetMouseButton(0);
    public bool isOnAttack => isMobileControl ? isOnAttackJoystick : isOnMouse;
    public bool isDragAttack => isOnAttack && attackDirection != Vector3.zero;

    public Vector3 skillJoystickVector;
    public Vector3 skillJoystickDirection => skillJoystickVector.normalized;
    public Vector3 skillDirection => isMobileControl ? skillJoystickDirection : mouseDirection;
    public bool isOnSkillJoystick;
    public bool isOnKeyS => Input.GetKey(KeyCode.S);
    public bool isOnSkill => isMobileControl ? isOnSkillJoystick : isOnKeyS;
    public bool isDragSkill => isOnSkill && attackDirection != Vector3.zero;
}
