using UnityEngine;
using UnityEngine.UI;

public class UI_PlayScene : UI_Scene
{
    private enum Texts
    {
        Text_SpecialSkill,

        Text_HP,
        Text_Wave,
        Text_WaveProgress,
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        UpdateHPText();
        UpdateWaveText();
        UpdateWaveProgressText();
        UpdateSpecialSkillText();
    }

    private void Init()
    {
        Bind<Text, Texts>();


        Manager.Game.onPlayerDie.Add(this, () =>
        {
            Get<Text>(Texts.Text_HP).transform.localScale += Vector3.one * 0.25f;
            Get<Text>(Texts.Text_Wave).transform.localScale += Vector3.one * 0.4f;
        });
    }







    private void UpdateHPText()
    {
        Text hpText = Get<Text>(Texts.Text_HP);
        if (Manager.Game.isNightmare)
        {
            hpText.text = $"꿈에서의 HP:{Manager.Game.healthInDream}/{Manager.Game.characterMaxHealth / 2}";
            hpText.color = Utility.StringToColor("#7d6080");
        }
        else if (Manager.Game.currentCharacter == Sprites.Characters.Suhyen)
        {
            hpText.text = $"수현HP:{Manager.Game.suhyenHealth}/{60}";
            hpText.color = Utility.StringToColor("#8f40ff");
            hpText.fontStyle = FontStyle.Bold;
        }
        else
        {
            hpText.text = $"HP:{Manager.Game.remainingHealth}/{Manager.Game.characterMaxHealth}";
            hpText.color = Utility.StringToColor("#806262");
            hpText.fontStyle = FontStyle.Normal;
        }
    }

    private void UpdateWaveText()
    {
        const float defaultScale = 0.75f;
        const float increaseScale = 0.18f;

        Text waveText = Get<Text>(Texts.Text_Wave);

        if (Manager.Game.isNightmare)
        {
            waveText.text = $"WAVE:7 - 악몽";
            waveText.color = Utility.StringToColor("#704080");
            waveText.transform.localScale = Vector3.one * (defaultScale + increaseScale);
        }
        else
        {
            waveText.text = $"WAVE:{Manager.Game.wave}";
            waveText.color = Utility.StringToColor("#3e5c0a");
            waveText.transform.localScale = Vector3.one * defaultScale;
        }
    }

    private void UpdateWaveProgressText()
    {
        Text waveProgressText = Get<Text>(Texts.Text_WaveProgress);

        if (Manager.Game.currentCharacter == Sprites.Characters.Suhyen)
        {
            waveProgressText.text = "카메라의 빛으로 처치하세요!";
            waveProgressText.color = Utility.StringToColor("#ebfad1");
        }
        else if (Manager.Game.wave == 15)
        {
            waveProgressText.text = "공룡을 처치하세요!";
            waveProgressText.color = Utility.StringToColor("#ff0000");
        }
        else
        {
            if (Manager.Game.isNightmare)
                waveProgressText.color = Utility.StringToColor("#704080");
            else
                waveProgressText.color = Utility.StringToColor("#3e5c0a");

            waveProgressText.text = "다음 웨이브까지:";
            if (Manager.Game.remainingWaveSecond > 0)
            {
                waveProgressText.text += $"{Manager.Game.remainingWaveSecond}초";
                if (Manager.Game.remainingWaveKill > 0)
                    waveProgressText.text += ",";
            }
            if (Manager.Game.remainingWaveKill > 0)
            {
                waveProgressText.text += $"{Manager.Game.remainingWaveKill}킬";
            }
        }


        if (Manager.Game.wave == 1)
        {
            if (Manager.Game.shadowState == ShadowState.EndOfGiantization)
                waveProgressText.enabled = true;
            else
                waveProgressText.enabled = false;
        }
        else
            waveProgressText.enabled = true;
    }

    private void UpdateSpecialSkillText()
    {
        Text specialSkillText = Get<Text>(Texts.Text_SpecialSkill);
        if (Manager.Game.isSpecialSkillInvoking)
        {
            if (Manager.Game.currentCharacter == Sprites.Characters.Sleepground)
            {
                specialSkillText.color = Utility.StringToColor("#918d10");
                specialSkillText.text = "S로 검뽑기";
                return;
            }
            if (Manager.Game.currentCharacter == Sprites.Characters.Dino)
            {
                specialSkillText.color = Utility.StringToColor("#918d10");
                specialSkillText.text = "마우스로 흡혈";
                return;
            }
        }

        if (Manager.Game.specialSkillCooltime > 0)
        {
            specialSkillText.color = Utility.StringToColor("#848484");
            specialSkillText.text = $"특수기술 쿨타임:{Manager.Game.specialSkillCooltime:F1}";
        }
        else
        {
            specialSkillText.color = Utility.StringToColor("#918d10");
            specialSkillText.text = "S로 특수기술!";
        }
    }

}
