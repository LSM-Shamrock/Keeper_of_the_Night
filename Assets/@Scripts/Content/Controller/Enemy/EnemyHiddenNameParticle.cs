using System.Collections;
using UnityEngine;

public class EnemyHiddenNameParticle : BaseController
{
    public void Init(Vector3 position)
    {
        SpriteRenderer sr = gameObject.Component<SpriteRenderer>();

        transform.localScale = Vector3.one * RandomUtil.RandomNumber(8, 10);
        sr.SetBrightness(RandomUtil.RandomNumber(-0.05f, 0.25f));
        transform.position = position;
        transform.AddX(RandomUtil.RandomNumber(-50, 50));
        transform.AddY(RandomUtil.RandomNumber(-10, 10));
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return transform.MoveOverTime(0.05f, Vector3.up * 5f);
        Manager.Object.DespawnAfterSec(gameObject, 0.1f);
    }

}
