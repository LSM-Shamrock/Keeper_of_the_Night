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

        SetSelectedCharacter(Manager.Game.selectedCharacter);
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
        GetChild(CharacterNameText).text = Manager.Game.selectedCharacter.ToString();
        GetChild(CharacterNormalText).text = Manager.Game.characterDescription;
        GetChild(CharacterSpecialText).text = "특수기술:" + Manager.Game.specialDescription;
        GetChild(CharacterHPText).text = "HP:" + Manager.Game.maxHealth;
    }


    private void SetSelectedCharacter(Characters character)
    {
        Manager.Game.selectedCharacter = character;
        switch (Manager.Game.selectedCharacter)
        {
            case global::Characters.Sleepground:
                Manager.Game.characterDescription = "월광검으로 근거리 공격";
                Manager.Game.specialDescription = "월광검 방어막";
                Manager.Game.maxHealth = 200; break;
            case global::Characters.Rather:
                Manager.Game.characterDescription = "물로 곡선형 공격";
                Manager.Game.specialDescription = "물감옥 생성";
                Manager.Game.maxHealth = 200; break;
            case global::Characters.Dino:
                Manager.Game.characterDescription = "월광건으로 장거리공격";
                Manager.Game.specialDescription = "야괴로 변신해 흡혈";
                Manager.Game.maxHealth = 100; break;
        }

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
