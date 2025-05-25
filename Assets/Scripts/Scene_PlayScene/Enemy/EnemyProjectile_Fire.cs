using System.Collections;
using UnityEngine;

public class EnemyProjectile_Fire : EnemyProjectile
{
    protected override IEnumerator Start_Shoot()
    {
        _sr.SetTransparency(0.1f);
        transform.localScale = Vector3.one * 25;
        foreach (int i in Count(30))
        {
            transform.localScale += Vector3.one * 0.5f;
            transform.position += _direction * 8f;

            if (ContactMoonlightswordShield)
                DestroyThisClone();

            if (ContactCharacter)
            {
                while (true)
                {
                    transform.position = Character.position;
                    yield return waitForFixedUpdate;
                }
            }

            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }
    protected override IEnumerator Routine_ContactCharacter()
    {
        yield return WaitUntil(() => ContactCharacter);
        _sr.SetTransparency(0.5f);
        transform.localScale = Vector3.one * 50f;
        foreach (int i in Count(10))
        {
            TakeDamageToPlayer(1);
            _sr.AddTransparency(0.04f);
            yield return WaitForSeconds(0.2f);
            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }
}
