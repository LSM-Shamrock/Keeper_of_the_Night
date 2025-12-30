using System.Collections;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    private SpriteRenderer _ship;

    private void Start()
    {
        _ship = GameObject.Find("Ship").GetComponent<SpriteRenderer>();

        Manager.Game.Init();
        StartCoroutine(LoopWave());
    }

    private void Update()
    {
        if (Manager.Game.IsPlayerDie)
            return;

        if (Manager.Game.SkillCooltime > 0f)
            Manager.Game.SkillCooltime -= Time.deltaTime;

        if (Manager.Game.Wave == 15)
        {
            if (_ship.color.a < 1f)
                _ship.AddAlpha(Time.deltaTime / 0.5f);
            else
                _ship.SetAlpha(1f);
        }
        else
            _ship.SetAlpha(0f);
    }

    private void SetWaveClearCondition()
    {
        int wave = Manager.Game.Wave;
        if (wave == 0)
        {
            Manager.Game.RemainingWaveSecond = 0;
            Manager.Game.RemainingWaveKill = 2;
            return;
        }
        if (wave == 7)
        {
            Manager.Game.RemainingWaveSecond = 60;
            Manager.Game.RemainingWaveKill = 0;
            return;
        }

        if (wave <= 4)
        {
            Manager.Game.RemainingWaveSecond = 10;
            Manager.Game.RemainingWaveKill = 3;
        }
        else if (wave <= 10)
        {
            Manager.Game.RemainingWaveSecond = 20;
            Manager.Game.RemainingWaveKill = 6;
        }
        else
        {
            Manager.Game.RemainingWaveSecond = 30;
            Manager.Game.RemainingWaveKill = 10;
        }
    }

    private IEnumerator LoopWave()
    {
        SetWaveClearCondition();
        while (true)
        {
            if (Manager.Game.Wave == 1)
            {
                Manager.Game.RemainingWaveSecond = 0;
                Manager.Game.RemainingWaveKill = 0;
                Manager.Game.ShadowState = ShadowState.None;
                yield return new WaitUntil(() => Manager.Game.ShadowState == ShadowState.Killed);
            }

            if (Manager.Game.Wave == 15)
            {
                Manager.Game.SkillCooltime = 0f;
                Manager.Game.RemainingWaveSecond = 0;
                Manager.Game.RemainingWaveKill = 0;
                yield return new WaitUntil(() => Manager.Game.IsBossDinoKilled);
                yield return new WaitForSeconds(1f);
                Util.StartScene(Scenes.EndingScene);
            }

            while (Manager.Game.RemainingWaveSecond > 0)
            {
                yield return new WaitForSeconds(1f);
                Manager.Game.RemainingWaveSecond--;
            }

            yield return new WaitUntil(() => Manager.Game.RemainingWaveKill <= 0);

            Manager.Game.Wave++;
            Manager.Game.OnWaveClear.Call();
            SetWaveClearCondition();
        }
    }
}
