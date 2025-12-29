using System.Collections;
using UnityEngine;

public class MoonlightSwordShieldController : CharacterSkillController
{
    private GameObject _swordBody;
    private SpriteRenderer _swordBodySR;
    private Collider2D _swordBodyCol;
    private Sprite _sprite_Droping;
    private Sprite _sprite_StuckInTheGround;

    private SpriteRenderer[] _shieldEffect;
    private float[] _shieldEffectDefaultAlpha;

    private bool IsContactGround => _swordBodyCol.IsContact(PlaySceneObjects.Ground);
    private bool IsSleepground => Manager.Game.currentCharacter == Characters.Sleepground;

    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        _swordBody = transform.GetChild(0).gameObject;
        _swordBodySR = _swordBody.GetComponent<SpriteRenderer>();
        _swordBodyCol = _swordBody.GetComponent<Collider2D>();

        _shieldEffect = new SpriteRenderer[3];
        _shieldEffectDefaultAlpha = new float[_shieldEffect.Length];
        _shieldEffect[0] = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _shieldEffect[1] = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _shieldEffect[2] = transform.GetChild(3).GetComponent<SpriteRenderer>();
        for (int i = 0; i < _shieldEffect.Length; i++)
            _shieldEffectDefaultAlpha[i] = _shieldEffect[i].color.a;

        _sprite_Droping = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_Droping);
        _sprite_StuckInTheGround = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_StuckInTheGround);

        Manager.Game.onDisarmSpecialSkill.Add(this, OnDisarmSpecialSkill);
        StartCoroutine(LoopRelease());
        StartCoroutine(LoopOnSkill());
        StartCoroutine(LoopShieldEffect());
    }

    private void ShowShield()
    {
        for (int i = 0; i < _shieldEffect.Length; i++)
        {
            _shieldEffect[i].SetAlpha(_shieldEffectDefaultAlpha[i]);
            _shieldEffect[i].gameObject.SetActive(true);
        }
    }
    private void HideShield()
    {
        foreach (SpriteRenderer sr in _shieldEffect)
            sr.gameObject.SetActive(false);
    }

    private IEnumerator LoopShieldEffect()
    {
        while (true)
        {
            foreach (int i in Count(100))
            {
                foreach (SpriteRenderer sr in _shieldEffect)
                    sr.AddTransparency(0.001f);

                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(100))
            {
                foreach (SpriteRenderer sr in _shieldEffect)
                    sr.AddTransparency(-0.001f);

                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void OnDisarmSpecialSkill()
    {
        if (IsSleepground == false)
            return;

        HideShield();
        _swordBody.SetActive(false);
        Manager.Game.isSpecialSkillInvoking = false;
        Manager.Game.skillCooltime = 0.5f;
    }

    private IEnumerator LoopRelease()
    {
        while (true)
        {
            yield return null;
            if (IsSleepground == false)
                continue;

            if (Manager.Game.isSpecialSkillInvoking)
            {
                foreach (int i in Count(150))
                {
                    yield return new WaitForSeconds(0.1f);
                    if (!Manager.Game.isSpecialSkillInvoking) break;
                }

                if (Manager.Game.isSpecialSkillInvoking)
                    Manager.Game.onDisarmSpecialSkill.Call();
            }
        }
    }
    private IEnumerator LoopOnSkill()
    {
        while (true)
        {
            yield return new WaitUntil(() => IsSleepground);

            if (Manager.Game.isSpecialSkillInvoking)
            {
                // 일반공격시 스킬 해제
                if (Manager.Input.isOnSkill || Manager.Input.isDragAttack)
                {
                    Manager.Game.onDisarmSpecialSkill.Call();
                    yield return new WaitUntil(() => !Manager.Input.isOnSkill);
                }
                continue;
            }

            //  스킬 발동
            if (Manager.Input.isOnSkill && Manager.Game.skillCooltime <= 0f)
            {
                Manager.Game.isSpecialSkillInvoking = true;

                _swordBodySR.sprite = _sprite_Droping;
                _swordBody.SetActive(true);

                transform.position = Manager.Object.Character.position + Vector3.up * 30f;

                yield return new WaitUntil(() => !IsContactGround);
                while (!IsContactGround)
                {
                    transform.position += Vector3.down * 5f;
                    yield return new WaitForFixedUpdate();
                }
                transform.position += Vector3.down * 5f;
                _swordBodySR.sprite = _sprite_StuckInTheGround;
                ShowShield();

                yield return new WaitForSeconds(0.5f);

                yield return new WaitUntil(() => !Manager.Input.isOnSkill);
            }
        }

    }
}
