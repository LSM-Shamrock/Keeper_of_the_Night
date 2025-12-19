using System.Collections;
using UnityEngine.UI;

public class Lobby_SelectedCharacter : BaseController
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
            text.text = Manager.Game.selectedCharacter.ToString();
            yield return null;
        }
    }
}
