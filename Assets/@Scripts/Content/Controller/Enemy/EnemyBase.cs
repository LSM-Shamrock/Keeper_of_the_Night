using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : BaseController
{
    protected SpriteRenderer _sr;
    protected Collider2D _col;
    protected AudioSource _audioSource;
    protected Vector3 _moveDirection;

    protected bool IsContactWall => _col.IsContact(PlaySceneObjects.Wall);
    protected bool IsContactGround => _col.IsContact(PlaySceneObjects.Ground);
    protected bool IsContactCharacter => _col.IsContact(PlaySceneObjects.Character);
    protected bool IsContactMoonlightSword => _col.IsContact(PlaySceneObjects.MoonlightSword);
    protected bool IsContactMoonlightswordShield => _col.IsContact(PlaySceneObjects.MoonlightswordShield);
    protected bool IsContactWater => _col.IsContact(PlaySceneObjects.Water);
    protected bool IsContactWaterPrison => _col.IsContact(PlaySceneObjects.WaterPrison);
    protected bool IsContactMoonlightgunBullet => _col.IsContact(Prefabs.Play.CharacterSkill_MoonlightgunBullet);
    protected bool IsContactBossDinoSkill => _col.IsContact(PlaySceneObjects.BossDinoSkillParticle);
    protected bool IsContactCameraLight => _col.IsContact(Prefabs.Play.CharacterSkill_CameraFlash);
    protected bool isSkillIgnore = false;

    protected void Show()
    {
        _col.enabled = true;
        _sr.enabled = true;
    }
    protected void Hide()
    {
        _col.enabled = false;
        _sr.enabled = false;
    }
    protected virtual void DeleteThisClone()
    {
        Manager.Object.Despawn(gameObject);
    }
    protected void MoveToMoveDirection(float amount)
    {
        transform.position += _moveDirection * amount;
    }
    protected void LookAtTheTarget(Transform target)
    {
        _moveDirection = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.Euler(Vector3.up * (_moveDirection.x < 0 ? 180 : 0));
    }

    public virtual void Init()
    {
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(Loop_Contact_AttackSkill());
        StartCoroutine(Loop_Contact_MoonlightswordShield());
        StartCoroutine(Loop_Contact_WatterPrison());
    }

    protected abstract IEnumerator WhenTakingDamage(int damage);

    IEnumerator Loop_Contact_AttackSkill()
    {
        while (true)
        {
            yield return null;
            if (isSkillIgnore) continue;

            if (IsContactMoonlightSword)
                yield return WhenTakingDamage(3);

            if (IsContactMoonlightgunBullet)
            {
                yield return WhenTakingDamage(4);
                LookAtTheTarget(Manager.Object.Character);
                MoveToMoveDirection(-5);
            }

            if (IsContactBossDinoSkill)
            {
                if (Manager.Game.wave == 7) Manager.Game.dreamHealth += 2;
                else Manager.Game.health += 2;
                yield return WhenTakingDamage(3);
            }

            if (IsContactWater)
            {
                yield return WhenTakingDamage(2);
                LookAtTheTarget(Manager.Object.Character);
                MoveToMoveDirection(-4);
            }
        }
    }
    IEnumerator Loop_Contact_MoonlightswordShield()
    {
        while (true)
        {
            yield return null;
            if (isSkillIgnore) continue;

            if (IsContactMoonlightswordShield)
            {
                float shieldX = Manager.Object.MoonlightswordShield.position.x;
                int xDir = -Math.Sign(shieldX - transform.position.x);
                foreach (int i in Count(10))
                {
                    transform.AddX(xDir * 5f);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
    IEnumerator Loop_Contact_WatterPrison()
    {
        while (true)
        {
            yield return null;
            if (isSkillIgnore) continue;

            if (IsContactWaterPrison && Manager.Game.isSpecialSkillInvoking)
            {
                while (IsContactWaterPrison)
                {
                    yield return transform.MoveToPositionOverTime(0.1f, Manager.Object.WaterPrison.position);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}
