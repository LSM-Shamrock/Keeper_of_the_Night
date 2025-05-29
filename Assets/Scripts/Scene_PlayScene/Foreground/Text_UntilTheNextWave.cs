using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Utile;

public class Text_UntilTheNextWave : PlaySceneObjectBase
{
    Text _text;
    protected override void Start()
    {
        _text = GetComponentInChildren<Text>();
        StartCoroutine(Start_SetRemainingWaveSecond());
        StartCoroutine(Start_ControlWave());
    }
    private void FixedUpdate()
    {
        Update_SetText();
        Update_HideAndShow();
    }

    void Update_SetText()
    {
        if (currentCharacter == Sprites.Characters.Suhyen)
        {
            _text.text = "카메라의 빛으로 처치하세요!";
            _text.color = StringToColor("#ebfad1");
        }
        else if (wave == 15)
        {
            _text.text = "공룡을 처치하세요!";
            _text.color = StringToColor("#ff0000");
        }
        else
        {
            if (isNightmare)
                _text.color = StringToColor("#704080");
            else
                _text.color = StringToColor("#3e5c0a");

            _text.text = "다음 웨이브까지:";
            if (remainingWaveSecond > 0)
            {
                _text.text += $"{remainingWaveSecond}초";
                if (remainingWaveKill > 0)
                    _text.text += ",";
            }
            if (remainingWaveKill > 0)
            {
                _text.text += $"{remainingWaveKill}킬";
            }
        }
    }

    IEnumerator Start_SetRemainingWaveSecond()
    {
        while (true)
        {
            if (remainingWaveSecond > 0) 
            {
                yield return WaitForSeconds(1f);
                remainingWaveSecond--;
            }
            else yield return null;
        }
    }

    IEnumerator SetWaveClearCondition()
    {
        if (wave == 0)
        {
            remainingWaveSecond = 0;
            remainingWaveKill = 2;
        }
        else if (wave == 1)
        {
            remainingWaveSecond = 0;
            remainingWaveKill = 0;
            shadowState = ShadowState.None;
            yield return WaitUntil(() => shadowState == ShadowState.Killed);
        }
        else if (wave == 7)
        {
            remainingWaveSecond = 60;
            remainingWaveKill = 0;
        }
        else
        {
            if (wave > 4)
            {
                if (wave > 10)
                {
                    remainingWaveSecond = 30;
                    remainingWaveKill = 10;
                }
                else
                {
                    remainingWaveSecond = 20;
                    remainingWaveKill = 6;
                }
            }
            else
            {
                remainingWaveSecond = 10;
                remainingWaveKill = 3;
            }
        }
    }
    IEnumerator Start_ControlWave()
    {
        yield return SetWaveClearCondition();
        while (true)
        {
            if (wave == 15)
            {
                yield return WaitUntil(() => isBossDinoKilled);
                yield return WaitForSeconds(1f);
                StartScene(Scenes.EndingScene);
                continue;
            }
            else if (remainingWaveSecond <= 0 && remainingWaveKill <= 0)
            {
                wave++;
                onWaveClear.Call();
                yield return SetWaveClearCondition();
            }
            yield return waitForFixedUpdate;
        }
    }

    void Update_HideAndShow()
    {
        if (wave == 1)
        {
            if (shadowState == ShadowState.EndOfGiantization)
                _text.enabled = true;
            else
                _text.enabled = false;
        }
        else
            _text.enabled = true;
    }
}
