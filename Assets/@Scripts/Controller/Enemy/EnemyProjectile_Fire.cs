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

            if (IsContactMoonlightswordShield)
                DestroyThisClone();

            if (IsContactCharacter)
            {
                while (true)
                {
                    transform.position = Manager.Object.Character.position;
                    yield return new WaitForFixedUpdate();
                }
            }

            yield return new WaitForFixedUpdate();
        }
        DestroyThisClone();
    }
    protected override IEnumerator Routine_ContactCharacter()
    {
        yield return new WaitUntil(() => IsContactCharacter);
        _sr.SetTransparency(0.5f);
        transform.localScale = Vector3.one * 50f;
        foreach (int i in Count(10))
        {
            Manager.Game.TakeDamageToPlayer(1);
            _sr.AddTransparency(0.04f);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitForFixedUpdate();
        }
        DestroyThisClone();
    }
}
