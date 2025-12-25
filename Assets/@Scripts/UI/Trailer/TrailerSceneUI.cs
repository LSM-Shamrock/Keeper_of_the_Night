using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrailerSceneUI : SceneUI
{
    private ChildKey<Image> Trailer = new(nameof(Trailer));
    private ChildKey<Text> ClickToStart = new(nameof(ClickToStart));

    private AudioSource _audioSource;

    [SerializeField] private Sprite[] _trailerSprites;
    [SerializeField] private AudioClip _trailerAudioClip;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        BindChild(
        Trailer,
        ClickToStart);

        // 트레일러씬 시작시 커서 모양 설정
        Texture2D texture2D = Manager.Resource.LoadResource<Texture2D>(Sprites.Cursor.Moonlightsword);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);

        StartCoroutine(PlayTrailerImage());
        StartCoroutine(PlayTrailerSound());
        StartCoroutine(TextSizeUpdate());
        StartCoroutine(ClickCheck());
    }

    private IEnumerator PlayTrailerImage()
    {
        foreach (Sprite sprite in _trailerSprites)
        {
            GetChild(Trailer).sprite = sprite;
            yield return new WaitForSeconds(44f / 107);
        }
        GetChild(Trailer).color = Color.black;
    }
    private IEnumerator PlayTrailerSound()
    {
        _audioSource.volume = 0.25f;
        _audioSource.clip = _trailerAudioClip;
        _audioSource.Play();
        yield return new WaitForSeconds(_trailerAudioClip.length);
        yield return new WaitForSeconds(1f);
        Utility.StartScene(Scenes.LobbyScene);
    }

    private IEnumerator TextSizeUpdate()
    {
        float size = 2f;
        Transform textTransform = GetChild(ClickToStart).transform;
        while (true)
        {
            for (int i = 50; i > 0; i--)
            {
                size += 0.004f;
                textTransform.localScale = Vector3.one * size;
                yield return new WaitForFixedUpdate();
            }
            for (int i = 50; i > 0; i--)
            {
                size -= 0.004f;
                textTransform.localScale = Vector3.one * size;
                yield return new WaitForFixedUpdate();
            }
        }
    }
    private IEnumerator ClickCheck()
    {
        Text text = GetChild(ClickToStart);
        Color color = text.color;

        color.a = 0f;
        text.color = color;

        yield return new WaitForSeconds(1f);

        color.a = 1f;
        text.color = color;

        yield return new WaitUntil(() => Input.GetMouseButton(0));

        yield return new WaitForSeconds(0.1f);
        Utility.StartScene(Scenes.LobbyScene);
    }
}
