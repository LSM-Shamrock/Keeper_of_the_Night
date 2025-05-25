using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_CharacterDescription : ObjectBase
{
    protected override void Start()
    {
        base.Start();
        Init();
    }

    Text _text;

    void Init()
    {
        _text = GetComponentInChildren<Text>();
        StartCoroutine(Start_Loop());
    }

    IEnumerator Start_Loop()
    {
        while (true)
        {
            _text.text = characterDescription;
            yield return null;
        }
    }
}
