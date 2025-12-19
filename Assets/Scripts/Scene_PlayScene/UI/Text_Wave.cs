using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Text_Wave : PlaySceneObjectBase
{
    protected override void Start()
    {
        
        remainingHealth = Manager.Game.characterMaxHealth;
        healthInDream = Manager.Game.characterMaxHealth / 2;
        StartCoroutine(Start_IncreaseSizeWhenPlayerDie());
        StartCoroutine(Start_UpdateText());
    }
    IEnumerator Start_IncreaseSizeWhenPlayerDie()
    {
        yield return WaitUntil(() => remainingHealth > 0 && suhyenHealth > 0);
        yield return WaitUntil(() => remainingHealth <= 0 || suhyenHealth <= 0);
        transform.localScale += Vector3.one * 0.4f;
    }
    IEnumerator Start_UpdateText()
    {
        Text text = GetComponentInChildren<Text>();
        while (true)
        {
            if (isNightmare)
            {
                text.text = $"WAVE:7 - 악몽";
                text.color = Utility.StringToColor("#704080");
                transform.localScale += Vector3.one * 0.18f;
                yield return WaitUntil(() => !isNightmare);
                transform.localScale += Vector3.one * -0.18f;
            }
            text.text = $"WAVE:{Manager.Game.wave}";
            text.color = Utility.StringToColor("#3e5c0a");
            yield return null;
        }
    }
}
