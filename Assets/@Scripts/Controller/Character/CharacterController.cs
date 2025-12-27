using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterController : BaseController
{
    private Rigidbody2D _rigidbody;

    private int _jumpGauge;

    private bool IsOnGround => _rigidbody.IsContact(PlaySceneObjects.Ground);

    protected override void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        UpdateDinoSpecial();
    }

    private void SetCurrentCharacter(Characters character)
    {
        Manager.Game.currentCharacter = character;
        foreach (Transform child in transform)
        {
            if (child.name == character.ToString())
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        SetCurrentCharacter(Manager.Game.selectedCharacter);

        StartCoroutine(LoopJump());
        StartCoroutine(LoopMove());
        StartCoroutine(LoopIceDown());
        StartCoroutine(LoopShoutEnemyName());

        Manager.Game.onShadowStateChange.Add(this, () =>
        {
            if (Manager.Game.ShadowState == ShadowState.EndOfGiantization) 
                SetCurrentCharacter(Characters.Suhyen);
            if (Manager.Game.ShadowState == ShadowState.Killed)
                SetCurrentCharacter(Manager.Game.selectedCharacter);
        });

        Manager.Game.onNightmareEvent.Add(this, () =>
        {
            if (Manager.Game.wave == 7) Manager.Speech.SpeechForSeconds(transform, "ZzzzZzzz", 3f);
            if (Manager.Game.wave == 8) Manager.Speech.SpeechForSeconds(transform, "!", 2f);
        });
    }
    private IEnumerator LoopJump()
    {
        while (true)
        {
            if (Manager.Game.ice > 0) { yield return new WaitForFixedUpdate(); continue; }
            if (IsOnGround)
            {
                _jumpGauge = 15;
                while (Manager.Input.onJump && _jumpGauge > 0)
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
            yield return new WaitForFixedUpdate();
            if (Manager.Game.ice > 0)
                continue;

            Vector3 direction = Vector3.zero;
            if (Manager.Input.isOnLeft) direction = Vector3.left;
            if (Manager.Input.isOnRight) direction = Vector3.right;
            if (direction == Vector3.zero) 
                continue;

            Manager.Game.characterMoveDirection = direction;
            Vector3 position = transform.position + direction * 3;
            float limit = Define.CharacterXLimit;
            position.x = Mathf.Clamp(position.x, -limit, limit);
            transform.position = position;
        }
    }

    // TODO
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

    // TODO
    private void UpdateDinoSpecial()
    {
        if (Manager.Game.selectedCharacter != Characters.Dino) 
            return;

        Transform dinoTransform = transform.Find(Characters.Dino.ToString());
        SpriteRenderer dinoSpriteRenderer = dinoTransform.GetComponentInChildren<SpriteRenderer>();

        if (!Manager.Game.isSpecialSkillInvoking)
        {
            dinoSpriteRenderer.enabled = true;
            dinoTransform.localScale = Vector3.one;
        }
        else
        {
            dinoSpriteRenderer.enabled = false;
            dinoTransform.localScale = Vector3.one * 1.5f;
        }
    }

    private IEnumerator LoopShoutEnemyName()
    {
        while (true)
        {
            if (Manager.Input.isOnKeyT)
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
