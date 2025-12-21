using UnityEngine;

public class InputManager 
{
    public bool IsMouseClicked => Input.GetMouseButton(0);
    public bool IsPressedW => Input.GetKey(KeyCode.W);
    public bool IsPressedA => Input.GetKey(KeyCode.A);
    public bool IsPressedS => Input.GetKey(KeyCode.S);
    public bool IsPressedD => Input.GetKey(KeyCode.D);
    public bool IsPressedN => Input.GetKey(KeyCode.N);
    public bool IsPressedT => Input.GetKey(KeyCode.T);
    public bool IsPressedEnter => Input.GetKey(KeyCode.Return);
}
