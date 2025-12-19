using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_HP : BaseController
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Start_Loop());
    }

    IEnumerator Start_Loop()
    {
        Text text = GetComponentInChildren<Text>();
        while (true)
        {
            text.text = "HP:" + Manager.Game.characterMaxHealth;
            yield return null;
        }
    }
}
