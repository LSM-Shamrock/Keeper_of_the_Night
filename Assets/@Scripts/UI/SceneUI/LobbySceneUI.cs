using UnityEngine;
using UnityEngine.UI;

public class LobbySceneUI : SceneUI
{
    private ChildKey<Text> CharacterNameText = new(nameof(CharacterNameText));
    private ChildKey<Text> CharacterNormalText = new(nameof(CharacterNormalText));
    private ChildKey<Text> CharacterSpecialText = new(nameof(CharacterSpecialText));
    private ChildKey<Text> CharacterHPText = new(nameof(CharacterHPText));


    private void Start()
    {
        Bind(
        CharacterNameText,
        CharacterNormalText,
        CharacterSpecialText,
        CharacterHPText);
    }

    private void Update()
    {
        Get(CharacterNameText).text = Manager.Game.selectedCharacter.ToString();
        Get(CharacterNormalText).text = Manager.Game.characterDescription;
        Get(CharacterSpecialText).text = "특수기술:" + Manager.Game.specialDescription;
        Get(CharacterHPText).text = "HP:" + Manager.Game.characterMaxHealth;
    }
}
