using System.Collections;
using UnityEngine;

public class EnemyGenerator : BaseController
{
    private Vector3 _createPosition = new Vector3(250f, 0f);
    
    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        StartCoroutine(Loop());
    }

    private void CreateEnemy(Enemys type)
    {
        GameObject prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Scene_Play.Enemy);
        GameObject go = prefab.CreateClone();
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.transform.position = _createPosition;
        enemy.Init(type);
    }

    private IEnumerator CreateEnemyAndWait(int enemyChoice)
    {
        if (enemyChoice == 9 || enemyChoice == 10)
            enemyChoice = Utility.RandomNumber(2, 8);
        if (enemyChoice == 14)
            enemyChoice = Utility.RandomNumber(2, 13);

        if (Utility.RandomNumber(1, 2) == 1)
            _createPosition.x = 300;
        else
            _createPosition.x = -300;

        if (enemyChoice == 2) CreateEnemy(Enemys.VoidCavity);
        if (enemyChoice == 3) CreateEnemy(Enemys.CrazyLaughMask);
        if (enemyChoice == 4) CreateEnemy(Enemys.MotherSpiritSnake);
        if (enemyChoice == 5) CreateEnemy(Enemys.Bird);
        if (enemyChoice == 6) CreateEnemy(Enemys.SadEyes);
        if (enemyChoice == 8) CreateEnemy(Enemys.ThePiedPiper);
        if (enemyChoice == 11) CreateEnemy(Enemys.Fire);
        if (enemyChoice == 12) CreateEnemy(Enemys.Red);
        if (enemyChoice == 13) CreateEnemy(Enemys.SnowLady);

        if (Manager.Game.isNightmare)
            yield return new WaitForSeconds(Utility.RandomNumber(2.5f, 3.75f));
        else
            yield return new WaitForSeconds(Utility.RandomNumber(2.5f, 5f));
    }
    
    private IEnumerator Loop()
    {
        while (true)
        {
            if (Manager.Game.wave == 0)
            {
                yield return CreateEnemyAndWait(12);
            }
            else if (Manager.Game.wave == 1)
            {
                CreateEnemy(Enemys.Shadow);
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
                    CreateEnemy(Enemys.BossDino);
                    while (Manager.Game.wave == 15)
                    {
                        yield return CreateEnemyAndWait(Utility.RandomNumber(2, 13));
                        yield return new WaitForSeconds(3f);
                    }
                }
                else
                {
                    CreateEnemyAndWait(Manager.Game.wave);
                    int checkingWave = Manager.Game.wave;
                    while (Manager.Game.wave == checkingWave)
                    {
                        yield return CreateEnemyAndWait(Utility.RandomNumber(2, Manager.Game.wave));
                    }
                }
            }
            yield return null;
        }
    }
}
