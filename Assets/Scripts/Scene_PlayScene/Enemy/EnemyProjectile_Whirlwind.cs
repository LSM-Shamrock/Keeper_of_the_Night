using System.Collections;
using UnityEngine;

public class EnemyProjectile_Whirlwind : EnemyProjectile
{
    bool IsContactGround => _col.IsContact(Objects.Ground);
    protected override IEnumerator Start_Shoot()
    {
        transform.localScale = Vector3.one * 5f;
        _sr.SetTransparency(0.25f);
        Vector3 direction = (Character.transform.position - transform.position).normalized;
        foreach (var i in Count(5))
        {
            foreach (var j in Count(6))
            {
                if (!IsContactGround)
                    transform.position += direction * 5f;

                transform.AddX(2f);
                transform.localScale += Vector3.one * 1f;

                if (IsContactMoonlightswordShield)
                    DestroyThisClone();

                yield return waitForFixedUpdate;
            }
            foreach (var j in Count(6))
            {
                if (!IsContactGround)
                    transform.position += direction * 5f;

                transform.AddX(-2f);
                transform.localScale += Vector3.one * 1f;

                if (IsContactMoonlightswordShield)
                    DestroyThisClone();

                yield return waitForFixedUpdate;
            }
        }
        foreach (var i in Count(20))
        {
            _sr.AddTransparency(0.03f);
            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }
    protected override IEnumerator Routine_ContactCharacter()
    {
        while (true)
        {
            if (IsContactCharacter)
            {
                TakeDamageToPlayer(2);
                yield return WaitForSeconds(0.1f);
            }
            yield return waitForFixedUpdate;
        }
    }
}
