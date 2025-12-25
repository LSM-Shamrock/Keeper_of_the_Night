using System.Collections;
using UnityEngine;

public class CharacterSkill_BossDino : BaseController
{
    private GameObject _child;
    private GameObject _black;

    protected override void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        UpdateVisual();
    }

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _black = transform.GetChild(1).gameObject;

        Manager.Game.onDisarmSpecialSkill.Add(this, OnSignalSpecialSkillDisarm);

        StartCoroutine(LoopRoutine());
        StartCoroutine(LoopBlackBrightnessEftect());
    }

    private void OnSignalSpecialSkillDisarm()
    {
        if (Manager.Game.selectedCharacter == Sprites.Characters.Dino)
            Manager.Game.isSpecialSkillInvoking = false;
    }

    private void UpdateVisual()
    {
        bool b = Manager.Game.selectedCharacter == Sprites.Characters.Dino && Manager.Game.isSpecialSkillInvoking;
        _child.SetActive(b);
        _black.SetActive(b);

        transform.position = Manager.Game.Character.position;

        bool flip = Manager.Game.characterMoveDirection.x < 0;
        transform.rotation = Quaternion.Euler(Vector3.up * (flip ? 180 : 0));

        _black.transform.position = transform.position;
        _black.transform.position += Vector3.up * -20f;
        _black.transform.position += Manager.Game.characterMoveDirection * 15f;
    }

    private IEnumerator LoopBlackBrightnessEftect()
    {
        while (true)
        {
            foreach (int i in Count(30))
            {
                _black.Component<SpriteRenderer>().AddBrightness(0.01f);
                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(30))
            {
                _black.Component<SpriteRenderer>().AddBrightness(-0.01f);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator LoopRoutine()
    {
        if (Manager.Game.selectedCharacter == Sprites.Characters.Dino)
            Manager.Game.specialSkillCooltime = 35f;

        while (true)
        {
            yield return new WaitUntil(() => Manager.Game.currentCharacter == Sprites.Characters.Dino);

            if (Manager.Game.specialSkillCooltime <= 0 && Manager.Input.isPressedS)
            {
                Manager.Game.specialSkillCooltime = 45f;
                Manager.Game.isSpecialSkillInvoking = true;

                yield return new WaitForSeconds(10f);

                Manager.Game.onDisarmSpecialSkill.Call();
            }
        }
    }
}
