using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_SpecialSkill : BaseController
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
            text.text = "특수기술:" + Manager.Game.specialDescription;
            yield return null;
        }
    }
}
