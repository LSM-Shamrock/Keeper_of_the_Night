using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_MoonlightGunBullet : BaseController
{
    private Collider2D _col;
    private Vector3 _direction;

    private bool IsContactEnemy => _col.IsContact(Prefabs.Play.Enemy);
    private bool IsContactGround => _col.IsContact(PlaySceneObjects.Ground);

    public void Shoot(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        _direction = direction;
        _col = gameObject.Component<Collider2D>();
        StartCoroutine(RoutineShoot());
    }

    private void DestroyThisClone()
    {
        Manager.Object.Despawn(gameObject);
    }

    private IEnumerator RoutineShoot()
    {
        yield return new WaitForFixedUpdate();
        foreach (int i in Count(30))
        {
            transform.position += _direction * 10f;
            if (IsContactEnemy || IsContactGround)
            {
                yield return new WaitForSeconds(0.01f);
                DestroyThisClone();
            }
            yield return new WaitForFixedUpdate();
        }
        DestroyThisClone();
    }
}
