using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingScene_Character : EndingSceneObjectBase
{
    Image _image;

    Sprites.Characters _character;

    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _image = GetComponent<Image>();

        _character = Enum.Parse<Sprites.Characters>(_image.sprite.name);

        Character[_character] = transform;
        IEnumerator startRoutine = null;
        switch (_character)
        {
            case Sprites.Characters.Sleepground:
                startRoutine = Start_Sleepground();
                break;
            case Sprites.Characters.Dino:
                startRoutine = Start_Dino();
                break;
            case Sprites.Characters.Heptagram:
                startRoutine = Start_Heptagram();
                break;
            case Sprites.Characters.Rather:
                startRoutine = Start_Rather();
                break;
        }
        StartCoroutine(startRoutine);
    }

    void CreatePurpleParticle()
    {
        var prefab = Utility.LoadResource<GameObject>(Prefabs.Scene_EndingScene.Ending_Particle);
        var go = prefab.CreateClone();
        go.transform.SetParent(transform.parent);
        var particle = go.Component<EndingScene_Particle>();
        particle.Init(Sprites.Ending.Particle_Purple);
    }

    void CreateYellowParticle()
    {
        var prefab = Utility.LoadResource<GameObject>(Prefabs.Scene_EndingScene.Ending_Particle);
        var go = prefab.CreateClone();
        go.transform.SetParent(transform.parent);
        var particle = go.Component<EndingScene_Particle>();
        particle.Init(Sprites.Ending.Particle_Yellow);
    }

    IEnumerator Start_Sleepground()
    {
        yield return WaitUntil(() => EndingProgress == 20);

        Transform heptagram = Character[Sprites.Characters.Heptagram];
        yield return transform.MoveToTransformOverTime(0.2f, heptagram);

        yield return transform.MoveOverTime(0.3f, new Vector3(-200f, 100f));

        EndingProgress++;

        yield return transform.MoveOverTime(0.2f, new Vector3(-100f, -50f));

        EndingProgress++;
    }

    IEnumerator Start_Dino()
    {
        yield return WaitUntil(() => EndingProgress == 4);
        
        foreach (int i in Count(25))
        {
            CreatePurpleParticle();
            yield return waitForFixedUpdate;
        }
        
        yield return WaitForSeconds(0.5f);
        
        _image.enabled = false;

        Transform sleepground = Character[Sprites.Characters.Sleepground];
        yield return transform.MoveToTransformOverTime(0.2f, sleepground);
        transform.AddY(25f);
        
        EndingProgress++;

        while (EndingProgress < 21)
        {
            yield return transform.MoveToTransformOverTime(0.2f, sleepground);
            transform.AddY(25f);
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Start_Heptagram()
    {
        yield return WaitUntil(() => EndingProgress == 19);

        _image.enabled = false;

        foreach (int i in Count(25))
        {
            CreateYellowParticle();
            yield return waitForFixedUpdate;
        }

        yield return WaitForSeconds(0.5f);

        EndingProgress++;
    }

    IEnumerator Start_Rather()
    {
        yield return WaitUntil(() => EndingProgress == 22);

        yield return WaitForSeconds(0.5f);

        yield return transform.MoveOverTime(0.5f, new Vector3(-125f, 10f));

        yield return WaitForSeconds(0.75f);

        EndingProgress++;

        _image.enabled = false;
    }
}
