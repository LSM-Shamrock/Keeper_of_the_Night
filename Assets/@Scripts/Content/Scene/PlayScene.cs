using System.Collections;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    [SerializeField]
    private Transform _bg;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Manager.Game.isPlayerDie)
            return;

        if (Manager.Game.SpecialSkillCooltime > 0f)
            Manager.Game.SpecialSkillCooltime -= Time.deltaTime;
    }

    private void Init()
    {
        Manager.Game.Health = Manager.Game.currentCharacterData.maxHealth;
        Manager.Game.SuhyenHealth = Manager.Game.suhyenMaxHealth;
        Manager.Game.DreamHealth = Manager.Game.dreamMaxHealth;

        Manager.Game.onNightMareChange.Add(this, () =>
        {
            if (Manager.Game.IsNightmare == true)
                Manager.Game.SpecialSkillCooltime = 0f;
        });

        StartCoroutine(LoopWave());
    }

    private IEnumerator LoopWave()
    {
        SetWaveClearCondition();
        while (true)
        {
            if (Manager.Game.wave == 1)
            {
                Manager.Game.remainingWaveSecond = 0;
                Manager.Game.remainingWaveKill = 0;
                Manager.Game.ShadowState = ShadowState.None;
                yield return new WaitUntil(() => Manager.Game.ShadowState == ShadowState.Killed);
            }

            if (Manager.Game.wave == 15)
            {
                Manager.Game.SpecialSkillCooltime = 0f;
                Manager.Game.remainingWaveSecond = 0;
                Manager.Game.remainingWaveKill = 0;
                yield return new WaitUntil(() => Manager.Game.isBossDinoKilled);
                yield return new WaitForSeconds(1f);
                Utility.StartScene(Scenes.EndingScene);
            }

            while (Manager.Game.remainingWaveSecond > 0)
            {
                yield return new WaitForSeconds(1f);
                Manager.Game.remainingWaveSecond--;
            }

            yield return new WaitUntil(() => Manager.Game.remainingWaveKill <= 0);

            Manager.Game.wave++;
            Manager.Game.onWaveClear.Call();
            SetWaveClearCondition();
        }
    }

    private void SetWaveClearCondition()
    {
        int wave = Manager.Game.wave;
        if (wave == 0)
        {
            Manager.Game.remainingWaveSecond = 0;
            Manager.Game.remainingWaveKill = 2;
            return;
        }
        if (wave == 7)
        {
            Manager.Game.remainingWaveSecond = 60;
            Manager.Game.remainingWaveKill = 0;
            return;
        }

        if (wave <= 4)
        {
            Manager.Game.remainingWaveSecond = 10;
            Manager.Game.remainingWaveKill = 3;
        }
        else if (wave <= 10)
        {
            Manager.Game.remainingWaveSecond = 20;
            Manager.Game.remainingWaveKill = 6;
        }
        else
        {
            Manager.Game.remainingWaveSecond = 30;
            Manager.Game.remainingWaveKill = 10;
        }
    }
}
