using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_MoonlightswordShield : PlaySceneObjectBase
{
    GameObject _child;
    SpriteRenderer _blue;
    SpriteRenderer _yellow;
    SpriteRenderer _white;

    bool IsContactGround => _child.Component<Collider2D>().IsContact(PlaySceneObjects.Ground);

    bool IsSleepground => currentCharacter == Sprites.Characters.Sleepground;

    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _blue = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _yellow = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _white = transform.GetChild(3).GetComponent<SpriteRenderer>();
        onDisarmSpecialSkill.Add(this, () => StartCoroutine(OnDisarmSpecialSkill()));
        StartCoroutine(Loop_Release());
        StartCoroutine(Loop_OnSkill());
    }
   
    void ShowShield()
    {
        _blue.gameObject.SetActive(true);
        _yellow.gameObject.SetActive(true);
        _white.gameObject.SetActive(true);

        _blue.SetTransparency(0.5f);
        _yellow.SetTransparency(0.75f);
        _white.SetTransparency(0.5f);
    }
    void HideShield()
    {
        _blue.gameObject.SetActive(false);
        _yellow.gameObject.SetActive(false);
        _white.gameObject.SetActive(false);
    }
    IEnumerator Loop_ShieldEffect()
    {
        while (true)
        {
            foreach (int i in Count(100))
            {
                _blue.AddTransparency(0.1f);
                _yellow.AddTransparency(0.1f);
                _white.AddTransparency(0.1f);
                yield return waitForFixedUpdate;
            }
            foreach (int i in Count(100))
            {
                _blue.AddTransparency(-0.1f);
                _yellow.AddTransparency(-0.1f);
                _white.AddTransparency(-0.1f);
                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator OnDisarmSpecialSkill()
    {
        if (IsSleepground)
        {
            HideShield();
            _child.SetActive(false);
            yield return WaitForSeconds(0.1f);
            isSpecialSkillInvoking = false;
            specialSkillCooltime = 0.5f;
        }
    }

    IEnumerator Loop_Release()
    {
        while (true)
        {
            yield return WaitUntil(() => IsSleepground);
            if (isSpecialSkillInvoking)
            {
                foreach (int i in Count(150))
                {
                    yield return WaitForSeconds(0.1f);
                    if (!isSpecialSkillInvoking) break;
                }

                if (isSpecialSkillInvoking)
                    onDisarmSpecialSkill.Call();
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_OnSkill()
    {
        Sprite sprite_Droping = Utile.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_Droping);
        Sprite sprite_StuckInTheGround = Utile.LoadResource<Sprite>(Sprites.CharacterSkill.Sleepground_MoonlightswordShield_Sword_StuckInTheGround);
        while (true)
        {
            yield return WaitUntil(() => IsSleepground);

            if (isSpecialSkillInvoking)
            {
                if (IsPressedS || IsMouseClicked)
                {
                    onDisarmSpecialSkill.Call();
                    yield return WaitUntil(() => !IsPressedS);
                }
            }
            else if (IsPressedS && specialSkillCooltime <= 0f)
            {
                isSpecialSkillInvoking = true;
                _child.SetSpriteAndPolygon(sprite_Droping);
                _child.SetActive(true);
                transform.position = Character.position + Vector3.up * 30f;
                yield return WaitUntil(() => !IsContactGround);
                while (!IsContactGround)
                {
                    transform.AddY(-5f);
                    yield return waitForFixedUpdate;
                }
                transform.AddY(-5f);
                ShowShield();
                _child.SetSpriteAndPolygon(sprite_StuckInTheGround);
                yield return WaitForSeconds(0.5f);
                yield return WaitUntil(() => !IsPressedS);
            }

            yield return waitForFixedUpdate;
        }

    }
}
