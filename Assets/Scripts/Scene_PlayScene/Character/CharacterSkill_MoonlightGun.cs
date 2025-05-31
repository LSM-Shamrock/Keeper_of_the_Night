using System.Collections;
using UnityEngine;

public class CharacterSkill_MoonlightGun : PlaySceneObjectBase
{
    GameObject _child;
    Vector3 _direction;
    SpriteRenderer _sr;
    Sprite _spriteR;
    Sprite _spriteL;

    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _spriteR = Utile.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_MoonlightGun_Right);
        _spriteL = Utile.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_MoonlightGun_Left);
        StartCoroutine(Loop_Update());
        StartCoroutine(Loop_Shot());
    }

    void Shoot()
    {
        var prefab = Utile.LoadResource<GameObject>(Prefabs.Scene_PlayScene.CharacterSkill_MoonlightgunBullet);
        var go = prefab.CreateClone();
        go.transform.position = transform.position;
        var bullet = go.Component<CharacterSkill_MoonlightgunBullet>();
        bullet.Shoot(transform.position, _direction);
    }

    IEnumerator Loop_Shot()
    {
        while (true)
        {
            yield return WaitUntil(() => currentCharacter == Sprites.Characters.Dino);

            if (!isSpecialSkillInvoking && IsMouseClicked)
            {
                Shoot();
                yield return WaitForSeconds(0.5f);
            }

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Update()
    {
        while (true)
        {
            if (currentCharacter == Sprites.Characters.Dino && !isSpecialSkillInvoking)
            { 
                _child.SetActive(true);
                transform.position = Character.position;
                transform.position += Vector3.up * -10f;

                bool flip = Utile.MouseX < transform.GetX();
                _sr.sprite = flip ? _spriteL : _spriteR;
                _direction = (Utile.MousePosition - transform.position).normalized;
                transform.rotation = Utile.Direction2Rotation(_direction);
                transform.position += _direction * 25f;
            }
            else
            {
                _child.SetActive(false);
            }

            yield return waitForFixedUpdate;
        }
    }

}
