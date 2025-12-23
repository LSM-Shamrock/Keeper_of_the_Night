using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;

public class Lobby_Character : UI_Base, IPointerClickHandler
{
    private Sprites.Characters _character;

    private void Start()
    {
        _character = (Sprites.Characters)Enum.Parse(typeof(Sprites.Characters), gameObject.name);
    }

    private void Update()
    {
        Image[] images = GetComponentsInChildren<Image>();
        float size;
        Color color;

        if (Manager.Game.selectedCharacter.ToString() == gameObject.name)
        {
            color = Color.white;
            size = 1.25f;
        }
        else
        {
            color = Color.gray;
            size = 1f;
        }
        foreach (var image in images) image.color = color;
        transform.localScale = Vector3.one * size;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Manager.Game.selectedCharacter = _character;
        switch (Manager.Game.selectedCharacter)
        {
            case Sprites.Characters.Sleepground:
                Manager.Game.characterDescription = "월광검으로 근거리 공격";
                Manager.Game.specialDescription = "월광검 방어막";
                Manager.Game.characterMaxHealth = 200;
                break;
            case Sprites.Characters.Rather:
                Manager.Game.characterDescription = "물로 곡선형 공격";
                Manager.Game.specialDescription = "물감옥 생성";
                Manager.Game.characterMaxHealth = 200;
                break;
            case Sprites.Characters.Dino:
                Manager.Game.characterDescription = "월광건으로 장거리공격";
                Manager.Game.specialDescription = "야괴로 변신해 흡혈";
                Manager.Game.characterMaxHealth = 100;
                break;
        }
    }
}
