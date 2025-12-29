using System.Collections;
using UnityEngine;

public class DreamGhost_Phantom : EnemyBase
{
    public override void Init()
    {
        base.Init();
        Manager.Game.onNightmareEvent.Add(this, DeleteThisClone);
        Util.SetSpriteAndPolygon(gameObject, Manager.Resource.LoadResource<Sprite>(Manager.Game.currentCharacter));


        float cameraX = Manager.Object.MainCamera.transform.position.x;
        float dist = Define.EnemySpawnDistance;
        Vector3 pos = transform.position;
        pos.x = cameraX + RandomUtil.RandomSign() * dist;
        transform.position = pos; 
        StartCoroutine(Loop());
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        Manager.Object.Despawn(gameObject);
        yield break;
    }

    IEnumerator Loop()
    {
        Transform character = Manager.Object.Character;
        while (true)
        {
            Vector3 direction = (character.position - transform.position).normalized;
            transform.position += direction * 0.7f;

            if (IsContactGround)
            {
                if (Mathf.Abs(transform.position.x - character.position.x) < 30f)
                {
                    foreach (int i in Count(5))
                    {
                        transform.position += Vector3.up * 5f;
                        yield return new WaitForFixedUpdate();
                    }
                    if (IsContactCharacter)
                        Manager.Game.TakeDamageToPlayer(9);

                    while (!IsContactGround)
                    {
                        transform.position += Vector3.down * 2f;
                        yield return new WaitForFixedUpdate();
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else
                transform.position += Vector3.down * 1f;

            yield return new WaitForFixedUpdate();
        }
    }
}
