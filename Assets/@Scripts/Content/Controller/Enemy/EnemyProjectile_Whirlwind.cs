using System.Collections;
using UnityEngine;

public class EnemyProjectile_Whirlwind : EnemyProjectile
{
    bool IsContactGround => _col.IsContact(PlaySceneObjects.Ground);
    protected override IEnumerator StartShoot()
    {
        transform.localScale = Vector3.one * 5f;
        _sr.SetTransparency(0.25f);
        Vector3 direction = (Manager.Object.Character.transform.position - transform.position).normalized;
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

                yield return new WaitForFixedUpdate();
            }
            foreach (var j in Count(6))
            {
                if (!IsContactGround)
                    transform.position += direction * 5f;

                transform.AddX(-2f);
                transform.localScale += Vector3.one * 1f;

                if (IsContactMoonlightswordShield)
                    DestroyThisClone();

                yield return new WaitForFixedUpdate();
            }
        }
        foreach (var i in Count(20))
        {
            _sr.AddTransparency(0.03f);
            yield return new WaitForFixedUpdate();
        }
        DestroyThisClone();
    }
    protected override IEnumerator StartContactCharacter()
    {
        while (true)
        {
            if (IsContactCharacter)
            {
                Manager.Game.TakeDamageToPlayer(2);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
