using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class EnemyGenerator : BaseController
{
    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        StartCoroutine(Loop());
    }

    Vector3 position = new Vector3(250f, 0f);
    
    void CreateEnemy(EnemyType type)
    {
        GameObject prefab = LoadResource<GameObject>(Prefabs.Scene_Play.Enemy);
        GameObject go = prefab.CreateClone();
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.transform.position = position;
        enemy.Init(type);
    }

    IEnumerator Logic_CreateEnemy(int enemyChoice)
    {
        if (enemyChoice == 9 || enemyChoice == 10)
            enemyChoice = Utility.RandomNumber(2, 8);
        if (enemyChoice == 14)
            enemyChoice = Utility.RandomNumber(2, 13);

        if (Utility.RandomNumber(1, 2) == 1)
            position.x = 300;
        else
            position.x = -300;

        if (enemyChoice == 2)
            CreateEnemy(EnemyType.VoidCavity);
        if (enemyChoice == 3)
            CreateEnemy(EnemyType.CrazyLaughMask);
        if (enemyChoice == 4)
            CreateEnemy(EnemyType.MotherSpiritSnake);
        if (enemyChoice == 5)
            CreateEnemy(EnemyType.Bird);
        if (enemyChoice == 6)
            CreateEnemy(EnemyType.SadEyes);
        if (enemyChoice == 8)
            CreateEnemy(EnemyType.ThePiedPiper);
        if (enemyChoice == 11)
            CreateEnemy(EnemyType.Fire);
        if (enemyChoice == 12)
            CreateEnemy(EnemyType.Red);
        if (enemyChoice == 13)
            CreateEnemy(EnemyType.SnowLady);

        if (Manager.Game.isNightmare)
            yield return new WaitForSeconds(Utility.RandomNumber(2.5f, 3.75f));
        else
            yield return new WaitForSeconds(Utility.RandomNumber(2.5f, 5f));
    }
    
    IEnumerator Loop()
    {
        while (true)
        {
            if (Manager.Game.wave == 0)
            {
                yield return Logic_CreateEnemy(12);
            }
            else if (Manager.Game.wave == 1)
            {
                CreateEnemy(EnemyType.Shadow);
                yield return new WaitUntil(() => Manager.Game.wave != 1);
            }
            else
            {
                if (Manager.Game.wave == 7)
                {
                    Manager.Game.onDreamghostAppearance.Call();
                }
                if (Manager.Game.wave == 15)
                {
                    CreateEnemy(EnemyType.BossDino);
                    while (Manager.Game.wave == 15)
                    {
                        yield return Logic_CreateEnemy(Utility.RandomNumber(2, 13));
                        yield return new WaitForSeconds(3f);
                    }
                }
                else
                {
                    Logic_CreateEnemy(Manager.Game.wave);
                    int checkingWave = Manager.Game.wave;
                    while (Manager.Game.wave == checkingWave)
                    {
                        yield return Logic_CreateEnemy(Utility.RandomNumber(2, Manager.Game.wave));
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
