using System.Collections;
using UnityEngine;

public class DreamGhost_ButterflyParticle : BaseController
{
    public void OnCreated()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float brightness = Util.RandomNumber(-30, 30) / 100f;
        sr.SetBrightness(brightness);

        transform.localScale = Vector3.one * Util.RandomNumber(5, 10);
        transform.AddX(Util.RandomNumber(-20, 20));
        transform.AddY(Util.RandomNumber(-20, 20));
        Destroy(gameObject, 1f);
    }
}
