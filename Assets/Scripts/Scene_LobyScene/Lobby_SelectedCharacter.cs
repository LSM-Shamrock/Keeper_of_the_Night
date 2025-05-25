using System.Collections;
using UnityEngine.UI;

public class Lobby_SelectedCharacter : ObjectBase
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
            text.text = selectedCharacter.ToString();
            yield return null;
        }
    }
}
