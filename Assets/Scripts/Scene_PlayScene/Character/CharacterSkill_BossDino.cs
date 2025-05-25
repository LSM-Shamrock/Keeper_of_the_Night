using System.Collections;
using UnityEngine;

public class CharacterSkill_BossDino : PlaySceneObjectBase
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

        onDisarmSpecialSkill.Add(this, OnSignal_SpecialSkillDisarm);

        StartCoroutine(Loop_Routine());
        StartCoroutine(Loop_BlackBrightnessEftect());
    }

    void OnSignal_SpecialSkillDisarm()
    {
        if (selectedCharacter == Sprites.Characters.Dino)
        {
            isSpecialSkillInvoking = false;
        }
    }

    void Update_Visual()
    {
        bool b = selectedCharacter == Sprites.Characters.Dino && isSpecialSkillInvoking;
        _child.SetActive(b);
        _black.SetActive(b);

        transform.position = Character.position;

        bool flip = characterMoveDirection.x < 0;
        transform.rotation = Quaternion.Euler(Vector3.up * (flip ? 180 : 0));

        _black.transform.position = transform.position;
        _black.transform.position += Vector3.up * -20f;
        _black.transform.position += characterMoveDirection * 15f;
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
        if (selectedCharacter == Sprites.Characters.Dino)
            specialSkillCooltime = 35f;

        while (true)
        {
            yield return WaitUntil(() => currentCharacter == Sprites.Characters.Dino);

            if (specialSkillCooltime <= 0 && IsPressedS)
            {
                specialSkillCooltime = 45f;
                isSpecialSkillInvoking = true;
                yield return WaitForSeconds(10f);
                onDisarmSpecialSkill.Call();
            }

            yield return waitForFixedUpdate;
        }
    }
}
