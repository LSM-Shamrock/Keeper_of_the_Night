using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Utile;

public class Trailer_Trailer : ObjectBase
{
    [SerializeField]
    Sprite[] _sprites;
    [SerializeField]
    AudioClip _audioClip;

    protected override void Start()
    {
        StartCoroutine(PlayImage());
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlayImage()
    {
        Image image = GetComponent<Image>();
        foreach (var sprite in _sprites)
        {
            image.sprite = sprite;
            yield return WaitForSeconds(44f / 107);
        }
        image.color = Color.black;
    }

    private IEnumerator PlaySound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.25f;
        audioSource.clip = _audioClip;
        audioSource.Play();
        yield return WaitForSeconds(_audioClip.length);
        yield return WaitForSeconds(1f);
        StartScene(Scenes.LobbyScene);
    }
}
