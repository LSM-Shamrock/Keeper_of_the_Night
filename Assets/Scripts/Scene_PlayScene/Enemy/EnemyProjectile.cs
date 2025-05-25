using System.Collections;
using UnityEngine;

public abstract class EnemyProjectile : PlaySceneObjectBase
{
    protected bool ContactCharacter => _col.IsContact(Objects.Character);
    protected bool ContactMoonlightswordShield => false;
    protected SpriteRenderer _sr;
    protected Collider2D _col;
    protected Vector3 _direction;
    protected void DestroyThisClone()
    {
        Destroy(gameObject);
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
