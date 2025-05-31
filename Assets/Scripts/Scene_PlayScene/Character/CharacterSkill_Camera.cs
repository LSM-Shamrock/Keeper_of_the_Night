
using System.Collections;
using UnityEngine;
using static Utile;

public class CharacterSkill_Camera : PlaySceneObjectBase
{
    GameObject _child;
    Vector3 _direction;
    SpriteRenderer _sr;
    Sprite _sprite_right;
    Sprite _sprite_left;


    protected override void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        Update_PositionAndDircetion();
    }

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _sprite_right = LoadResource<Sprite>(Sprites.CharacterSkill.Suhyen_Camera_Right);
        _sprite_left = LoadResource<Sprite>(Sprites.CharacterSkill.Suhyen_Camera_Left);
        StartCoroutine(Loop_Routine());
    }

    void Flash()
    {
        var prefab = LoadResource<GameObject>(Prefabs.Scene_PlayScene.CharacterSkill_CameraFlash);
        var go = prefab.CreateClone();
        var flash = go.GetComponent<CharacterSkill_CameraFlash>();
        flash.Init(transform.position, _direction);
    }

    IEnumerator Loop_Routine()
    {
        while (true)
        {
            _child.SetActive(false);
            yield return WaitUntil(() => currentCharacter == Sprites.Characters.Suhyen);
            
            _child.SetActive(true);
            if (IsMouseClicked)
            {
                Flash();
                yield return WaitForSeconds(0.1f);
                yield return WaitUntil(() => !IsMouseClicked);
            }

            yield return waitForFixedUpdate;
        }
    }

    void Update_PositionAndDircetion()
    {
        transform.position = Character.position;
        transform.position += Vector3.up * -10f;

        bool flip = MouseX < transform.GetX();
        _sr.sprite = flip ? _sprite_left : _sprite_right;

        _direction = (MousePosition - transform.position).normalized;
        transform.rotation = Utile.Direction2Rotation(_direction);
        transform.position += _direction * 25f;
    }
}
