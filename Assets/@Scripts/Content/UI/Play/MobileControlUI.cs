using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MobileControlUI : UIBase
{

    private ChildKey<JoystickUI> MoveJoystick = new(nameof(MoveJoystick));
    private ChildKey<JoystickUI> AttackJoystick = new(nameof(AttackJoystick));
    private ChildKey<JoystickUI> SkillJoystick = new(nameof(SkillJoystick));

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        BindChild(
        MoveJoystick,
        AttackJoystick,
        SkillJoystick);

        GetChild(AttackJoystick).onPointerDownAction = () => Manager.Input.isOnAttackJoystick = true;
        GetChild(AttackJoystick).onPointerUpAction = () => Manager.Input.isOnAttackJoystick = false;
        GetChild(AttackJoystick).onDragAction = (vec) => Manager.Input.attackJoystickVector = vec;

        GetChild(SkillJoystick).onPointerDownAction = () => Manager.Input.isOnSkillJoystick = true;
        GetChild(SkillJoystick).onPointerUpAction = () => Manager.Input.isOnSkillJoystick = false;
        GetChild(SkillJoystick).onDragAction = (vec) => Manager.Input.skillJoystickVector = vec;
        Manager.Game.onSkillCooltimeChange.Add(this, () =>
        {
            bool isCooltime = Manager.Game.SkillCooltime > 0;
            GetChild(SkillJoystick).isDragable = !isCooltime;
            GetChild(SkillJoystick).bodyImage.enabled = !isCooltime;
            GetChild(SkillJoystick).handleImage.color = isCooltime ? new Color(0.4f, 0.4f, 0f) : Color.yellow;
        });

        //GetChild(MoveJoystick).isBodyFollowHandle = true;
        GetChild(MoveJoystick).onPointerUpAction = () =>
        {
            Manager.Input.isOnJumpButton = false;
            Manager.Input.isOnLeftButton = false;
            Manager.Input.isOnRightButton = false;
        };
        GetChild(MoveJoystick).onDragAction = (vec) =>
        {
            Vector3 dir = vec.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            float absAngle = Mathf.Abs(angle);

            bool isJump = absAngle < 60f;
            bool isMove = absAngle > 30f && absAngle < 135f;
            Manager.Input.isOnJumpButton = isJump;
            Manager.Input.isOnLeftButton = isMove && vec.x < 0;
            Manager.Input.isOnRightButton = isMove && vec.x > 0;
        };
    }
}
