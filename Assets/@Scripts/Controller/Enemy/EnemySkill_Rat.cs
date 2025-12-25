using System.Collections;
using UnityEngine;

public class EnemySkill_Rat : EnemyBase
{
    int _hp = 4;

    public void OnCreate()
    {
        base.Init();
        Manager.Game.onNightmareEvent.Add(this, DeleteThisClone);
        transform.SetX(Utility.RandomNumber(1, 2) == 1 ? 300f : -300f);
        StartCoroutine(Routine_Move());
        StartCoroutine(Routine_Attack());
    }

    IEnumerator Routine_Move()
    {
        while (true)
        {
            if (IsContactGround)
                transform.AddY(0.1f);
            else
                transform.AddY(-2f);

            _sr.flipX = Manager.Game.Character.GetX() < transform.GetX();

            if (DistanceTo(Manager.Game.Character) > 25f || IsContactWall)
            {
                if (Manager.Game.Character.GetX() > transform.GetX())
                    transform.AddX(1.5f);
                else
                    transform.AddX(-1.5f);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Routine_Attack()
    {
        while (true)
        {
            if (Mathf.Abs(transform.GetX() - Manager.Game.Character.GetX()) < 30 && IsContactGround)
            {
                foreach (var i in Count(5))
                {
                    transform.AddY(4f);
                    yield return new WaitForFixedUpdate();
                }
                if (IsContactCharacter)
                    Manager.Game.TakeDamageToPlayer(2);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        _hp -= damage;
        if (_hp > 0)
            yield return Manager.Speech.SpeechForSeconds(transform, _hp.ToString(), 0.1f);
        else 
            DeleteThisClone();
    }
}
