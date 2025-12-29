using System.Collections;
using UnityEngine;

public class DreamGhost_ButterflyParticle : BaseController
{
    public void OnCreated()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float brightness = RandomUtil.RandomNumber(-30, 30) / 100f;
        sr.SetBrightness(brightness);

        transform.localScale = Vector3.one * RandomUtil.RandomNumber(5, 10);
        transform.AddX(RandomUtil.RandomNumber(-20, 20));
        transform.AddY(RandomUtil.RandomNumber(-20, 20));
        Destroy(gameObject, 1f);
    }
}
