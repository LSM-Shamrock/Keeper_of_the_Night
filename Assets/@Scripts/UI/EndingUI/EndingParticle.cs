using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingParticle : EndingBase
{
    Image _image;

    Sprites.Ending _type;

    public void Init(Sprites.Ending type)
    {
        _image = GetComponent<Image>();
        _image.sprite = Utility.LoadResource<Sprite>(type);

        _type = type;

        IEnumerator typeLogic = null;
        switch (_type)
        {
            case Sprites.Ending.Particle_Purple:
                typeLogic = Loop_Purple();
                break;
            case Sprites.Ending.Particle_Yellow:
                typeLogic= Loop_Yellow();
                break;
        }
        StartCoroutine(typeLogic);
        StartCoroutine(Start_FadeoutAndDestroy());
    }

    IEnumerator Start_FadeoutAndDestroy()
    {
        yield return WaitUntil(() => EndingProgress == 21);
        foreach (int i in Count(10))
        {
            _image.AddTransparency(0.1f);
            yield return waitForFixedUpdate;
        }
        Destroy(gameObject);
    }

    IEnumerator Loop_Purple()
    {
        while (true)
        {
            float brightness = Utility.RandomNumber(-0.25f, 0.1f);
            _image.SetBrightness(brightness);
      
            Transform dino = Character[Sprites.Characters.Dino];

            transform.position = dino.position;
            transform.AddX(Utility.RandomNumber(-50, 50));
            transform.AddY(Utility.RandomNumber(-50, 50));
            
            yield return transform.MoveToTransformOverTime(0.5f, dino);
            
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Yellow()
    {
        while (true)
        {
            float brightness = Utility.RandomNumber(0.1f, 0.25f);
            _image.SetBrightness(brightness);

            Transform target;
            if (EndingProgress > 20)
                target = Character[Sprites.Characters.Sleepground];
            else 
                target = Character[Sprites.Characters.Heptagram];

            transform.position = target.position;

            Vector3 v = new Vector3(Utility.RandomNumber(-25, 25), 50);
            yield return transform.MoveToPositionOverTime(0.5f, target.position + v);
            
            yield return waitForFixedUpdate;
        }
    }
}
