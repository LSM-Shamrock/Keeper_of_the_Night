using System.Collections;
using UnityEngine;

public class EnemyProjectile_DinoProjectile : EnemyProjectile
{
    protected override IEnumerator Routine_ContactCharacter()
    {
        while (true)
        {
            if (IsContactCharacter)
            {
                TakeDamageToPlayer(2);
                yield return WaitForSeconds(0.01f);
                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }
    }
    protected override IEnumerator Start_Shoot()
    {
        _sr.SetTransparency(0.25f);
        transform.localScale = Vector3.one * 5f;
        foreach (int i in Count(40))
        {
            transform.localScale += Vector3.one * 0.5f;
            transform.position += _direction * 7f;

            if (IsContactMoonlightswordShield)
                DestroyThisClone();

            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }
}
