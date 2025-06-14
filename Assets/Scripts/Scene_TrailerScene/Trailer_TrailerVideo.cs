using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static Utile;

public class Trailer_TrailerVideo : ObjectBase
{
    VideoPlayer _videoPlayer;

    protected override void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();

        VideoClip clip = _videoPlayer.clip;
        _videoPlayer.Play();
        Invoke(nameof(GoStart), (float)clip.length);
    }

    void GoStart()
    {
        StartScene(Scenes.LobbyScene);
    }
}
