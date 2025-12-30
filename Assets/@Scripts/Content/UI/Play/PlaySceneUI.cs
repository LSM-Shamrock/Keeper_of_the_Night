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

        StartCoroutine(LoopWaveClearImageEffect());
        Manager.Game.OnWaveClear.Add(this, () => StartCoroutine(OnWaveClear()));

        Manager.Game.OnPlayerDie.Add(this, () =>
        {
            GetChild(Text_HP).transform.localScale += Vector3.one * 0.25f;
            GetChild(Text_Wave).transform.localScale += Vector3.one * 0.4f;
            StartCoroutine(ShowGameOver());
        });


        SetMobileControl(Manager.Input.isMobileControl);
        Manager.Input.onControlTypeChange.Add(this, () => SetMobileControl(Manager.Input.isMobileControl));

    }

    private void UpdateHPText()
    {
        Text hpText = GetChild(Text_HP);
        if (Manager.Game.IsNightmare)
        {
            hpText.text = $"꿈에서의 HP:{Manager.Game.DreamHealth}/{Manager.Game.DreamMaxHealth}";
            hpText.color = Util.StringToColor("#7d6080");
        }
        else if (Manager.Game.CurrentCharacter == Characters.Suhyen)
        {
            hpText.text = $"수현HP:{Manager.Game.SuhyenHealth}/{Manager.Game.SuhyenMaxHealth}";
            hpText.color = Util.StringToColor("#8f40ff");
            hpText.fontStyle = FontStyle.Bold;
        }
        else
        {
            hpText.text = $"HP:{Manager.Game.Health}/{Manager.Game.CurrentCharacterData.maxHealth}";
            hpText.color = Util.StringToColor("#806262");
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
            waveText.color = Util.StringToColor("#704080");
            waveText.transform.localScale = Vector3.one * (defaultScale + increaseScale);
        }
        else
        {
            waveText.text = $"WAVE:{Manager.Game.Wave}";
            waveText.color = Util.StringToColor("#3e5c0a");
            waveText.transform.localScale = Vector3.one * defaultScale;
        }
    }
    private void UpdateWaveProgressText()
    {
        Text waveProgressText = GetChild(Text_WaveProgress);

        if (Manager.Game.CurrentCharacter == Characters.Suhyen)
        {
            waveProgressText.text = "카메라의 빛으로 처치하세요!";
            waveProgressText.color = Util.StringToColor("#ebfad1");
        }
        else if (Manager.Game.Wave == 15)
        {
            waveProgressText.text = "공룡을 처치하세요!";
            waveProgressText.color = Util.StringToColor("#ff0000");
        }
        else
        {
            if (Manager.Game.IsNightmare)
                waveProgressText.color = Util.StringToColor("#704080");
            else
                waveProgressText.color = Util.StringToColor("#3e5c0a");

            waveProgressText.text = "다음 웨이브까지:";
            if (Manager.Game.RemainingWaveSecond > 0)
            {
                waveProgressText.text += $"{Manager.Game.RemainingWaveSecond}초";
                if (Manager.Game.RemainingWaveKill > 0)
                    waveProgressText.text += ",";
            }
            if (Manager.Game.RemainingWaveKill > 0)
            {
                waveProgressText.text += $"{Manager.Game.RemainingWaveKill}킬";
            }
        }

        waveProgressText.enabled = true;
        if (Manager.Game.Wave == 1 && 
            Manager.Game.ShadowState != ShadowState.EndOfGiantization)
                waveProgressText.enabled = false;
    }
    private void UpdateSpecialSkillText()
    {
        Text specialSkillText = GetChild(Text_SpecialSkill);
        if (Manager.Game.IsSpecialSkillInvoking)
        {
            if (Manager.Game.CurrentCharacter == Characters.Sleepground)
            {
                specialSkillText.color = Util.StringToColor("#918d10");
                specialSkillText.text = "S: 검뽑기";
                return;
            }
            if (Manager.Game.CurrentCharacter == Characters.Dino)
            {
                specialSkillText.color = Util.StringToColor("#918d10");
                specialSkillText.text = "공격: 흡혈";
                return;
            }
        }

        if (Manager.Game.SkillCooltime > 0)
        {
            specialSkillText.color = Util.StringToColor("#848484");
            specialSkillText.text = $"특수기술 쿨타임:{Manager.Game.SkillCooltime:F1}";
        }
        else
        {
            specialSkillText.color = Util.StringToColor("#918d10");
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
        CoroutineUtil.StartRunForSec(this, 0.5f, sec =>
        {
            float progress = sec / 0.45f;
            deathThumbnail.SetAlpha(progress * 0.5f);
        });

        yield return new WaitForSeconds(0.1f);
        StopCodeOfAnotherObject();

        Image gameOver = GetChild(GameOver);
        gameOver.enabled = true;
        yield return CoroutineUtil.RunForSec(0.5f, sec =>
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

        Util.StartScene(Scenes.LobbyScene);
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

    private void SetMobileControl(bool isMobileControl)
    {
        GetChild(Text_Move).gameObject.SetActive(!isMobileControl);
        GetChild(Text_Attack).gameObject.SetActive(!isMobileControl);
        GetChild(UI_MobileControl).gameObject.SetActive(isMobileControl);
    }
}
