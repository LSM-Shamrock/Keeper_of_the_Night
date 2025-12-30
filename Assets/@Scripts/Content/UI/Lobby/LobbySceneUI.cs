using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class LobbySceneUI : SceneUI
{
    private ChildKey<GameObject> ControlTexts = new(nameof(ControlTexts));

    private ChildKey<Text> CharacterNameText = new(nameof(CharacterNameText));
    private ChildKey<Text> CharacterNormalText = new(nameof(CharacterNormalText));
    private ChildKey<Text> CharacterSpecialText = new(nameof(CharacterSpecialText));
    private ChildKey<Text> CharacterHPText = new(nameof(CharacterHPText));

    private ChildKey<Transform> Characters = new(nameof(Characters));
    private Dictionary<Characters, Transform> _characterDict = new();

    private void Start()
    {
        BindChild(
        ControlTexts,
        CharacterNameText,
        CharacterNormalText,
        CharacterSpecialText,
        CharacterHPText,
        Characters);

        GetChild(ControlTexts).SetActive(!Manager.Input.isMobileControl);
        Manager.Input.onControlTypeChange.Add(this, () =>
        {
            GetChild(ControlTexts).SetActive(!Manager.Input.isMobileControl);
        });

        SetSelectedCharacter(Manager.Game.SelectedCharacter);
        foreach (Transform child in GetChild(Characters))
        {
            Characters character = (Characters)Enum.Parse(typeof(Characters), child.name);
            _characterDict[character] = child;

            BindEvent(child, EventType.PointerClick, () =>
            {
                SetSelectedCharacter(character);
            });
        }
        
    }

    private void Update()
    {
        CharacterData characterData = Manager.Data.characterDatas[Manager.Game.SelectedCharacter];
        GetChild(CharacterNameText).text = characterData.name;
        GetChild(CharacterNormalText).text = characterData.description;
        GetChild(CharacterSpecialText).text = "특수기술:" + characterData.specialDescription;
        GetChild(CharacterHPText).text = "HP:" + characterData.maxHealth;
    }


    private void SetSelectedCharacter(Characters character)
    {
        Manager.Game.SelectedCharacter = character;

        CharacterData data = Manager.Data.characterDatas[character];

        foreach (Characters key in _characterDict.Keys)
        {
            Transform transform = _characterDict[key];
            Image[] images = transform.GetComponentsInChildren<Image>();

            float size = character == key ? 1.25f : 1f;
            Color color = character == key ? Color.white : Color.gray;

            foreach (Image image in images) image.color = color;
            transform.localScale = Vector3.one * size;
        }
    }
}
