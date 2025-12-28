using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyPlayButton : UIBase
{
    [SerializeField] int _wave;
    [SerializeField] float _size1;
    [SerializeField] float _size2;

    private void Start()
    {
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
                    yield return new WaitUntil(() => !Input.GetMouseButton(0));
                    if (IsContactMousePointer)
                    {
                        size = _size2;
                        yield return new WaitForSeconds(0.1f);
                        Manager.Game.wave = _wave;
                        Utility.StartScene(Scenes.PlayScene);
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
