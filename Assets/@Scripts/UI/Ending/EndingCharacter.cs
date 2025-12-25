using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingCharacter : EndingBase
{
    Image _image;

    Characters _character;


    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _image = GetComponent<Image>();

        _character = Enum.Parse<Characters>(_image.sprite.name);

        Character[_character] = transform;
        IEnumerator coroutine = null;
        switch (_character)
        {
            case Characters.Sleepground: coroutine = StartSleepground(); break;
            case Characters.Dino: coroutine = StartDino(); break;
            case Characters.Heptagram: coroutine = StartHeptagram(); break;
            case Characters.Rather: coroutine = StartRather(); break;
        }
        StartCoroutine(coroutine);
    }

    void CreatePurpleParticle()
    {
        GameObject prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Scene_Ending.Ending_Particle);
        GameObject go = prefab.CreateClone();
        go.transform.SetParent(ParticleRoot);
        EndingParticle particle = go.Component<EndingParticle>();
        particle.Init(Sprites.Ending.Particle_Purple);
    }

    void CreateYellowParticle()
    {
        GameObject prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Scene_Ending.Ending_Particle);
        GameObject go = prefab.CreateClone();
        go.transform.SetParent(ParticleRoot);
        EndingParticle particle = go.Component<EndingParticle>();
        particle.Init(Sprites.Ending.Particle_Yellow);
    }

    IEnumerator StartSleepground()
    {
        yield return new WaitUntil(() => EndingProgress == 20);

        Transform heptagram = Character[Characters.Heptagram];
        yield return transform.MoveToTransformOverTime(0.2f, heptagram);

        yield return transform.MoveOverTime(0.3f, new Vector3(-200f, 100f));

        EndingProgress++;

        yield return transform.MoveOverTime(0.2f, new Vector3(-100f, -50f));

        EndingProgress++;
    }

    IEnumerator StartDino()
    {
        yield return new WaitUntil(() => EndingProgress == 4);
        
        foreach (int i in Count(25))
        {
            CreatePurpleParticle();
            yield return new WaitForFixedUpdate();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        _image.enabled = false;

        Transform sleepground = Character[Characters.Sleepground];
        yield return transform.MoveToTransformOverTime(0.2f, sleepground);
        transform.AddY(25f);
        
        EndingProgress++;

        while (EndingProgress < 21)
        {
            transform.position = sleepground.position + Vector3.up * 25f;
            yield return null;
        }
    }

    IEnumerator StartHeptagram()
    {
        yield return new WaitUntil(() => EndingProgress == 19);

        _image.enabled = false;

        foreach (int i in Count(25))
        {
            CreateYellowParticle();
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        EndingProgress++;
    }

    IEnumerator StartRather()
    {
        yield return new WaitUntil(() => EndingProgress == 22);

        yield return new WaitForSeconds(0.5f);

        yield return transform.MoveOverTime(0.5f, new Vector3(-125f, 10f));

        yield return new WaitForSeconds(0.75f);

        EndingProgress++;

        _image.enabled = false;
    }
}
