using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_SpecialSkill : ObjectBase
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
            text.text = "특수기술:" + specialDescription;
            yield return null;
        }
    }
}
