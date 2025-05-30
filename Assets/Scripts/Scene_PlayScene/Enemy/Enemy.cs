using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utile;

public class Enemy : EnemyBase
{
    float _hp;

    Sprites.Enemys _type;

    protected override void DeleteThisClone()
    {
        Debug.Log($"삭제함: {_type.ToString()}");
        base.DeleteThisClone();
    }

    #region 야괴 이름 외치기
    string _hiddenName;
    void Init_HiddenNameLogic()
    {
        switch (_type)
        {
            case Sprites.Enemys.ThePiedPiper:
                _hiddenName = "하민우";
                break;
            case Sprites.Enemys.BossDino:
                _hiddenName = "공룡";
                break;

            default:
                _hiddenName = "";
                _hiddenName += Utile.RandomElement(hiddenSurnames);
                _hiddenName += Utile.RandomElement(hiddenMainames);
                break;
        }
        StartCoroutine(Loop_HiddenNameLogic());
    }
    void CreateNameParticles()
    {
        var loop = StartCoroutine(Loop_Create());
        StartCoroutine(StopParticle());
        IEnumerator StopParticle()
        {
            foreach (int i in Count(20))
            {
                yield return WaitForSeconds(0.05f);
                yield return waitForFixedUpdate;
            }
            StopCoroutine(loop);
        }
        IEnumerator Loop_Create()
        {
            while (true)
            {
                Create();
                yield return waitForFixedUpdate;
            }
        }
        void Create()
        {
            var prefab = Utile.LoadResource<GameObject>(Prefabs.EnemyHiddenNameParticle);
            var go = prefab.CreateClone();
            var particle = go.Component<EnemyHiddenNameParticle>();
            particle.Init(transform.position);
        }
    }
    IEnumerator Loop_HiddenNameLogic()
    {
        while (true)
        {
            if (IsPressedN)
            {
                yield return SpeechForSeconds(_hiddenName, 0.01f);
                continue;
            }

            if (shoutedEnemyName == _hiddenName)
            {
                Debug.Log("야괴 이름 적중 : " + _hiddenName + " : " + _type.ToString());
                yield return WaitForSeconds(0.25f);
                CreateNameParticles();
                if (_type == Sprites.Enemys.BossDino)
                {
                    yield return SpeechForSeconds("윽!", 0.75f);
                    yield return SpeechForSeconds("으아앗", 1f);
                    yield return SpeechForSeconds("이럴 줄 알았죠?", 1.5f);
                }

                else
                {
                    Speech("!");
                    Vector3 p = transform.position;
                    foreach (int i in Count(20))
                    {
                        transform.position = p;
                        yield return WaitForSeconds(0.05f);
                        yield return waitForFixedUpdate;
                    }

                    remainingWaveKill--;
                    if (_type == Sprites.Enemys.Shadow)
                        shadowState = ShadowState.Killed;

                    DeleteThisClone();
                }
            }

            yield return waitForFixedUpdate;
        }
    }
    #endregion

    public void Init(Sprites.Enemys type)
    {
        base.Init();
        _type = type;
        onNightmareEvent.Add(this, DeleteThisClone);
        Init_HpAndSprite();
        StartCoroutine(Loop_Move());
        StartCoroutine(Loop_Attack());
        StartCoroutine(Loop_Shadow_Logic());
        StartCoroutine(Loop_Flap_Bird());
        StartCoroutine(Loop_Jump_Thepiedpiper());
        StartCoroutine(Loop_Jump_Dino());

        Init_HiddenNameLogic();
    }

    void Init_HpAndSprite()
    {
        Sprite sprite = LoadResource<Sprite>(_type);
        gameObject.SetSpriteAndPolygon(sprite);
        float size = 0;
        switch (_type)
        {
            case Sprites.Enemys.Shadow: _hp = 100; break;
            case Sprites.Enemys.VoidCavity: _hp = 12; size = 31.4f; break;
            case Sprites.Enemys.CrazyLaughMask: _hp = 18; size = 31.9f; break;
            case Sprites.Enemys.MotherSpiritSnake: _hp = 23; size = 43.2f; break;
            case Sprites.Enemys.Bird: _hp = 20; size = 31.0f; break;
            case Sprites.Enemys.SadEyes: _hp = 20; size = 31.4f; break;
            case Sprites.Enemys.ThePiedPiper: _hp = 18; size = 36.6f; break;
            case Sprites.Enemys.Fire: _hp = 23; size = 40.5f; break;
            case Sprites.Enemys.Red: _hp = 25; size = 54.9f; break;
            case Sprites.Enemys.SnowLady: _hp = 23; size = 50.0f; break;
            case Sprites.Enemys.BossDino: _hp = 100; size = 76.8f; break;
        }
        transform.localScale = Vector3.one * size;
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        // 영도는 일반적인 공격에 효과 없음
        if (_type == Sprites.Enemys.Shadow)
            yield break;

        _hp -= damage;
        if (_hp < 0) 
        {
            remainingWaveKill -= 1;

            if (_type == Sprites.Enemys.BossDino) 
                isBossDinoKilled = true;

            DeleteThisClone();
        }
        yield return SpeechForSeconds(_hp.ToString(), 0.1f);
    }

    void CreatePoison()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemyProjectile_Poison);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Poison>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    void CreateWhirlwind()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemyProjectile_Whirlwind);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Whirlwind>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    void CallingRat()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemySkill_Rat);
        var go = Instantiate(prefab);
        var rat = go.GetComponent<EnemySkill_Rat>();
        rat.OnCreate();
    }
    IEnumerator PlayPipeSoundAndWaiting()
    {
        var clip = LoadResource<AudioClip>(Audios.Pipe);
        _audioSource?.PlayOneShot(clip);
        yield return WaitForSeconds(clip.length);
    }
    void CreateFire()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemyProjectile_Fire);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_Fire>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    void CreateIceShard()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemyProjectile_IceShard);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_IceShard>();
        projectile.OnCreate(transform.position, _moveDirection);
    }
    void CreateDinoProjectile()
    {
        var prefab = LoadResource<GameObject>(Prefabs.EnemyProjectile_DinoProjectile);
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<EnemyProjectile_DinoProjectile>();
        projectile.OnCreate(transform.position, _moveDirection);
    }

    IEnumerator Loop_Shadow_Logic()
    {
        // 영도가 아닐 시 바로 종료
        if (_type != Sprites.Enemys.Shadow)
            yield break;

        // 체력 변화: 100~0 
        // 크기 변화: 200~25 = 25+(175~0)

        // 최소 크기 25
        // hp변화당 1.75크기변화

        // 기존 카메라 타격 횟수: 100/0.5 = 200회

        float timer = -5f;
        float giantization = 0;

        const float Interval = 1f;

        while (true)
        {
            // 거대화
            if (giantization < 1f)
            {
                timer += FixedDeltaTime;

                if (timer > 0f)
                    _sr.SetBrightness(-0.75f * (timer / Interval));
            
                if (timer >= Interval)
                {
                    timer = 0;
                    giantization += 0.25f;
                }
            }
            else
            {
                giantization = 1f;
                shadowState = ShadowState.EndOfGiantization;
                _sr.SetBrightness(0f);
            }

            // 크기 업데이트
            transform.localScale = Vector3.one * (25f + giantization * _hp * 1.75f);

            // 카메라 빛에 피해 받기
            if (IsContactCameraLight)
            {
                _hp -= 0.5f;

                // 죽음
                if (_hp <= 0)
                {
                    shadowState = ShadowState.Killed;
                    DeleteThisClone();
                }
            }

            // 특수기술 캔슬링
            if (IsContactMoonlightswordShield ||
                IsContactBossDinoSkill ||
                IsContactWaterPrison) onDisarmSpecialSkill.Call();

            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_Flap_Bird()
    {
        if (_type != Sprites.Enemys.Bird)
            yield break;

        while (true)
        {
            if (transform.position.y > 60f)
            {
                for (int _ = 15; _ > 0; _--)
                {
                    transform.AddY(-0.5f);
                    yield return waitForFixedUpdate;
                }
            }
            for (int _ = 5; _ > 0; _--)
            {
                transform.AddY(1.5f);
                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_Jump_Thepiedpiper()
    {
        if (_type != Sprites.Enemys.ThePiedPiper)
            yield break;

        while (true)
        {
            if (IsContactWall || 
                DistanceTo(Character) > 200 ||
                DistanceTo(Character) < 150)
            {
                for (int _ = 10; _ > 0; _--)
                {
                    transform.AddY(4f);
                    yield return waitForFixedUpdate;
                }
                yield return WaitUntil(() => IsContactGround);
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Loop_Jump_Dino()
    {
        if (_type != Sprites.Enemys.BossDino)
            yield break;

        while (true)
        {
            if (IsContactGround)
            {
                for (int _ = 40; _ > 0; _--)
                {
                    transform.AddY(3f);
                    yield return waitForFixedUpdate;
                }
            }
            else
            {
                transform.AddY(-3f);
                yield return waitForFixedUpdate;
            }
        }
    }

    IEnumerator Loop_Move()
    {
        while (true)
        {
            IEnumerator enumerator = null;
            switch (_type)
            {
                case Sprites.Enemys.Shadow: enumerator = Move_Shadow_And_Voidcavity(); break;
                case Sprites.Enemys.VoidCavity: enumerator = Move_Shadow_And_Voidcavity(); break;
                case Sprites.Enemys.CrazyLaughMask: enumerator = Move_Crazylaughmask_And_Sadeyes(); break;
                case Sprites.Enemys.MotherSpiritSnake: enumerator = Move_Motherspiritsnake(); break;
                case Sprites.Enemys.Bird: enumerator = Move_Bird(); break;
                case Sprites.Enemys.SadEyes: enumerator = Move_Crazylaughmask_And_Sadeyes(); break;
                case Sprites.Enemys.ThePiedPiper: enumerator = Move_Thepiedpiper(); break;
                case Sprites.Enemys.Fire: enumerator = Move_Fire(); break;
                case Sprites.Enemys.Red: enumerator = Move_Red(); break;
                case Sprites.Enemys.SnowLady: enumerator = Move_Snowlady(); break;
                case Sprites.Enemys.BossDino: enumerator = Move_Dino(); break;
            }
            yield return enumerator;
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Move_Shadow_And_Voidcavity()
    {
        if (_type != Sprites.Enemys.Shadow && _type != Sprites.Enemys.VoidCavity)
            yield break;

        if (IsContactGround) transform.position += Vector3.up * 0.1f;
        else transform.position += Vector3.up * -2f;
        LookAtTheTarget(Character);
        if (DistanceTo(Character) > 25 || IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.position += Vector3.right * 0.5f;
            if (Character.position.x < transform.position.x) transform.position += Vector3.right * -0.5f;
        }
    }
    IEnumerator Move_Crazylaughmask_And_Sadeyes()
    {
        if (_type != Sprites.Enemys.CrazyLaughMask && _type != Sprites.Enemys.SadEyes)
            yield break;

        LookAtTheTarget(Character);
        MoveToMoveDirection(1f);
        if (_type == Sprites.Enemys.CrazyLaughMask)
        {
            if (RandomNumber(1, 500) == 1)
                yield return SpeechForSeconds("ㅋ흐하하하하핳ㅋ흫흐하핳", 1f);
        }
        if (DistanceTo(Character) < 50f)
        {
            for (int _ = 15; _ > 0; _--)
            {
                MoveToMoveDirection(3f);
                if (IsContactCharacter) break;
                yield return waitForFixedUpdate;
            }
            yield return WaitForSeconds(0.5f);
        }

        yield break;
    }
    IEnumerator Move_Motherspiritsnake()
    {
        if (_type != Sprites.Enemys.MotherSpiritSnake)
            yield break;

        if (IsContactGround) transform.position += Vector3.up * 0.1f;
        else transform.position += Vector3.up * -2;
        LookAtTheTarget(Character);
        if (DistanceTo(Character) > 100 || IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.position += Vector3.right * 1f;
            if (Character.position.x < transform.position.x) transform.position += Vector3.right * -1f;
        }

        yield break;
    }
    IEnumerator Move_Bird()
    {
        if (_type != Sprites.Enemys.Bird)
            yield break;

        if (Math.Abs(Character.position.x - transform.position.x) > 50)
        {
            if (Character.position.x > transform.position.x) transform.position += Vector3.right * 1f;
            if (Character.position.x < transform.position.x) transform.position += Vector3.right * -1f;
        }

        yield break;
    }
    IEnumerator Move_Thepiedpiper()
    {
        if (_type != Sprites.Enemys.ThePiedPiper)
            yield break;

        if (IsContactGround) 
            transform.AddY(0.1f);
        else 
            transform.AddY(-2f);

        LookAtTheTarget(Character);
        if (IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.AddX(0.5f);
            if (Character.position.x < transform.position.x) transform.AddX(-0.5f);
        }
        else
        {
            if (DistanceTo(Character) > 200)
            {
                if (Character.position.x > transform.position.x) transform.AddX(0.5f);
                if (Character.position.x < transform.position.x) transform.AddX(-0.5f);
            }
            if (DistanceTo(Character) < 150)
            {
                if (Character.position.x > transform.position.x) transform.AddX(-0.5f);
                if (Character.position.x < transform.position.x) transform.AddX(0.5f);
            }
        }
    }
    IEnumerator Move_Fire()
    {
        if (_type != Sprites.Enemys.Fire)
            yield break;

        if (IsContactGround)
            transform.AddY(0.1f);
        else
            transform.AddY(-2f);

        LookAtTheTarget(Character);
        if (DistanceTo(Character) > 150 || IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.AddX(1.2f);
            if (Character.position.x < transform.position.x) transform.AddX(-1.2f);
        }
    }
    IEnumerator Move_Red()
    {
        if (_type != Sprites.Enemys.Red)
            yield break;

        if (IsContactGround)
            transform.position += Vector3.up * 0.1f;
        else
            transform.position += Vector3.up * -2f;
        LookAtTheTarget(Character);
        if (DistanceTo(Character) > 35 || IsContactWall)
        {
            if (Character.position.x > transform.position.x)
                transform.position += Vector3.right;
            if (Character.position.x < transform.position.x)
                transform.position += Vector3.left;
        }

        yield break;
    }
    IEnumerator Move_Snowlady()
    {
        if (_type != Sprites.Enemys.SnowLady)
            yield break;

        if (IsContactGround) 
            transform.AddY(0.05f);
        else 
            transform.AddY(-1f);

        LookAtTheTarget(Character);
        if (IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.AddX(0.5f);
            if (Character.position.x < transform.position.x) transform.AddX(-0.5f);
        }
        else
        {
            if (DistanceTo(Character) > 200f)
            {
                if (Character.position.x > transform.position.x) transform.AddX(0.5f);
                if (Character.position.x < transform.position.x) transform.AddX(-0.5f);
            }
            if (DistanceTo(Character) < 150f)
            {
                if (Character.position.x > transform.position.x) transform.AddX(-0.5f);
                if (Character.position.x < transform.position.x) transform.AddX(0.5f);
            }
        }
    }
    IEnumerator Move_Dino()
    {
        if (_type != Sprites.Enemys.BossDino)
            yield break;

        LookAtTheTarget(Character);
        if (IsContactWall)
        {
            if (Character.position.x > transform.position.x) transform.AddX(1f);
            if (Character.position.x < transform.position.x) transform.AddX(-1f);
        }
        else
        {
            if (DistanceTo(Character) > 150f)
            {
                if (Character.position.x > transform.position.x) transform.AddX(1f);
                if (Character.position.x < transform.position.x) transform.AddX(-1f);
            }
            if (DistanceTo(Character) < 175f)
            {
                if (Character.position.x > transform.position.x) transform.AddX(-1f);
                if (Character.position.x < transform.position.x) transform.AddX(1f);
            }
        }
    }

    IEnumerator Loop_Attack()
    {
        while (true)
        {
            IEnumerator enumerator = null;
            switch (_type)
            {
                case Sprites.Enemys.Shadow: enumerator = Attack_Shadow(); break;
                case Sprites.Enemys.VoidCavity: enumerator = Attack_Voidcavity(); break;
                case Sprites.Enemys.CrazyLaughMask: enumerator = Attack_Crazylaughmask(); break;
                case Sprites.Enemys.MotherSpiritSnake: enumerator = Attack_Motherspiritsnake(); break;
                case Sprites.Enemys.Bird: enumerator = Attack_Bird(); break;
                case Sprites.Enemys.SadEyes: enumerator = Attack_Sadeyes(); break;
                case Sprites.Enemys.ThePiedPiper: enumerator = Attack_Thepiedpiper(); break;
                case Sprites.Enemys.Fire: enumerator = Attack_Fire(); break;
                case Sprites.Enemys.Red: enumerator = Attack_Red(); break;
                case Sprites.Enemys.SnowLady: enumerator = Attack_Snowlady(); break;
                case Sprites.Enemys.BossDino: enumerator = Attack_Dino(); break;
            }
            yield return enumerator;
        }
    }
    IEnumerator Attack_Shadow()
    {
        if (_type != Sprites.Enemys.Shadow)
            yield break;

        if (IsContactCharacter == false) 
            yield break;

        TakeDamageToPlayer(12);
        yield return WaitForSeconds(0.5f);
    }
    IEnumerator Attack_Voidcavity()
    {
        if (_type != Sprites.Enemys.VoidCavity)
            yield break;

        if (Math.Abs(transform.position.x - Character.position.x) < 30 == false) 
            yield break;
        if (IsContactGround == false) 
            yield break; 

        for (int _ = 5; _ > 0; _--)
        {
            transform.position += Vector3.up * 5f;
            yield return waitForFixedUpdate;
        }
        if (IsContactCharacter) TakeDamageToPlayer(9);
        yield return WaitForSeconds(0.5f);
    }
    IEnumerator Attack_Crazylaughmask()
    {
        if (_type != Sprites.Enemys.CrazyLaughMask)
            yield break;

        if (IsContactCharacter == false) 
            yield break;

        TakeDamageToPlayer(13);
        for (int i = 10; i > 0; i--)
        {
            MoveToMoveDirection(-2f);
            yield return waitForFixedUpdate;
        }
        yield return WaitForSeconds(0.5f);
    }
    IEnumerator Attack_Motherspiritsnake()
    {
        if (_type != Sprites.Enemys.MotherSpiritSnake)
            yield break;

        if (DistanceTo(Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return WaitForSeconds(1f);
            _sr.SetBrightness(0f);
            CreatePoison();
            yield return WaitForSeconds(2f);
        }
    }
    IEnumerator Attack_Bird()
    {
        if (_type != Sprites.Enemys.Bird)
            yield break;

        LookAtTheTarget(Character);
        if (transform.position.y <= 50) 
            yield break;

        if (RandomNumber(1, 2) == 1)
        {
            _sr.SetBrightness(-0.75f);
            for (var timer = 1f; timer > 0; timer -= Time.fixedDeltaTime)
            {
                LookAtTheTarget(Character);
                yield return waitForFixedUpdate;
            }
            _sr.SetBrightness(0);
            CreateWhirlwind();
        }

        else if (Math.Abs(Character.position.x - transform.position.x) < 100) 
        {
            _sr.SetBrightness(0.75f);
            for (var timer = 1f; timer > 0; timer -= Time.fixedDeltaTime)
            {
                LookAtTheTarget(Character);
                yield return waitForFixedUpdate;
            }
            _sr.SetBrightness(0f);
            foreach (int _ in Count(30))
            {
                MoveToMoveDirection(8f);
                if (IsContactCharacter)
                {
                    TakeDamageToPlayer(12);
                    break;
                }
                if (IsContactGround || IsContactMoonlightswordShield) 
                    break;

                yield return waitForFixedUpdate;
            }
        }

        for (float timer = 2f; timer > 0; timer -= Time.fixedDeltaTime)
        {
            LookAtTheTarget(Character);
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator Attack_Sadeyes()
    {
        if (_type != Sprites.Enemys.SadEyes)
            yield break;

        if (!IsContactCharacter) 
            yield break;

        TakeDamageToPlayer(12);
        for (int i = 10; i > 0; i--)
        {
            MoveToMoveDirection(-2f);
            yield return waitForFixedUpdate;
        }
        yield return WaitForSeconds(0.5f);
    }
    IEnumerator Attack_Thepiedpiper()
    {
        if (_type != Sprites.Enemys.ThePiedPiper)
            yield break;

        if (DistanceTo(Character) < 200)
        {
            Speech("♪");
            foreach (int i in Count(RandomNumber(3, 6)))
            {
                CallingRat();
                yield return PlayPipeSoundAndWaiting();
                yield return WaitForSeconds(0.1f);
                yield return waitForFixedUpdate;
            }
            EraseSpeachbubble();
            yield return WaitForSeconds(5f);
        }
    }
    IEnumerator Attack_Fire()
    {
        if (_type != Sprites.Enemys.Fire)
            yield break;

        if (IsContactWaterPrison) 
            yield break;

        if (DistanceTo(Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return WaitForSeconds(1f);
            _sr.SetBrightness(0f);
            CreateFire();
            yield return WaitForSeconds(2f);
        }
    }
    IEnumerator Attack_Red()
    {
        if (_type != Sprites.Enemys.Red)
            yield break;

        if (Mathf.Abs(transform.position.x - Character.position.x) < 40)
        {
            if (IsContactGround)
            {
                for (int i = 5; i > 0; i--)
                {   
                    transform.Translate(Vector3.up * 5f);
                    yield return waitForFixedUpdate;
                }
                if (IsContactCharacter) TakeDamageToPlayer(13);
                yield return WaitForSeconds(0.5f);
            }
        }
    }
    IEnumerator Attack_Snowlady()
    {
        if (_type != Sprites.Enemys.SnowLady)
            yield break;

        if (DistanceTo(Character) < 250f)
        {
            _sr.SetBrightness(0.5f);
            yield return WaitForSeconds(0.5f);
            _sr.SetBrightness(0);
            CreateIceShard();
            yield return WaitForSeconds(1f);
        }
    }
    IEnumerator Attack_Dino()
    {
        if (_type != Sprites.Enemys.BossDino)
            yield break;

        if (DistanceTo(Character) < 250f)
        {
            _sr.SetBrightness(-0.5f);
            yield return WaitForSeconds(0.5f);
            _sr.SetBrightness(0);
            for (int _ = 3; _ > 0; _--)
            {
                CreateDinoProjectile();
                yield return WaitForSeconds(0.01f);
                yield return waitForFixedUpdate;
            }
            yield return WaitForSeconds(2f);
        }
    }
}
