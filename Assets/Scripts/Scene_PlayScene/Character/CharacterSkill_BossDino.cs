using System.Collections;
using UnityEngine;

public class CharacterSkill_BossDino : BaseController
{
    GameObject _child;
    GameObject _black;

    protected override void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        Update_Visual();
    }

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _black = transform.GetChild(1).gameObject;

        Manager.Game.onDisarmSpecialSkill.Add(this, OnSignal_SpecialSkillDisarm);

        StartCoroutine(Loop_Routine());
        StartCoroutine(Loop_BlackBrightnessEftect());
    }

    void OnSignal_SpecialSkillDisarm()
    {
        if (Manager.Game.selectedCharacter == Sprites.Characters.Dino)
        {
            Manager.Game.isSpecialSkillInvoking = false;
        }
    }

    void Update_Visual()
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

    IEnumerator Loop_BlackBrightnessEftect()
    {
        while (true)
        {
            foreach (int i in Count(30))
            {
                _black.Component<SpriteRenderer>().AddBrightness(0.01f);
                yield return waitForFixedUpdate;
            }
            foreach (int i in Count(30))
            {
                _black.Component<SpriteRenderer>().AddBrightness(-0.01f);
                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Routine()
    {
        if (Manager.Game.selectedCharacter == Sprites.Characters.Dino)
            Manager.Game.specialSkillCooltime = 35f;

        while (true)
        {
            yield return WaitUntil(() => Manager.Game.currentCharacter == Sprites.Characters.Dino);

            if (Manager.Game.specialSkillCooltime <= 0 && Manager.Input.IsPressedS)
            {
                Manager.Game.specialSkillCooltime = 45f;
                Manager.Game.isSpecialSkillInvoking = true;
                yield return WaitForSeconds(10f);
                Manager.Game.onDisarmSpecialSkill.Call();
            }

            yield return waitForFixedUpdate;
        }
    }
}
