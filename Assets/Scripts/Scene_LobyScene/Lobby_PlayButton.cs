using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static Utility;

public class Lobby_PlayButton : BaseController
{
    [SerializeField]
    int _wave;
    [SerializeField]
    float _size1;
    [SerializeField]
    float _size2;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Start_Loop());
    }

    IEnumerator Start_Loop()
    {
        float size;
        while (true)
        {
            if (IsContactMousePointer)
            {
                if (Input.GetMouseButton(0))
                {
                    size = _size1;
                    yield return WaitUntil(() => !Input.GetMouseButton(0));
                    if (IsContactMousePointer)
                    {
                        size = _size2;
                        yield return WaitForSeconds(0.1f);
                        Manager.Game.wave = _wave;
                        StartScene(Scenes.PlayScene);
                    }
                }
                else size = _size2;
            }
            else size = _size1;
            transform.localScale = Vector3.one * size;
            yield return null;
        }
    }
}
