using System.Collections;
using UnityEngine;

public class EnemyHiddenNameParticle : BaseController
{
    public void Init(Vector3 position)
    {
        SpriteRenderer sr = gameObject.Component<SpriteRenderer>();

        transform.localScale = Vector3.one * Utility.RandomNumber(8, 10);
        sr.SetBrightness(Utility.RandomNumber(-0.05f, 0.25f));
        transform.position = position;
        transform.AddX(Utility.RandomNumber(-50, 50));
        transform.AddY(Utility.RandomNumber(-10, 10));
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        yield return transform.MoveOverTime(0.05f, Vector3.up * 5f);
        yield return WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

}
