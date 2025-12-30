using System.Collections;
using UnityEngine;

public class CharacterSkill_MoonlightGun : BaseController
{
    private GameObject _child;
    private Vector3 _direction;
    private SpriteRenderer _sr;
    private Sprite _spriteR;
    private Sprite _spriteL;

    protected override void Start()
    {
        Init();
    }

    protected override void Update()
    {
        base.Update();
        UpdateGun();
    }

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _spriteR = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_MoonlightGun_Right);
        _spriteL = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_MoonlightGun_Left);
        StartCoroutine(LoopShot());
    }

    private void Shoot()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.CharacterSkill_MoonlightgunBullet);
        var go = prefab.CreateClone();
        go.transform.position = transform.position;
        var bullet = go.Component<CharacterSkill_MoonlightGunBullet>();
        bullet.Shoot(transform.position, _direction);
    }

    private IEnumerator LoopShot()
    {
        while (true)
        {
            yield return new WaitUntil(() => Manager.Game.CurrentCharacter == Characters.Dino);

            if (!Manager.Game.IsSpecialSkillInvoking && Manager.Input.isDragAttack)
            {
                Shoot();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void UpdateGun()
    {
        if (Manager.Game.CurrentCharacter == Characters.Dino && !Manager.Game.IsSpecialSkillInvoking)
        { 
            _child.SetActive(true);

            Vector3 characterPos = Manager.Object.Character.position;
            transform.position = characterPos + Vector3.up * -10f;

            _direction = Manager.Input.attackDirection;

            _sr.sprite = _direction.x < 0 ? _spriteL : _spriteR;

            transform.rotation = Util.Direction2Rotation(_direction);
            transform.position += _direction * 25f;
        }
        else
        {
            _child.SetActive(false);
        }
    }

}
