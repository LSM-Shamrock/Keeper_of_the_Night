using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : PlaySceneObjectBase
{
    Collider2D _col;
    SpriteRenderer _sr;
    Dictionary<Sprites.Characters, Sprite> _sprites = new();
    int _jumpGauge;

    bool IsOnGround => _col.IsContact(PlaySceneObjects.Ground);

    protected override void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        Update_DinoSpecial();
    }

    void Init()
    {
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        foreach (Sprites.Characters character in Enum.GetValues(typeof(Sprites.Characters)))
            _sprites[character] = Utile.LoadResource<Sprite>(character);

        currentCharacter = selectedCharacter;
        _sr.sprite = _sprites[currentCharacter];
        onNightmareEvent.Add(this, OnNightmareEvent);
        StartCoroutine(Loop_Jump());
        StartCoroutine(Loop_Move());
        StartCoroutine(Loop_IceDown());
        StartCoroutine(Loop_SuhyenEvent());
        StartCoroutine(Loop_ShoutEnemyName());
    }
    IEnumerator Loop_Jump()
    {
        while (true)
        {
            if (ice > 0) { yield return waitForFixedUpdate; continue; }
            if (IsOnGround)
            {
                _jumpGauge = 15;
                while (IsPressedW && _jumpGauge > 0)
                {   
                    transform.position += Vector3.up * (_jumpGauge * _jumpGauge / 10);
                    _jumpGauge--;
                    yield return WaitForSeconds(0.01f);
                    yield return waitForFixedUpdate;
                }
            }
            else transform.position += Vector3.up * -5f;
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_Move()
    {
        while(true)
        {   
            if (ice <= 0)
            {
                if (IsPressedA && transform.position.x > -240)
                {
                    characterMoveDirection = Vector3.left;
                    transform.position += characterMoveDirection * 3;
                }
                if (IsPressedD && transform.position.x < 240)
                {
                    characterMoveDirection = Vector3.right;
                    transform.position += characterMoveDirection * 3;
                }
            }

            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_SuhyenEvent()
    {
        while (true)
        {
            if (shadowState != ShadowState.Giantization)
            {
                yield return waitForFixedUpdate;
                continue;
            }
            currentCharacter = Sprites.Characters.Suhyen;
            _sr.sprite = _sprites[Sprites.Characters.Suhyen];
            yield return WaitUntil(() => shadowState == ShadowState.Killed);
            currentCharacter = selectedCharacter;
            _sr.sprite = _sprites[currentCharacter];
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_IceDown()
    {
        while (true)
        {
            if (ice > 0)
            {
                yield return WaitForSeconds(0.1f);
                ice--;
            }
            else yield return waitForFixedUpdate;
        }
    }
    void Update_DinoSpecial()
    {
        if (selectedCharacter != Sprites.Characters.Dino) return;
        if (!isSpecialSkillInvoking)
        {
            _sr.color = Color.white;
            transform.localScale = Vector3.one * 50;
        }
        else
        {
            _sr.color = Color.clear;
            transform.localScale = Vector3.one * 75;
        }
    }
    void OnNightmareEvent()
    {
        if (wave == 7) SpeechForSeconds("ZzzzZzzz", 3f);
        if (wave == 8) SpeechForSeconds("!", 2f);
    }


    IEnumerator Loop_ShoutEnemyName()
    {
        while (true)
        {
            if (IsPressedT)
            {
                yield return SpeechAndWaitInput("야괴 이름 외치기:", inpuut => shoutedEnemyName = inpuut);
                foreach (int i in Count(3))
                {
                    yield return SpeechForSeconds(shoutedEnemyName + "!", 0.5f);
                    yield return WaitForSeconds(0.25f);
                    yield return waitForFixedUpdate;
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
