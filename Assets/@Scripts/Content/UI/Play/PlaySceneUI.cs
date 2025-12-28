using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlaySceneUI : SceneUI
{
    private ChildKey<Text> Text_Move = new(nameof(Text_Move));
    private ChildKey<Text> Text_Attack = new(nameof(Text_Attack));
    private ChildKey<Text> Text_SpecialSkill = new(nameof(Text_SpecialSkill));
    private ChildKey<Text> Text_HP = new(nameof(Text_HP));
    private ChildKey<Text> Text_Wave = new(nameof(Text_Wave));
    private ChildKey<Text> Text_WaveProgress = new(nameof(Text_WaveProgress));
    
    private ChildKey<Image> DeathThumbnail = new(nameof(DeathThumbnail));
    private ChildKey<Image> WaveClear = new(nameof(WaveClear));
    private ChildKey<Image> GameOver = new(nameof(GameOver));

    private ChildKey<Transform> UI_MobileControl = new(nameof(UI_MobileControl));

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
        BindChild(
        Text_Move,
        Text_Attack,
        Text_SpecialSkill,
        Text_HP,
        Text_Wave,
        Text_WaveProgress,
        DeathThumbnail,
        WaveClear,
        GameOver,
        UI_MobileControl);

        SetControlUI(Manager.Input.isMobileControl);
        Manager.Input.onControlTypeChange.Add(this, () =>
        {
            SetControlUI(Manager.Input.isMobileControl);
        });

        Manager.Game.onPlayerDie.Add(this, () =>
        {
            GetChild(Text_HP).transform.localScale += Vector3.one * 0.25f;
            GetChild(Text_Wave).transform.localScale += Vector3.one * 0.4f;
            StartCoroutine(ShowGameOver());
        });

        Manager.Game.onWaveClear.Add(this, () =>
        {
            StartCoroutine(OnWaveClear());
        });

        StartCoroutine(LoopWaveClearImageEffect());
    }


    private void UpdateHPText()
    {
        Text hpText = GetChild(Text_HP);
        if (Manager.Game.IsNightmare)
        {
            hpText.text = $"꿈에서의 HP:{Manager.Game.DreamHealth}/{Manager.Game.dreamMaxHealth}";
            hpText.color = Utility.StringToColor("#7d6080");
        }
        else if (Manager.Game.currentCharacter == Characters.Suhyen)
        {
            hpText.text = $"수현HP:{Manager.Game.SuhyenHealth}/{Manager.Game.suhyenMaxHealth}";
            hpText.color = Utility.StringToColor("#8f40ff");
            hpText.fontStyle = FontStyle.Bold;
        }
        else
        {
            hpText.text = $"HP:{Manager.Game.Health}/{Manager.Game.currentCharacterData.maxHealth}";
            hpText.color = Utility.StringToColor("#806262");
            hpText.fontStyle = FontStyle.Normal;
        }
    }
    private void UpdateWaveText()
    {
        const float defaultScale = 0.75f;
        const float increaseScale = 0.18f;

        Text waveText = GetChild(Text_Wave);

        if (Manager.Game.IsNightmare)
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
        Text waveProgressText = GetChild(Text_WaveProgress);

        if (Manager.Game.currentCharacter == Characters.Suhyen)
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
            if (Manager.Game.IsNightmare)
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

        waveProgressText.enabled = true;
        if (Manager.Game.wave == 1 && 
            Manager.Game.ShadowState != ShadowState.EndOfGiantization)
                waveProgressText.enabled = false;
    }
    private void UpdateSpecialSkillText()
    {
        Text specialSkillText = GetChild(Text_SpecialSkill);
        if (Manager.Game.isSpecialSkillInvoking)
        {
            if (Manager.Game.currentCharacter == Characters.Sleepground)
            {
                specialSkillText.color = Utility.StringToColor("#918d10");
                specialSkillText.text = "S: 검뽑기";
                return;
            }
            if (Manager.Game.currentCharacter == Characters.Dino)
            {
                specialSkillText.color = Utility.StringToColor("#918d10");
                specialSkillText.text = "공격: 흡혈";
                return;
            }
        }

        if (Manager.Game.SpecialSkillCooltime > 0)
        {
            specialSkillText.color = Utility.StringToColor("#848484");
            specialSkillText.text = $"특수기술 쿨타임:{Manager.Game.SpecialSkillCooltime:F1}";
        }
        else
        {
            specialSkillText.color = Utility.StringToColor("#918d10");
            specialSkillText.text = "S: 특수기술!";
        }
    }


    private void StopCodeOfAnotherObject()
    {
        BaseController[] codes = FindObjectsByType<BaseController>(FindObjectsSortMode.None);
        foreach (BaseController code in codes)
        {
            //if (code == this)
            //    continue;

            code.enabled = false;
            code.StopAllCoroutines();
        }
    }

    private IEnumerator ShowGameOver()
    {
        Image deathThumbnail = GetChild(DeathThumbnail);
        Utility.StartRunForSec(this, 0.5f, sec =>
        {
            float progress = sec / 0.45f;
            deathThumbnail.SetAlpha(progress * 0.5f);
        });

        yield return new WaitForSeconds(0.1f);
        StopCodeOfAnotherObject();

        Image gameOver = GetChild(GameOver);
        gameOver.enabled = true;
        yield return Utility.RunForSec(0.5f, sec =>
        {
            float progress = sec / 0.5f;
            Vector3 pos = gameOver.transform.localPosition;
            pos.x = 300 - 300 * progress;
            gameOver.transform.localPosition = pos;
        });

        //this.enabled = false;
        //this.StopAllCoroutines();

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        Utility.StartScene(Scenes.LobbyScene);
    }


    private IEnumerator OnWaveClear()
    {
        Image waveClearImage = GetChild(WaveClear);

        waveClearImage.SetAlpha(0.5f);

        Vector3 position = waveClearImage.transform.localPosition;
        position.x = 300;
        waveClearImage.transform.localPosition = position;

        for (int i = 12; i > 0; i--)
        {
            waveClearImage.transform.localPosition += Vector3.right * -25f;
            yield return new WaitForFixedUpdate();
        }
        Manager.Game.Health += 10;

        yield return new WaitForSeconds(1f);

        for (int i = 25; i > 0; i--)
        {
            waveClearImage.AddAlpha(-0.02f);
            yield return new WaitForFixedUpdate();
        }
        waveClearImage.SetAlpha(0);
    }

    private IEnumerator LoopWaveClearImageEffect()
    {
        Image waveClearImage = GetChild(WaveClear);

        while (true)
        {
            for (int i = 10; i > 0; i--)
            {
                waveClearImage.AddBrightness(0.05f);

                yield return new WaitForFixedUpdate();
            }

            for (int i = 10; i > 0; i--)
            {
                waveClearImage.AddBrightness(-0.05f);

                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForFixedUpdate();
        }
    }


    private void SetControlUI(bool isMobileControl)
    {
        GetChild(Text_Move).gameObject.SetActive(!isMobileControl);
        GetChild(Text_Attack).gameObject.SetActive(!isMobileControl);
        GetChild(UI_MobileControl).gameObject.SetActive(isMobileControl);
    }
}
