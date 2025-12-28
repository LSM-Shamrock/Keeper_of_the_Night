using System.Collections;
using UnityEngine;

public class Background_Ship : BaseController
{
    private SpriteRenderer _sr;
    
    protected override void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    
    protected override void Update()
    {
        if (Manager.Game.wave == 15)
        {
            if (_sr.color.a < 1f)
                _sr.AddAlpha(Time.deltaTime / 0.5f);
            else
                _sr.SetAlpha(1f);
        }
        else
            _sr.SetAlpha(0f);
    }
}
