
using System.Collections;
using UnityEngine;
using static Utility;

public class CharacterSkill_Camera : BaseController
{
    private GameObject _child;
    private Vector3 _direction;
    private SpriteRenderer _sr;
    private Sprite _sprite_right;
    private Sprite _sprite_left;


    protected override void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        UpdatePositionAndDircetion();
    }

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _sprite_right = LoadResource<Sprite>(Sprites.CharacterSkill.Suhyen_Camera_Right);
        _sprite_left = LoadResource<Sprite>(Sprites.CharacterSkill.Suhyen_Camera_Left);
        StartCoroutine(LoopRoutine());
    }

    private void Flash()
    {
        var prefab = LoadResource<GameObject>(Prefabs.Scene_Play.CharacterSkill_CameraFlash);
        var go = prefab.CreateClone();
        var flash = go.GetComponent<CharacterSkill_CameraFlash>();
        flash.Init(transform.position, _direction);
    }

    private IEnumerator LoopRoutine()
    {
        while (true)
        {
            _child.SetActive(false);
            yield return new WaitUntil(() => Manager.Game.currentCharacter == CharacterType.Suhyen);
            
            _child.SetActive(true);
            if (Manager.Input.isPressedAttack)
            {
                Flash();
                yield return new WaitForSeconds(0.1f);
                yield return new WaitUntil(() => !Manager.Input.isPressedAttack);
            }
        }
    }

    private void UpdatePositionAndDircetion()
    {
        transform.position = Manager.Game.Character.position;
        transform.position += Vector3.up * -10f;

        bool flip = MouseX < transform.GetX();
        _sr.sprite = flip ? _sprite_left : _sprite_right;

        _direction = (MousePosition - transform.position).normalized;
        transform.rotation = Utility.Direction2Rotation(_direction);
        transform.position += _direction * 25f;
    }
}
