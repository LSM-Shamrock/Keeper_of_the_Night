using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lobby_Thubnail : BaseController
{
    [SerializeField]
    Sprite[] _sprites;

    protected override void Start()
    {
        StartCoroutine(Co_ChangeSprite());
        StartCoroutine(Co_MoveToMouse());
    }

    private IEnumerator Co_ChangeSprite()
    {
        Image[] images = GetComponentsInChildren<Image>();
        Image backImage = images[0];
        Image frontImage = images[1];

        int spriteIndex = 0;
        float size = 0.75f;
        Color color = Color.white;

        void SetNextSprite()
        {
            spriteIndex++;
            spriteIndex %= _sprites.Length;
            frontImage.sprite = _sprites[spriteIndex];
        }

        void SetSize(float value)
        {
            size = value;
            frontImage.transform.localScale = Vector3.one * value;
        }
        void AddSize(float value)
        {
            size += value;
            frontImage.transform.localScale = Vector3.one * value;
        }

        void SetTransparency(float value)
        {
            color.a = 1f - value;
            frontImage.color = color;
        }
        void AddTransparency(float value)
        {
            color.a -= value;
            frontImage.color = color;
        }

        void UpdateClone()
        {
            backImage.sprite = frontImage.sprite;
            backImage.transform.localScale = frontImage.transform.localScale;
            backImage.color = frontImage.color;
        }

        while (true)
        {
            SetSize(0.75f);
            UpdateClone();
            SetTransparency(1f);
            SetNextSprite();
            for (int i = 100; i > 0; i--)
            {
                AddTransparency(-0.01f);
                yield return new WaitForFixedUpdate();
            }
            if (spriteIndex != 0 && spriteIndex != 16 && spriteIndex != 19)
            {
                yield return WaitForSeconds(1.5f);
            }
            if (spriteIndex == 15 || spriteIndex == 18)
            {
                yield return WaitForSeconds(6f);
                UpdateClone();
                SetTransparency(1f);
                SetSize(0f);
                SetNextSprite();
                for (int i = 50; i > 0; i--)
                {
                    AddTransparency(-0.02f);
                    AddSize(0.015f);
                }
            }
        }
    }

    private IEnumerator Co_MoveToMouse()
    {
        while (true)
        {
            if (!Input.GetMouseButton(0)) { yield return null; continue; }
            if (!IsContactMousePointer) { yield return null; continue; }

            transform.position = Utility.MousePosition;
            yield return null;
        }
    }

}
