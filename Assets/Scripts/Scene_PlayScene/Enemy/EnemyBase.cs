using System.Collections;
using UnityEngine;

public abstract class EnemyBase : PlaySceneObjectBase
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
    protected bool IsContactMoonlightgunBullet => _col.IsContact(Prefabs.Scene_PlayScene.CharacterSkill_MoonlightgunBullet);
    protected bool IsContactBossDinoSkill => _col.IsContact(PlaySceneObjects.BossDinoSkill);
    protected bool IsContactCameraLight => _col.IsContact(Prefabs.Scene_PlayScene.CharacterSkill_CameraFlash);

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
        Destroy(gameObject);
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
        StartCoroutine(Loop_ContactCharacterSkill_AttackSkill());
        StartCoroutine(Loop_ContactCharacterSkill_MoonlightswordShield());
        StartCoroutine(Loop_ContactCharacterSkill_WatterPrison());
    }

    protected abstract IEnumerator WhenTakingDamage(int damage);
    IEnumerator Loop_ContactCharacterSkill_AttackSkill()
    {
        while (true)
        {
            if (IsContactMoonlightSword)
            {
                yield return WhenTakingDamage(3);
            }
            if (IsContactMoonlightgunBullet)
            {
                yield return WhenTakingDamage(4);
                LookAtTheTarget(Character);
                MoveToMoveDirection(-5);
            }
            if (IsContactBossDinoSkill)
            {
                if (wave == 7) healthInDream += 2;
                else remainingHealth += 2;
                yield return WhenTakingDamage(3);
            }
            if (IsContactWater)
            {
                yield return WhenTakingDamage(2);
                LookAtTheTarget(Character);
                MoveToMoveDirection(-4);
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_ContactCharacterSkill_MoonlightswordShield()
    {
        while (true)
        {
            if (IsContactMoonlightswordShield)
            {
                if (transform.GetX() < MoonlightswordShield.GetX())
                {
                    foreach (int i in Count(10))
                    {
                        transform.AddX(-5f);
                        yield return waitForFixedUpdate;
                    }
                }
                if (transform.GetX() > MoonlightswordShield.GetX())
                {
                    foreach (int i in Count(10))
                    {
                        transform.AddX(5f);
                        yield return waitForFixedUpdate;
                    }
                }
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_ContactCharacterSkill_WatterPrison()
    {
        while (true)
        {
            if (IsContactWaterPrison && isSpecialSkillInvoking)
            {
                while (IsContactWaterPrison)
                {
                    yield return transform.MoveToPositionOverTime(0.1f, WaterPrison.position);
                    yield return waitForFixedUpdate;
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
