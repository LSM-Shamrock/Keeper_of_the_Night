using System.Collections;
using UnityEngine;

public class CharacterSkill_CameraFlash : BaseController
{
    private SpriteRenderer _sr;
    private Vector3 _direction;

    public void Init(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        _direction = direction;
        _sr = GetComponent<SpriteRenderer>();
        StartCoroutine(CloneEffect());
    }

    private IEnumerator CloneEffect()
    {
        _sr.SetTransparency(0.9f);
        foreach (int i in Count(5))
        {
            transform.localScale += Vector3.one * 30f;
            transform.position += _direction * 10f;
            _sr.AddTransparency(-0.02f);
            yield return new WaitForFixedUpdate();
        }
        Manager.Object.Despawn(gameObject);
    }

}
