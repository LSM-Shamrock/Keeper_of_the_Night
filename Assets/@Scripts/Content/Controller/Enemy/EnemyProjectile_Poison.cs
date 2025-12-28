using System.Collections;
using UnityEngine;

public class EnemyProjectile_Poison : EnemyProjectile
{
    protected override IEnumerator Start_Shoot()
    {
        _sr.SetTransparency(0.75f);
        transform.localScale = Vector3.one * 5f;
        foreach (int i in Count(35))
        {
            transform.localScale += Vector3.one * 0.5f;
            transform.position += _direction * 6f;

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
        transform.localScale = Vector3.one * 20f;
        foreach (int i in Count(10))
        {
            Manager.Game.TakeDamageToPlayer(1);
            _sr.AddTransparency(0.04f);
            yield return new WaitForSeconds(0.2f);
        }
        DestroyThisClone();
    }
}
