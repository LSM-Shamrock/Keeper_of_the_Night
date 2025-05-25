using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingScene_Particle : EndingSceneObjectBase
{
    Image _image;

    Sprites.Ending _type;

    public void Init(Sprites.Ending type)
    {
        _image = GetComponent<Image>();
        _image.sprite = Utile.LoadResource<Sprite>(type);

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
            float brightness = Utile.RandomNumber(-0.25f, 0.1f);
      
            Transform dino = Character[Sprites.Characters.Dino];

            transform.position = dino.position;
            transform.AddX(Utile.RandomNumber(-50, 50));
            transform.AddY(Utile.RandomNumber(-50, 50));
            
            yield return transform.MoveToTransformOverTime(0.5f, dino);
            
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Yellow()
    {
        while (true)
        {
            float brightness = Utile.RandomNumber(0.1f, 0.25f);

            Transform to;
            if (EndingProgress > 20)
                to = Character[Sprites.Characters.Sleepground];
            else 
                to = Character[Sprites.Characters.Heptagram];

            transform.position = to.position;

            Vector3 v = Vector3.zero;
            v.x = Utile.RandomNumber(-25, 25);
            v.y = Utile.RandomNumber(-25, 25);
            yield return transform.MoveToPositionOverTime(0.5f, to.position + v);
            
            yield return waitForFixedUpdate;
        }
    }
}
