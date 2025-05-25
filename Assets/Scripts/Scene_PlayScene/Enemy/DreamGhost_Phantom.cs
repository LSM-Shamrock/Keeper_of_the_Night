using System.Collections;
using UnityEngine;
using static Utile;

public class DreamGhost_Phantom : EnemyBase
{
    public override void Init()
    {
        base.Init();
        onNightmareEvent.Add(this, DeleteThisClone);
        gameObject.SetSpriteAndPolygon(LoadResource<Sprite>(currentCharacter));
        transform.SetX(RandomNumber(1, 2) == 1 ? 300 : -300);
        StartCoroutine(Loop());
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        Destroy(gameObject);
        yield break;
    }

    IEnumerator Loop()
    {
        while (true)
        {
            Vector3 direction = (Character.position - transform.position).normalized;
            transform.position += direction * 0.7f;

            if (IsContactGround)
            {
                if (Mathf.Abs(transform.GetX() - Character.GetX()) < 30f)
                {
                    foreach (int i in Count(5))
                    {
                        transform.AddY(5f);
                        yield return waitForFixedUpdate;
                    }
                    if (IsContactCharacter)
                        TakeDamageToPlayer(9);

                    while (!IsContactGround)
                    {
                        transform.AddY(-2f);
                        yield return waitForFixedUpdate;
                    }
                    yield return WaitForSeconds(0.5f);
                }
            }
            else
                transform.AddY(-1f);

            yield return waitForFixedUpdate;
        }
    }
}
