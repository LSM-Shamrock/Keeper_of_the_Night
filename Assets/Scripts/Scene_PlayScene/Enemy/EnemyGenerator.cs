using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utile;

public class EnemyGenerator : PlaySceneObjectBase
{
    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        StartCoroutine(Loop_SelectEnemyCreate());
    }

    Sprites.Enemys RandomEnemy()
    {
        List<Sprites.Enemys> list = new();
        foreach (Sprites.Enemys enemy in Enum.GetValues(typeof(Sprites.Enemys)))
        {
            switch (enemy)
            {
                case Sprites.Enemys.Shadow:
                case Sprites.Enemys.DreamGhost:
                case Sprites.Enemys.BossDino:
                    break;
                default:
                    if ((int)enemy <= wave) list.Add(enemy); 
                    break;
            }
        }
        return Utile.RandomElement(list);
    }
    
    void Create(Sprites.Enemys type)
    {
        Vector3 position = Vector3.zero;
        position.x = RandomNumber(1, 2) == 1 ? 300 : -300;

        Debug.Log( $"{type.ToString()} ¼ÒÈ¯ÇÔ");

        GameObject prefab = LoadResource<GameObject>(Prefabs.Enemy);
        GameObject go = prefab.CreateClone();
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.transform.position = position;
        enemy.Init(type);
    }
    
    IEnumerator CreateAndWait(Sprites.Enemys type)
    {
        Create(type);
        if (isNightmare)
        {
            yield return WaitForSeconds(RandomNumber(2.5f, 3.75f));
        }
        else
        {
            yield return WaitForSeconds(RandomNumber(2.5f, 5f));
        }
    }
    
    IEnumerator Loop_SelectEnemyCreate()
    {
        while (true)
        {
            if (wave == 0)
            {
                yield return CreateAndWait(Sprites.Enemys.Red);
            }
            else if (wave == 1)
            {
                Create(Sprites.Enemys.Shadow);
                yield return WaitUntil(() => wave != 1);
            }
            else if (wave == 15)
            {
                Create(Sprites.Enemys.BossDino);
                while (wave == 15)
                {
                    yield return CreateAndWait(RandomEnemy());
                    yield return WaitForSeconds(3f);
                    yield return waitForFixedUpdate;
                }
            }
            else
            {
                if (wave == 7) onDreamghostAppearance.Call();
                else if (Enum.IsDefined(typeof(Sprites.Enemys), wave))
                    yield return CreateAndWait((Sprites.Enemys)wave);

                int generateWave = wave;
                while (wave == generateWave)
                {
                    yield return CreateAndWait(RandomEnemy());
                    yield return waitForFixedUpdate;
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
