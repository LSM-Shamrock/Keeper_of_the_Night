using System.Collections;
using UnityEngine;

public class Background_Ship : PlaySceneObjectBase
{
    SpriteRenderer _sr;
    protected override void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    protected override void Update()
    {
        _sr.enabled = Manager.Game.wave == 15;
    }
}
