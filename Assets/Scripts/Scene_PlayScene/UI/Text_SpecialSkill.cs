using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Text_SpecialSkill : PlaySceneObjectBase
{
    Text _text;

    private void Awake()
    {
        Init();
    }
    private void FixedUpdate()
    {
        Update_Text();
    }

    void Init()
    {
        _text = GetComponentInChildren<Text>();
        StartCoroutine(Loop_UpdateCooltime());
        StartCoroutine(Start_WaitForNightmareAndClearCooltime());
        StartCoroutine(Start_WaitFor15WaveAndClearCooltime());
    }

    void Update_Text()
    {
        if (isSpecialSkillInvoking)
        {
            if (currentCharacter == Sprites.Characters.Sleepground)
            {
                _text.color = Utility.StringToColor("#918d10");
                _text.text = "S로 검뽑기";
                return;
            }
            if (currentCharacter == Sprites.Characters.Dino)
            {
                _text.color = Utility.StringToColor("#918d10");
                _text.text = "마우스로 흡혈";
                return;
            }
        }

        if (specialSkillCooltime > 0)
        {
            _text.color = Utility.StringToColor("#848484");
            _text.text = $"특수기술 쿨타임:{specialSkillCooltime:F1}";
        }
        else
        {
            _text.color = Utility.StringToColor("#918d10");
            _text.text = "S로 특수기술!";
        }
    }

    IEnumerator Loop_UpdateCooltime()
    {
        while (true)
        {
            if (specialSkillCooltime > 0f)
            {
                yield return WaitForSeconds(0.1f);
                specialSkillCooltime -= 0.1f;
            }
            else
            {
                specialSkillCooltime = 0f;
            }
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Start_WaitForNightmareAndClearCooltime()
    {
        yield return WaitUntil(() => isNightmare);
        specialSkillCooltime = 0f;
    }
    IEnumerator Start_WaitFor15WaveAndClearCooltime()
    {
        yield return WaitUntil(() => Manager.Game.wave == 15);
        specialSkillCooltime = 0f;
    }
}
