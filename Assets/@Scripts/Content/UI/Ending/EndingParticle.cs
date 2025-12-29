using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingParticle : UIBase
{
    private Image _image;
    private Dictionary<Characters, Transform> _characters;
    private int _endingProgress;

    public void Init(Sprites.Ending type, Dictionary<Characters, Transform> characters)
    {
        _image = GetComponent<Image>();
        _image.sprite = Manager.Resource.LoadResource<Sprite>(type);

        _characters = characters;

        IEnumerator typeLogic = null;
        switch (type)
        {
            case Sprites.Ending.Particle_Purple: typeLogic = LoopPurple(); break;
            case Sprites.Ending.Particle_Yellow: typeLogic= LoopYellow(); break;
        }
        StartCoroutine(typeLogic);
        StartCoroutine(StartFadeoutAndDestroy());
    }

    public void OnEndingProgressChange(int endingProgress)
    {
        _endingProgress = endingProgress;
    }

    private IEnumerator StartFadeoutAndDestroy()
    {
        yield return new WaitUntil(() => _endingProgress == 21);

        foreach (int i in Util.Count(10))
        {
            _image.AddTransparency(0.1f);
            yield return new WaitForFixedUpdate();
        }
        Manager.Object.Despawn(gameObject);
    }

    private IEnumerator LoopPurple()
    {
        while (true)
        {
            float brightness = Util.RandomNumber(-0.25f, 0.1f);
            _image.SetBrightness(brightness);
      
            Transform dino = _characters[Characters.Dino];

            transform.position = dino.position;
            transform.AddX(Util.RandomNumber(-50, 50));
            transform.AddY(Util.RandomNumber(-50, 50));
            
            yield return transform.MoveToTransformOverTime(0.5f, dino);
            
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator LoopYellow()
    {
        while (true)
        {
            float brightness = Util.RandomNumber(0.1f, 0.25f);
            _image.SetBrightness(brightness);

            Transform target;
            if (_endingProgress > 20)
                target = _characters[Characters.Sleepground];
            else 
                target = _characters[Characters.Heptagram];

            transform.position = target.position;

            Vector3 v = new Vector3(Util.RandomNumber(-25, 25), 50);
            yield return transform.MoveToPositionOverTime(0.5f, target.position + v);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
