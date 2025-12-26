using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    private float _hp;

    private Enemys _type;

    protected override void DeleteThisClone()
    {
        Debug.Log($"삭제함: {_type.ToString()}");
        base.DeleteThisClone();
    }

    #region 야괴 이름 외치기
    private string _hiddenName;
    private void Init_HiddenNameLogic()
    {
        switch (_type)
        {
            case Enemys.ThePiedPiper:
                _hiddenName = "하민우";
                break;
            case Enemys.BossDino:
                _hiddenName = "공룡";
                break;

            default:
                _hiddenName = "";
                _hiddenName += Utility.RandomElement(Manager.Game.hiddenSurnames);
                _hiddenName += Utility.RandomElement(Manager.Game.hiddenMainames);
                break;
        }
        StartCoroutine(Loop_HiddenNameLogic());
    }
    private void CreateNameParticles()
    {
        var loop = StartCoroutine(Loop_Create());
        StartCoroutine(StopParticle());
        IEnumerator StopParticle()
        {
            foreach (int i in Count(20))
            {
                yield return new WaitForSeconds(0.05f);
                yield return new WaitForFixedUpdate();
            }
            StopCoroutine(loop);
        }
        IEnumerator Loop_Create()
        {
            while (true)
            {
                Create();
                yield return new WaitForFixedUpdate();
            }
        }
        void Create()
        {
            var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyHiddenNameParticle);
            var go = prefab.CreateClone();
            var particle = go.Component<EnemyHiddenNameParticle>();
            particle.Init(transform.position);
        }
    }
    private IEnumerator Loop_HiddenNameLogic()
    {
        while (true)
        {
            if (Manager.Input.isPressedN)
            {
                yield return Manager.Speech.SpeechForSeconds(transform, _hiddenName, 0.01f);
                continue;
            }

            if (Manager.Game.shoutedEnemyName == _hiddenName)
            {
                Debug.Log("야괴 이름 적중 : " + _hiddenName + " : " + _type.ToString());
                yield return new WaitForSeconds(0.25f);
                CreateNameParticles();
                if (_type == Enemys.BossDino)
                {
                    yield return Manager.Speech.SpeechForSeconds(transform, "윽!", 0.75f);
                    yield return Manager.Speech.SpeechForSeconds(transform, "으아앗", 1f);
                    yield return Manager.Speech.SpeechForSeconds(transform, "이럴 줄 알았죠?", 1.5f);
                }

                else
                {
                    Manager.Speech.Speech(transform, "!");
                    Vector3 p = transform.position;
                    foreach (int i in Count(20))
                    {
                        transform.position = p;
                        yield return new WaitForSeconds(0.05f);
                        yield return new WaitForFixedUpdate();
                    }

                    Manager.Game.remainingWaveKill--;
                    if (_type == Enemys.Shadow)
                        Manager.Game.ShadowState = ShadowState.Killed;

                    DeleteThisClone();
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
    #endregion

    public void Init(Enemys type)
    {
        base.Init();
        _type = type;
        Manager.Game.onNightmareEvent.Add(this, DeleteThisClone);
        Init_HpAndSprite();
        StartCoroutine(Loop_Move());
        StartCoroutine(Loop_Attack());
        StartCoroutine(Loop_Shadow_Logic());
        StartCoroutine(Loop_Flap_Bird());
        StartCoroutine(Loop_Jump_Thepiedpiper());
        StartCoroutine(Loop_Jump_Dino());

        Init_HiddenNameLogic();
    }

    private void Init_HpAndSprite()
    {
        Sprite sprite = Manager.Resource.LoadResource<Sprite>(_type);
        gameObject.SetSpriteAndPolygon(sprite);
        float size = 0;
        switch (_type)
        {
            case Enemys.Shadow: _hp = 100; break;
            case Enemys.VoidCavity: _hp = 12; size = 31.4f; break;
            case Enemys.CrazyLaughMask: _hp = 18; size = 31.9f; break;
            case Enemys.MotherSpiritSnake: _hp = 23; size = 43.2f; break;
            case Enemys.Bird: _hp = 20; size = 31.0f; break;
            case Enemys.SadEyes: _hp = 20; size = 31.4f; break;
            case Enemys.ThePiedPiper: _hp = 18; size = 36.6f; break;
            case Enemys.Fire: _hp = 23; size = 40.5f; break;
            case Enemys.Red: _hp = 25; size = 54.9f; break;
            case Enemys.SnowLady: _hp = 23; size = 50.0f; break;
            case Enemys.BossDino: _hp = 100; size = 76.8f; break;
        }
        transform.localScale = Vector3.one * size;
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        _hp -= damage;
        if (_hp > 0)
            yield return Manager.Speech.SpeechForSeconds(transform, _hp.ToString(), 0.1f);
        else
        {
            if (_type == Enemys.BossDino)
                Manager.Game.isBossDinoKilled = true;

            Manager.Game.remainingWaveKill -= 1;
            DeleteThisClone();
        }
    }

    private void CreatePoison()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyProjectile_Poison);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Poison>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    private void CreateWhirlwind()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyProjectile_Whirlwind);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Whirlwind>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    private void CallingRat()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemySkill_Rat);
        var go = Instantiate(prefab);
        var rat = go.GetComponent<EnemySkill_Rat>();
        rat.OnCreate();
    }
    private IEnumerator PlayPipeSoundAndWaiting()
    {
        var clip = Manager.Resource.LoadResource<AudioClip>(Audios.Pipe);
        _audioSource?.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }
    private void CreateFire()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyProjectile_Fire);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Fire>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    private void CreateIceShard()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyProjectile_IceShard);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_IceShard>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    private void CreateDinoProjectile()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.EnemyProjectile_DinoProjectile);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_DinoProjectile>();
        projectile.OnCreate(transform.position, _moveDirection);
    }

    private IEnumerator Loop_Shadow_Logic()
    {
        if (_type != Enemys.Shadow)
            yield break;

        // 체력 변화: 100~0 
        // 크기 변화: 200~25 = 25+(175~0)
        // 최소 크기 25
        // hp변화당 1.75크기변화
        // 기존 카메라 타격 횟수: 100/0.5 = 200회

        isSkillIgnore = true; // 외부 스킬들 받지 않음

        float timer = -5f;
        float giantization = 0;
        const float Interval = 1f;

        while (true)
        {
            // 거대화
            if (giantization < 1f)
            {
                timer += Time.fixedDeltaTime;
                if (timer > 0f) _sr.SetBrightness(-0.75f * (timer / Interval));
                if (timer >= Interval)
                {
                    timer = 0;
                    giantization += 0.25f;
                }
            }
            else
            {
                giantization = 1f;
                Manager.Game.ShadowState = ShadowState.EndOfGiantization;
                _sr.SetBrightness(0f);
            }
            transform.localScale = Vector3.one * (25f + giantization * _hp * 1.75f);

            if (IsContactCameraLight)
            {
                _hp -= 0.5f;
                if (_hp <= 0)
                {
                    Manager.Game.ShadowState = ShadowState.Killed;
                    DeleteThisClone();
                }
            }

            // 특수기술 캔슬링
            if (IsContactMoonlightswordShield ||
                IsContactBossDinoSkill ||
                IsContactWaterPrison) Manager.Game.onDisarmSpecialSkill.Call();

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator Loop_Flap_Bird()
    {
        if (_type != Enemys.Bird)
            yield break;

        while (true)
        {
            if (transform.position.y > 60f)
            {
                for (int _ = 15; _ > 0; _--)
                {
                    transform.AddY(-0.5f);
                    yield return new WaitForFixedUpdate();
                }
            }
            for (int _ = 5; _ > 0; _--)
            {
                transform.AddY(1.5f);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator Loop_Jump_Thepiedpiper()
    {
        if (_type != Enemys.ThePiedPiper)
            yield break;

        while (true)
        {
            if (IsContactWall || 
                DistanceTo(Manager.Object.Character) > 200 ||
                DistanceTo(Manager.Object.Character) < 150)
            {
                for (int _ = 10; _ > 0; _--)
                {
                    transform.AddY(4f);
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitUntil(() => IsContactGround);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator Loop_Jump_Dino()
    {
        if (_type != Enemys.BossDino)
            yield break;

        while (true)
        {
            if (IsContactGround)
            {
                for (int _ = 40; _ > 0; _--)
                {
                    transform.AddY(3f);
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                transform.AddY(-3f);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator Loop_Move()
    {
        while (true)
        {
            IEnumerator enumerator = null;
            switch (_type)
            {
                case Enemys.Shadow: enumerator = Move_Shadow_And_Voidcavity(); break;
                case Enemys.VoidCavity: enumerator = Move_Shadow_And_Voidcavity(); break;
                case Enemys.CrazyLaughMask: enumerator = Move_Crazylaughmask_And_Sadeyes(); break;
                case Enemys.MotherSpiritSnake: enumerator = Move_Motherspiritsnake(); break;
                case Enemys.Bird: enumerator = Move_Bird(); break;
                case Enemys.SadEyes: enumerator = Move_Crazylaughmask_And_Sadeyes(); break;
                case Enemys.ThePiedPiper: enumerator = Move_Thepiedpiper(); break;
                case Enemys.Fire: enumerator = Move_Fire(); break;
                case Enemys.Red: enumerator = Move_Red(); break;
                case Enemys.SnowLady: enumerator = Move_Snowlady(); break;
                case Enemys.BossDino: enumerator = Move_Dino(); break;
            }
            yield return enumerator;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator Move_Shadow_And_Voidcavity()
    {
        if (_type != Enemys.Shadow && _type != Enemys.VoidCavity)
            yield break;

        if (IsContactGround) transform.position += Vector3.up * 0.1f;
        else transform.position += Vector3.up * -2f;
        LookAtTheTarget(Manager.Object.Character);
        if (DistanceTo(Manager.Object.Character) > 25 || IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.position += Vector3.right * 0.5f;
            if (Manager.Object.Character.position.x < transform.position.x) transform.position += Vector3.right * -0.5f;
        }
    }
    private IEnumerator Move_Crazylaughmask_And_Sadeyes()
    {
        if (_type != Enemys.CrazyLaughMask && _type != Enemys.SadEyes)
            yield break;

        LookAtTheTarget(Manager.Object.Character);
        MoveToMoveDirection(1f);
        if (_type == Enemys.CrazyLaughMask)
        {
            if (Utility.RandomNumber(1, 500) == 1)
                yield return Manager.Speech.SpeechForSeconds(transform, "ㅋ흐하하하하핳ㅋ흫흐하핳", 1f);
        }
        if (DistanceTo(Manager.Object.Character) < 50f)
        {
            for (int _ = 15; _ > 0; _--)
            {
                MoveToMoveDirection(3f);
                if (IsContactCharacter) break;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield break;
    }
    private IEnumerator Move_Motherspiritsnake()
    {
        if (_type != Enemys.MotherSpiritSnake)
            yield break;

        if (IsContactGround) transform.position += Vector3.up * 0.1f;
        else transform.position += Vector3.up * -2;
        LookAtTheTarget(Manager.Object.Character);
        if (DistanceTo(Manager.Object.Character) > 100 || IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.position += Vector3.right * 1f;
            if (Manager.Object.Character.position.x < transform.position.x) transform.position += Vector3.right * -1f;
        }

        yield break;
    }
    private IEnumerator Move_Bird()
    {
        if (_type != Enemys.Bird)
            yield break;

        if (Math.Abs(Manager.Object.Character.position.x - transform.position.x) > 50)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.position += Vector3.right * 1f;
            if (Manager.Object.Character.position.x < transform.position.x) transform.position += Vector3.right * -1f;
        }

        yield break;
    }
    private IEnumerator Move_Thepiedpiper()
    {
        if (_type != Enemys.ThePiedPiper)
            yield break;

        if (IsContactGround) 
            transform.AddY(0.1f);
        else 
            transform.AddY(-2f);

        LookAtTheTarget(Manager.Object.Character);
        if (IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(0.5f);
            if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-0.5f);
        }
        else
        {
            if (DistanceTo(Manager.Object.Character) > 200)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(0.5f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-0.5f);
            }
            if (DistanceTo(Manager.Object.Character) < 150)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(-0.5f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(0.5f);
            }
        }
    }
    private IEnumerator Move_Fire()
    {
        if (_type != Enemys.Fire)
            yield break;

        if (IsContactGround)
            transform.AddY(0.1f);
        else
            transform.AddY(-2f);

        LookAtTheTarget(Manager.Object.Character);
        if (DistanceTo(Manager.Object.Character) > 150 || IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(1.2f);
            if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-1.2f);
        }
    }
    private IEnumerator Move_Red()
    {
        if (_type != Enemys.Red)
            yield break;

        if (IsContactGround)
            transform.position += Vector3.up * 0.1f;
        else
            transform.position += Vector3.up * -2f;
        LookAtTheTarget(Manager.Object.Character);
        if (DistanceTo(Manager.Object.Character) > 35 || IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x)
                transform.position += Vector3.right;
            if (Manager.Object.Character.position.x < transform.position.x)
                transform.position += Vector3.left;
        }

        yield break;
    }
    private IEnumerator Move_Snowlady()
    {
        if (_type != Enemys.SnowLady)
            yield break;

        if (IsContactGround) 
            transform.AddY(0.05f);
        else 
            transform.AddY(-1f);

        LookAtTheTarget(Manager.Object.Character);
        if (IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(0.5f);
            if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-0.5f);
        }
        else
        {
            if (DistanceTo(Manager.Object.Character) > 200f)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(0.5f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-0.5f);
            }
            if (DistanceTo(Manager.Object.Character) < 150f)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(-0.5f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(0.5f);
            }
        }
    }
    private IEnumerator Move_Dino()
    {
        if (_type != Enemys.BossDino)
            yield break;

        LookAtTheTarget(Manager.Object.Character);
        if (IsContactWall)
        {
            if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(1f);
            if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-1f);
        }
        else
        {
            if (DistanceTo(Manager.Object.Character) > 150f)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(1f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(-1f);
            }
            if (DistanceTo(Manager.Object.Character) < 175f)
            {
                if (Manager.Object.Character.position.x > transform.position.x) transform.AddX(-1f);
                if (Manager.Object.Character.position.x < transform.position.x) transform.AddX(1f);
            }
        }
    }

    private IEnumerator Loop_Attack()
    {
        while (true)
        {
            IEnumerator enumerator = null;
            switch (_type)
            {
                case Enemys.Shadow: enumerator = Attack_Shadow(); break;
                case Enemys.VoidCavity: enumerator = Attack_Voidcavity(); break;
                case Enemys.CrazyLaughMask: enumerator = Attack_Crazylaughmask(); break;
                case Enemys.MotherSpiritSnake: enumerator = Attack_Motherspiritsnake(); break;
                case Enemys.Bird: enumerator = Attack_Bird(); break;
                case Enemys.SadEyes: enumerator = Attack_Sadeyes(); break;
                case Enemys.ThePiedPiper: enumerator = Attack_Thepiedpiper(); break;
                case Enemys.Fire: enumerator = Attack_Fire(); break;
                case Enemys.Red: enumerator = Attack_Red(); break;
                case Enemys.SnowLady: enumerator = Attack_Snowlady(); break;
                case Enemys.BossDino: enumerator = Attack_Dino(); break;
            }
            yield return enumerator;
        }
    }
    private IEnumerator Attack_Shadow()
    {
        if (_type != Enemys.Shadow)
            yield break;

        if (IsContactCharacter == false) 
            yield break;

        Manager.Game.TakeDamageToPlayer(12);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Attack_Voidcavity()
    {
        if (_type != Enemys.VoidCavity)
            yield break;

        if (Math.Abs(transform.position.x - Manager.Object.Character.position.x) < 30 == false) 
            yield break;
        if (IsContactGround == false) 
            yield break; 

        for (int _ = 5; _ > 0; _--)
        {
            transform.position += Vector3.up * 5f;
            yield return new WaitForFixedUpdate();
        }
        if (IsContactCharacter) Manager.Game.TakeDamageToPlayer(9);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Attack_Crazylaughmask()
    {
        if (_type != Enemys.CrazyLaughMask)
            yield break;

        if (IsContactCharacter == false) 
            yield break;

        Manager.Game.TakeDamageToPlayer(13);
        for (int i = 10; i > 0; i--)
        {
            MoveToMoveDirection(-2f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Attack_Motherspiritsnake()
    {
        if (_type != Enemys.MotherSpiritSnake)
            yield break;

        if (DistanceTo(Manager.Object.Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return new WaitForSeconds(1f);
            _sr.SetBrightness(0f);
            CreatePoison();
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator Attack_Bird()
    {
        if (_type != Enemys.Bird)
            yield break;

        LookAtTheTarget(Manager.Object.Character);
        if (transform.position.y <= 50) 
            yield break;

        if (Utility.RandomNumber(1, 2) == 1)
        {
            _sr.SetBrightness(-0.75f);
            for (var timer = 1f; timer > 0; timer -= Time.fixedDeltaTime)
            {
                LookAtTheTarget(Manager.Object.Character);
                yield return new WaitForFixedUpdate();
            }
            _sr.SetBrightness(0);
            CreateWhirlwind();
        }

        else if (Math.Abs(Manager.Object.Character.position.x - transform.position.x) < 100) 
        {
            _sr.SetBrightness(0.75f);
            for (var timer = 1f; timer > 0; timer -= Time.fixedDeltaTime)
            {
                LookAtTheTarget(Manager.Object.Character);
                yield return new WaitForFixedUpdate();
            }
            _sr.SetBrightness(0f);
            foreach (int _ in Count(30))
            {
                MoveToMoveDirection(8f);
                if (IsContactCharacter)
                {
                    Manager.Game.TakeDamageToPlayer(12);
                    break;
                }
                if (IsContactGround || IsContactMoonlightswordShield) 
                    break;

                yield return new WaitForFixedUpdate();
            }
        }

        for (float timer = 2f; timer > 0; timer -= Time.fixedDeltaTime)
        {
            LookAtTheTarget(Manager.Object.Character);
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator Attack_Sadeyes()
    {
        if (_type != Enemys.SadEyes)
            yield break;

        if (!IsContactCharacter) 
            yield break;

        Manager.Game.TakeDamageToPlayer(12);
        for (int i = 10; i > 0; i--)
        {
            MoveToMoveDirection(-2f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator Attack_Thepiedpiper()
    {
        if (_type != Enemys.ThePiedPiper)
            yield break;

        if (DistanceTo(Manager.Object.Character) < 200)
        {
            Manager.Speech.Speech(transform, "♪");
            foreach (int i in Count(Utility.RandomNumber(3, 6)))
            {
                CallingRat();
                yield return PlayPipeSoundAndWaiting();
                yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
            }
            Manager.Speech.EraseSpeachbubble(transform);
            yield return new WaitForSeconds(5f);
        }
    }
    private IEnumerator Attack_Fire()
    {
        if (_type != Enemys.Fire)
            yield break;

        if (IsContactWaterPrison) 
            yield break;

        if (DistanceTo(Manager.Object.Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return new WaitForSeconds(1f);
            _sr.SetBrightness(0f);
            CreateFire();
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator Attack_Red()
    {
        if (_type != Enemys.Red)
            yield break;

        if (Mathf.Abs(transform.position.x - Manager.Object.Character.position.x) < 40)
        {
            if (IsContactGround)
            {
                for (int i = 5; i > 0; i--)
                {   
                    transform.Translate(Vector3.up * 5f);
                    yield return new WaitForFixedUpdate();
                }
                if (IsContactCharacter) Manager.Game.TakeDamageToPlayer(13);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    private IEnumerator Attack_Snowlady()
    {
        if (_type != Enemys.SnowLady)
            yield break;

        if (DistanceTo(Manager.Object.Character) < 250f)
        {
            _sr.SetBrightness(0.5f);
            yield return new WaitForSeconds(0.5f);
            _sr.SetBrightness(0);
            CreateIceShard();
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator Attack_Dino()
    {
        if (_type != Enemys.BossDino)
            yield break;

        if (DistanceTo(Manager.Object.Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return new WaitForSeconds(0.5f);
            _sr.SetBrightness(0);
            for (int _ = 3; _ > 0; _--)
            {
                CreateDinoProjectile();
                yield return new WaitForSeconds(0.01f);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
