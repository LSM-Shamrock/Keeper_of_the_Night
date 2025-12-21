using System.Collections;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    private void Start()
    {
        Manager.Game.remainingHealth = Manager.Game.characterMaxHealth;
        Manager.Game.suhyenHealth = 60;
        Manager.Game.healthInDream = Manager.Game.characterMaxHealth / 2;

        Manager.Game.onNightMareChange.Add(this, () =>
        {
            if (Manager.Game.isNightmare == true)
                Manager.Game.specialSkillCooltime = 0f;
        });

        StartCoroutine(UpdateWave());
    }

    private void Update()
    {
        if (Manager.Game.specialSkillCooltime > 0f)
            Manager.Game.specialSkillCooltime -= Time.deltaTime;
        else
            Manager.Game.specialSkillCooltime = 0f;
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

    IEnumerator UpdateWave()
    {
        SetWaveClearCondition();
        while (true)
        {
            if (Manager.Game.wave == 1)
            {
                Manager.Game.remainingWaveSecond = 0;
                Manager.Game.remainingWaveKill = 0;
                Manager.Game.shadowState = ShadowState.None;
                yield return new WaitUntil(() => Manager.Game.shadowState == ShadowState.Killed);
            }

            if (Manager.Game.wave == 15)
            {
                Manager.Game.specialSkillCooltime = 0f;
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
}
