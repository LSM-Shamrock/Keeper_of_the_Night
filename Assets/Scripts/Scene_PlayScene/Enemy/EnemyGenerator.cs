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
        StartCoroutine(Loop());
    }

    Vector3 position = new Vector3(250f, 0f);
    
    void CreateEnemy(Sprites.Enemys type)
    {
        GameObject prefab = LoadResource<GameObject>(Prefabs.Scene_PlayScene.Enemy);
        GameObject go = prefab.CreateClone();
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.transform.position = position;
        enemy.Init(type);
    }

    IEnumerator Logic_CreateEnemy(int enemyChoice)
    {
        if (enemyChoice == 9 || enemyChoice == 10)
            enemyChoice = Utile.RandomNumber(2, 8);
        if (enemyChoice == 14)
            enemyChoice = Utile.RandomNumber(2, 13);

        if (Utile.RandomNumber(1, 2) == 1)
            position.x = 300;
        else
            position.x = -300;

        if (enemyChoice == 2)
            CreateEnemy(Sprites.Enemys.VoidCavity);
        if (enemyChoice == 3)
            CreateEnemy(Sprites.Enemys.CrazyLaughMask);
        if (enemyChoice == 4)
            CreateEnemy(Sprites.Enemys.MotherSpiritSnake);
        if (enemyChoice == 5)
            CreateEnemy(Sprites.Enemys.Bird);
        if (enemyChoice == 6)
            CreateEnemy(Sprites.Enemys.SadEyes);
        if (enemyChoice == 8)
            CreateEnemy(Sprites.Enemys.ThePiedPiper);
        if (enemyChoice == 11)
            CreateEnemy(Sprites.Enemys.Fire);
        if (enemyChoice == 12)
            CreateEnemy(Sprites.Enemys.Red);
        if (enemyChoice == 13)
            CreateEnemy(Sprites.Enemys.SnowLady);

        if (isNightmare)
            yield return WaitForSeconds(Utile.RandomNumber(2.5f, 3.75f));
        else
            yield return WaitForSeconds(Utile.RandomNumber(2.5f, 5f));
    }
    
    IEnumerator Loop()
    {
        while (true)
        {
            if (wave == 0)
            {
                yield return Logic_CreateEnemy(12);
            }
            else if (wave == 1)
            {
                CreateEnemy(Sprites.Enemys.Shadow);
                yield return WaitUntil(() => wave != 1);
            }
            else
            {
                if (wave == 7)
                {
                    onDreamghostAppearance.Call();
                }
                if (wave == 15)
                {
                    CreateEnemy(Sprites.Enemys.BossDino);
                    while (wave == 15)
                    {
                        yield return Logic_CreateEnemy(Utile.RandomNumber(2, 13));
                        yield return WaitForSeconds(3f);
                    }
                }
                else
                {
                    Logic_CreateEnemy(wave);
                    int checkingWave = wave;
                    while (wave == checkingWave)
                    {
                        yield return Logic_CreateEnemy(Utile.RandomNumber(2, wave));
                    }
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
