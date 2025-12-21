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
        Bind<Text, Texts>();
        

    }

    private void Update()
    {
        Get<Text>(Texts.CharacterNameText).text = Manager.Game.selectedCharacter.ToString();
        Get<Text>(Texts.CharacterNormalText).text = Manager.Game.characterDescription;
        Get<Text>(Texts.CharacterSpecialText).text = "특수기술:" + Manager.Game.specialDescription;
        Get<Text>(Texts.CharacterHPText).text = "HP:" + Manager.Game.characterMaxHealth;

    }
}
