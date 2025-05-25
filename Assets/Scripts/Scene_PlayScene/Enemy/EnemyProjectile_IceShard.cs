using System.Collections;
using UnityEngine;

public class EnemyProjectile_IceShard : EnemyProjectile
{
    protected override IEnumerator Routine_ContactCharacter()
    {
        yield return WaitUntil(() => ContactCharacter);
        TakeDamageToPlayer(11);
        _sr.sprite = Utile.LoadResource<Sprite>(Sprites.EnemySkill.Ice);
        _sr.SetTransparency(0.5f);
        transform.localScale = Vector3.one * 50f;
        ice += 4;
        yield return WaitForSeconds(0.3f);
        foreach (int i in Count(10))
        {
            _sr.AddTransparency(0.05f);
            yield return WaitForSeconds(0.03f);
            yield return waitForFixedUpdate;
        }
        DestroyThisClone();
    }

    protected override IEnumerator Start_Shoot()
    {
        _sr.SetTransparency(0.1f);
        transform.localScale = Vector3.one * 10f;
        foreach (int i in Count(20))
        {
            transform.position += _direction * 12f;
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
}
