using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveClear : BaseController
{
    CanvasGroup _group;
    protected override void Start()
    {
        _group = GetComponent<CanvasGroup>();
        Manager.Game.onWaveClear.Add(this, () => StartCoroutine(OnWaveClear()));
        StartCoroutine(Start_Effect());
    }

    IEnumerator OnWaveClear()
    {
        _group.alpha = 0.5f;

        Vector3 position = transform.position;
        position.x = 300;
        transform.position = position;

        for (int i = 12; i > 0; i--)
        {
            transform.position += Vector3.right * -25f;
            yield return waitForFixedUpdate;
        }
        Manager.Game.remainingHealth += 10;
        yield return WaitForSeconds(1f);
        for (int i = 25; i > 0; i--)
        {
            _group.alpha -= 0.02f;
            yield return waitForFixedUpdate;
        }

        _group.alpha = 0f;
    }

    IEnumerator Start_Effect()
    {
        Image effect = transform.GetChild(0).GetComponent<Image>();
        while (true)
        {
            for (int i = 10; i > 0; i--)
            {
                Color color = effect.color;
                color.a += 0.05f/2f;
                effect.color = color;
                yield return waitForFixedUpdate;
            }
            for (int i = 10; i > 0; i--)
            {
                Color color = effect.color;
                color.a += -0.05f/2f;
                effect.color = color;
                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }
    }
}
