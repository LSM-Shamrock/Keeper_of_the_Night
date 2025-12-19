using System.Collections;
using UnityEngine;
using static Utility;

public class DreamGhost_ButterflyParticle : PlaySceneObjectBase
{
    public void OnCreated()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float brightness = RandomNumber(-30, 30);
        sr.color = brightness < 0 ? Color.black : Color.white;
        sr.SetBrightness(Mathf.Abs(brightness) / 100f * 0.75f);

        transform.localScale = Vector3.one * RandomNumber(5, 10);
        transform.AddX(RandomNumber(-20, 20));
        transform.AddY(RandomNumber(-20, 20));
        Destroy(gameObject, 1f);
    }
}
