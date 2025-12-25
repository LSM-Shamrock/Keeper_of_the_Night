using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : BaseController
{
    private Collider2D _col;
    private SpriteRenderer _sr;
    private Dictionary<Sprites.Characters, Sprite> _sprites = new();
    private int _jumpGauge;

    private bool IsOnGround => _col.IsContact(PlaySceneObjects.Ground);

    protected override void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        UpdateDinoSpecial();
    }

    private void Init()
    {
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
        foreach (Sprites.Characters character in Enum.GetValues(typeof(Sprites.Characters)))
            _sprites[character] = Utility.LoadResource<Sprite>(character);

        Manager.Game.currentCharacter = Manager.Game.selectedCharacter;
        _sr.sprite = _sprites[Manager.Game.currentCharacter];
        Manager.Game.onNightmareEvent.Add(this, OnNightmareEvent);
        StartCoroutine(LoopJump());
        StartCoroutine(LoopMove());
        StartCoroutine(LoopIceDown());
        StartCoroutine(SuhyenEvent());
        StartCoroutine(LoopShoutEnemyName());
    }
    private IEnumerator LoopJump()
    {
        while (true)
        {
            if (Manager.Game.ice > 0) { yield return new WaitForFixedUpdate(); continue; }
            if (IsOnGround)
            {
                _jumpGauge = 15;
                while (Manager.Input.isPressedJump && _jumpGauge > 0)
                {   
                    transform.position += Vector3.up * (_jumpGauge * _jumpGauge / 10);
                    _jumpGauge--;
                    yield return new WaitForSeconds(0.01f);
                    yield return new WaitForFixedUpdate();
                }
            }
            else transform.position += Vector3.up * -5f;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator LoopMove()
    {
        while(true)
        {   
            if (Manager.Game.ice <= 0)
            {
                if (Manager.Input.isPressedLeft && transform.position.x > -240)
                {
                    Manager.Game.characterMoveDirection = Vector3.left;
                    transform.position += Manager.Game.characterMoveDirection * 3;
                }
                if (Manager.Input.isPressedRight && transform.position.x < 240)
                {
                    Manager.Game.characterMoveDirection = Vector3.right;
                    transform.position += Manager.Game.characterMoveDirection * 3;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator SuhyenEvent()
    {
        yield return new WaitUntil(() => Manager.Game.shadowState == ShadowState.EndOfGiantization);

        Manager.Game.currentCharacter = Sprites.Characters.Suhyen;
        _sr.sprite = _sprites[Sprites.Characters.Suhyen];

        yield return new WaitUntil(() => Manager.Game.shadowState == ShadowState.Killed);

        Manager.Game.currentCharacter = Manager.Game.selectedCharacter;
        _sr.sprite = _sprites[Manager.Game.currentCharacter];
    }
    private IEnumerator LoopIceDown()
    {
        while (true)
        {
            if (Manager.Game.ice > 0)
            {
                yield return new WaitForSeconds(0.1f);
                Manager.Game.ice--;
            }
            else yield return null;
        }
    }
    private void UpdateDinoSpecial()
    {
        if (Manager.Game.selectedCharacter != Sprites.Characters.Dino) return;
        if (!Manager.Game.isSpecialSkillInvoking)
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
    private void OnNightmareEvent()
    {
        if (Manager.Game.wave == 7) Manager.Speech.SpeechForSeconds(transform, "ZzzzZzzz", 3f);
        if (Manager.Game.wave == 8) Manager.Speech.SpeechForSeconds(transform, "!", 2f);
    }


    private IEnumerator LoopShoutEnemyName()
    {
        while (true)
        {
            if (Manager.Input.isPressedT)
            {
                yield return Manager.Speech.SpeechAndWaitInput(transform, "야괴 이름 외치기:", inpuut => Manager.Game.shoutedEnemyName = inpuut);
                foreach (int i in Count(3))
                {
                    yield return Manager.Speech.SpeechForSeconds(transform, Manager.Game.shoutedEnemyName + "!", 0.5f);
                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield return null;
        }
    }
}
