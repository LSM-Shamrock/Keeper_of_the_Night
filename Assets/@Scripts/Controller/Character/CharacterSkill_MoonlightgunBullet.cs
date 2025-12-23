using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_MoonlightgunBullet : BaseController
{
    Collider2D _col;
    Vector3 _direction;

    bool IsContactEnemy => _col.IsContact(Prefabs.Scene_PlayScene.Enemy);
    bool IsContactGround => _col.IsContact(PlaySceneObjects.Ground);

    public void Shoot(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        _direction = direction;
        _col = gameObject.Component<Collider2D>();
        StartCoroutine(Routine_Shoot());
    }

    void DestroyThisClone()
    {
        Destroy(gameObject);
    }

    IEnumerator Routine_Shoot()
    {
        yield return waitForFixedUpdate;
        foreach (int i in Count(30))
        {
            transform.position += _direction * 10f;
            if (IsContactEnemy || IsContactGround)
            {
                yield return WaitForSeconds(0.01f);
                DestroyThisClone();
            }
            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }
}
