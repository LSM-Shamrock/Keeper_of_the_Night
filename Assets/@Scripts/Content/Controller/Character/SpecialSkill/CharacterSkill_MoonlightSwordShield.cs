using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_MoonlightSwordShield : CharacterSkillController
{
    private GameObject _child;
    private SpriteRenderer _blue;
    private SpriteRenderer _yellow;
    private SpriteRenderer _white;

    private bool IsContactGround => _child.Component<Collider2D>().IsContact(PlaySceneObjects.Ground);

    private bool IsSleepground => Manager.Game.currentCharacter == Characters.Sleepground;

    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _blue = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _yellow = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _white = transform.GetChild(3).GetComponent<SpriteRenderer>();
        Manager.Game.onDisarmSpecialSkill.Add(this, () => StartCoroutine(OnDisarmSpecialSkill()));
        StartCoroutine(LoopRelease());
        StartCoroutine(LoopOnSkill());
    }

    private void ShowShield()
    {
        _blue.gameObject.SetActive(true);
        _yellow.gameObject.SetActive(true);
        _white.gameObject.SetActive(true);

        _blue.SetTransparency(0.5f);
        _yellow.SetTransparency(0.75f);
        _white.SetTransparency(0.5f);
    }
    private void HideShield()
    {
        _blue.gameObject.SetActive(false);
        _yellow.gameObject.SetActive(false);
        _white.gameObject.SetActive(false);
    }
    private IEnumerator LoopShieldEffect()
    {
        while (true)
        {
            foreach (int i in Count(100))
            {
                _blue.AddTransparency(0.1f);
                _yellow.AddTransparency(0.1f);
                _white.AddTransparency(0.1f);
                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(100))
            {
                _blue.AddTransparency(-0.1f);
                _yellow.AddTransparency(-0.1f);
                _white.AddTransparency(-0.1f);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator OnDisarmSpecialSkill()
    {
        if (IsSleepground)
        {
            HideShield();
            _child.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            Manager.Game.isSpecialSkillInvoking = false;
            Manager.Game.skillCooltime = 0.5f;
        }
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
        Sprite sprite_Droping = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_Droping);
        Sprite sprite_StuckInTheGround = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_StuckInTheGround);
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
                
                Util.SetSpriteAndPolygon(_child, sprite_Droping);
                _child.SetActive(true);
                
                transform.position = Manager.Object.Character.position + Vector3.up * 30f;

                yield return new WaitUntil(() => !IsContactGround);
                while (!IsContactGround)
                {
                    transform.AddY(-5f);
                    yield return new WaitForFixedUpdate();
                }
                transform.position += Vector3.down * 5f;
                ShowShield();
                Util.SetSpriteAndPolygon(_child, sprite_StuckInTheGround);
                
                yield return new WaitForSeconds(0.5f);

                yield return new WaitUntil(() => !Manager.Input.isOnSkill);
            }
        }

    }
}
