using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Utile;

public class Trailer_ClickToStart : ObjectBase
{
    protected override void Start()
    {
        SetCusorSprite();
        StartCoroutine(Co_SizeUpdate());
        StartCoroutine(Co_ClickCheck());
    }

    private void SetCusorSprite()
    {
        Texture2D texture2D = LoadResource<Texture2D>(Sprites.Cursor.Moonlightsword);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.ForceSoftware);
    }

    private IEnumerator Co_SizeUpdate()
    {
        float size = 2f;
        while (true)
        {
            for (int i = 50; i > 0; i--)
            {
                size += 0.004f;
                transform.localScale = Vector3.one * size;
                yield return new WaitForFixedUpdate();
            }
            for (int i = 50; i > 0; i--)
            {
                size -= 0.004f;
                transform.localScale = Vector3.one * size;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator Co_ClickCheck()
    {
        Text text = GetComponentInChildren<Text>();
        Color color = text.color;
        color.a = 0f;
        text.color = color;
        yield return WaitForSeconds(0.5f);
        color.a = 1f;
        text.color = color;
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                yield return WaitForSeconds(0.1f);
                StartScene(Scenes.LobbyScene);
            }
            yield return null;
        }
    }
}
