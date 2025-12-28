using System.Collections;
using UnityEngine;

public abstract class EnemyProjectile : BaseController
{
    protected bool IsContactCharacter => _col.IsContact(PlaySceneObjects.Character);
    protected bool IsContactMoonlightswordShield => _col.IsContact(PlaySceneObjects.MoonlightswordShield);
    protected SpriteRenderer _sr;
    protected Collider2D _col;
    protected Vector3 _direction;
    protected void DestroyThisClone()
    {
        Manager.Object.Despawn(gameObject);
    }
    public virtual void OnCreate(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        _direction = direction;
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        StartCoroutine(Start_Shoot());
        StartCoroutine(Routine_ContactCharacter());
    }
    protected abstract IEnumerator Start_Shoot();
    protected abstract IEnumerator Routine_ContactCharacter();
}
