using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Text_HP : PlaySceneObjectBase
{
    Text _text;
    protected override void Start()
    {
        _text = GetComponentInChildren<Text>();

        remainingHealth = characterMaxHealth;
        suhyenHealth = 60;
        healthInDream = characterMaxHealth / 2;
        StartCoroutine(StartCo_IncreaseSizeWhenPlayerDie());
    }
    private void FixedUpdate()
    {
        Update_HealthLimit();
        Update_SetText();
    }

    IEnumerator StartCo_IncreaseSizeWhenPlayerDie()
    {
        yield return WaitUntil(() => remainingHealth > 0 && suhyenHealth > 0);
        yield return WaitUntil(() => remainingHealth <= 0 || suhyenHealth <= 0);
        transform.localScale += Vector3.one * 0.25f;
    }
    void Update_HealthLimit()
    {
        if (remainingHealth > characterMaxHealth)
            remainingHealth = characterMaxHealth;
        else if (healthInDream > characterMaxHealth / 2)
            healthInDream = characterMaxHealth / 2;
    }
    void Update_SetText()
    {
        if (isNightmare)
        {
            _text.text = $"꿈에서의 HP:{healthInDream}/{characterMaxHealth / 2}";
            _text.color = Utile.StringToColor("#7d6080");
        }
        else if (currentCharacter == Sprites.Characters.Suhyen)
        {
            _text.text = $"수현HP:{suhyenHealth}/{60}";
            _text.color = Utile.StringToColor("#8f40ff");
            _text.fontStyle = FontStyle.Bold;
        }
        else
        {
            _text.text = $"HP:{remainingHealth}/{characterMaxHealth}";
            _text.color = Utile.StringToColor("#806262");
            _text.fontStyle = FontStyle.Normal;
        }
    }
}
