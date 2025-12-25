using UnityEngine;

public class InputManager 
{
    public bool isMobileControl
    {
        get
        {
            return Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }

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
    public Vector3 mousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public Vector3 mouseDirection => (mousePosition - Manager.Game.Character.position).normalized;
    public Vector3 attackDirection => isMobileControl ? attackJoystickDirection : mouseDirection;

    public bool isPressedAttackJoystick;
    public bool isPressedMouse => Input.GetMouseButton(0);
    public bool isPressedAttack => isMobileControl ? isPressedAttackJoystick : isPressedMouse;
    public bool isDragAttack => isPressedAttack && attackDirection != Vector3.zero;
}
