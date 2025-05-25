using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_BossDinoSkill : PlaySceneObjectBase
{
    LineRenderer _line;
    GameObject _child;
    SpriteRenderer _sr;

    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _line = gameObject.Component<LineRenderer>();
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();

        StartCoroutine(Loop_CreateClone());
        StartCoroutine(Loop_DrawLine());
        StartCoroutine(Loop_Brightness());
        StartCoroutine(Loop_LineColor());
    }

    void CreateClone()
    {
        GameObject go = _child.CreateClone();
        Transform transform = go.transform;
        SpriteRenderer sr = go.Component<SpriteRenderer>();

        sr.sprite = Utile.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_BossDinoSkill_Red);
        transform.localScale = Vector3.one * 3f;
        sr.SetAlpha(0.5f);
        sr.SetBrightness(0f);

        transform.position = BossDinoBlackBall.position;
        Vector3 direction = (Utile.MousePosition - transform.position).normalized;
        float randomDistance = Utile.RandomNumber(1, 250);
        transform.position += direction * randomDistance;


        List<Coroutine> coroutines = new();
        coroutines.Add(StartCoroutine(Loop_Effect()));
        coroutines.Add(StartCoroutine(WaitAndDestroy()));

        void DestroyThisClone()
        {
            foreach (Coroutine coroutine in coroutines) 
                StopCoroutine(coroutine);

            Destroy(go);
        }

        IEnumerator WaitAndDestroy()
        {
            yield return WaitForSeconds(0.1f);
            DestroyThisClone();
        }

        IEnumerator Loop_Effect()
        {
            while (true)
            {
                foreach (int i in Count(10))
                {
                    sr?.AddBrightness(0.02f);
                    transform.localScale += Vector3.one * 0.3f;
                    yield return waitForFixedUpdate;
                }
                foreach (int i in Count(10))
                {
                    sr?.AddBrightness(-0.02f);
                    transform.localScale += Vector3.one * -0.3f;
                    yield return waitForFixedUpdate;
                }
                yield return waitForFixedUpdate;
            }
        }
    }

    IEnumerator Loop_CreateClone()
    {
        while (true)
        {
            bool characterCheck = currentCharacter == Sprites.Characters.Dino;
            if (characterCheck && isSpecialSkillInvoking)
            {
                CreateClone();
            }
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_DrawLine()
    {
        while (true)
        {
            bool characterCheck = currentCharacter == Sprites.Characters.Dino;
            if (characterCheck && isSpecialSkillInvoking && IsMouseClicked)
            {
                _child.SetActive(true);
                _line.positionCount = 2;
                transform.position = BossDinoBlackBall.position;
                Vector3 direction = (Utile.MousePosition - transform.position).normalized;
                _line.SetPosition(0, transform.position);
                transform.position += direction * 250f;
                _line.SetPosition(1, transform.position);

            }
            else
            {
                _child.SetActive(false);
                _line.positionCount = 0;
            }
            
            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Brightness()
    {
        _sr.SetTransparency(0.5f);
        _sr.SetBrightness(0f);
        while (true)
        {
            foreach (int i in Count(30))
            {
                _sr.AddBrightness(0.01f);
                yield return waitForFixedUpdate;
            }
            foreach (int i in Count(30))
            {
                _sr.AddBrightness(-0.01f);
                yield return waitForFixedUpdate;
            }
        }
    }

    IEnumerator Loop_LineColor()
    {
        _line.startWidth = 1f;
        _line.endWidth = 1f;
        while (true)
        {
            Color color;
            color = Utile.StringToColor("#ff0000");
            color.a = 0.5f;
            _line.startColor = color;
            _line.endColor = color;
            yield return WaitForSeconds(0.1f);

            color = Utile.StringToColor("#ff4040");
            color.a = 0.5f;
            _line.startColor = color;
            _line.endColor = color;
            yield return WaitForSeconds(0.1f);
            yield return waitForFixedUpdate;
        }
    }
}
