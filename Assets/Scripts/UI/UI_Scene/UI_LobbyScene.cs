using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    private enum Texts
    {
        CharacterNameText,
        CharacterNormalText,
        CharacterSpecialText,
        CharacterHPText,
    }

    private void Start()
    {
        BindChildren<Text, Texts>();
        

    }

    private void Update()
    {
        GetChild<Text>(Texts.CharacterNameText).text = Manager.Game.selectedCharacter.ToString();
        GetChild<Text>(Texts.CharacterNormalText).text = Manager.Game.characterDescription;
        GetChild<Text>(Texts.CharacterSpecialText).text = "특수기술:" + Manager.Game.specialDescription;
        GetChild<Text>(Texts.CharacterHPText).text = "HP:" + Manager.Game.characterMaxHealth;

    }
}
