using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;

public class Lobby_Character : ObjectBase, IPointerClickHandler
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Start_Loop());
    }

    IEnumerator Start_Loop()
    {
        Image[] images = GetComponentsInChildren<Image>();
        float size;
        Color color;
        while (true)
        {
            if (selectedCharacter.ToString() == gameObject.name)
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
            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedCharacter = (Sprites.Characters)Enum.Parse(typeof(Sprites.Characters), gameObject.name);
        switch (selectedCharacter)
        {
            case Sprites.Characters.Sleepground:
                characterDescription = "���������� �ٰŸ� ����";
                specialDescription = "������ ��";
                characterMaxHealth = 200;
                break;
            case Sprites.Characters.Rather:
                characterDescription = "���� ��� ����";
                specialDescription = "������ ����";
                characterMaxHealth = 200;
                break;
            case Sprites.Characters.Dino:
                characterDescription = "���������� ��Ÿ�����";
                specialDescription = "�߱��� ������ ����";
                characterMaxHealth = 100;
                break;
        }
    }
}
