using System.Collections;
using UnityEngine;

public class EnemyProjectile_Poison : EnemyProjectile
{
    protected override IEnumerator StartShoot()
    {
        transform.localScale = Vector3.one * 5f;
        _sr.SetTransparency(0.75f);

        foreach (int i in Count(35))
        {
            yield return new WaitForFixedUpdate();

            transform.localScale += Vector3.one * 0.5f;
            transform.position += _direction * 6f;

            if (IsContactMoonlightswordShield)
                DestroyThisClone();

            if (IsContactCharacter)
            {
                transform.localScale = Vector3.one * 20f;

                _sr.SetTransparency(0.5f);

                StartCoroutine(CoroutineUtil.LoopRunAndWait(0.2f, 10, () =>
                {
                    Manager.Game.TakeDamageToPlayer(1);
                    _sr.AddTransparency(0.04f);
                },
                DestroyThisClone));

                while (true)
                {
                    transform.position = Manager.Object.Character.position;
                    yield return null;
                }
            }
        }
        DestroyThisClone();
    }
}
