using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill_BossDino : BaseController
{
    private LineRenderer _lineRenderer;
    private GameObject _body;
    private SpriteRenderer _ballStart;
    private SpriteRenderer _ballEnd;

    protected override void Start()
    {
        Init();
    }

    protected override void Update()
    {
        UpdateBoddy();
        UpdateDrawLine();
    }

    private void Init()
    {
        _lineRenderer = gameObject.Component<LineRenderer>();
        _body = transform.GetChild(0).gameObject;
        _ballStart = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _ballEnd = transform.GetChild(2).GetComponent<SpriteRenderer>();

        Manager.Game.onDisarmSpecialSkill.Add(this, () =>
        {
            if (Manager.Game.selectedCharacter == Characters.Dino)
                Manager.Game.isSpecialSkillInvoking = false;
        });

        StartCoroutine(LoopRoutine());
        StartCoroutine(LoopBallEftect());

        StartCoroutine(LoopCreateParticle());
        StartCoroutine(LoopLineEffect());
    }

    private void UpdateBoddy()
    {
        bool b = Manager.Game.selectedCharacter == Characters.Dino && Manager.Game.isSpecialSkillInvoking;
        _body.SetActive(b);
        _ballStart.gameObject.SetActive(b);

        transform.position = Manager.Game.Character.position;

        bool flip = Manager.Game.characterMoveDirection.x < 0;
        Vector3 scale = transform.localScale;
        scale.x = flip ? -1 : 1;
        transform.localScale = scale;
    }
    private void UpdateDrawLine()
    {
        bool characterCheck = Manager.Game.currentCharacter == Characters.Dino;
        if (characterCheck && Manager.Game.isSpecialSkillInvoking && Manager.Input.isPressedAttack)
        {
            _ballEnd.gameObject.SetActive(true);

            Vector3 startPos = _ballStart.transform.position;
            Vector3 endPos = startPos + Manager.Input.attackDirection * 250f;
            _ballEnd.transform.position = endPos;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }
        else
        {
            _ballEnd.gameObject.SetActive(false);
            _lineRenderer.positionCount = 0;
        }
    }

    private IEnumerator LoopRoutine()
    {
        if (Manager.Game.selectedCharacter == Characters.Dino)
            Manager.Game.specialSkillCooltime = 35f;

        while (true)
        {
            yield return new WaitUntil(() => Manager.Game.currentCharacter == Characters.Dino);

            if (Manager.Game.specialSkillCooltime <= 0 && Manager.Input.isPressedS)
            {
                Manager.Game.specialSkillCooltime = 45f;
                Manager.Game.isSpecialSkillInvoking = true;

                yield return new WaitForSeconds(10f);

                Manager.Game.onDisarmSpecialSkill.Call();
            }
        }
    }

    private IEnumerator LoopBallEftect()
    {
        while (true)
        {
            foreach (int i in Count(30))
            {
                _ballStart.AddBrightness(0.01f);
                _ballEnd.AddBrightness(0.01f);
                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(30))
            {
                _ballStart.AddBrightness(-0.01f);
                _ballEnd.AddBrightness(-0.01f);
                yield return new WaitForFixedUpdate();
            }
        }
    }
    private IEnumerator LoopLineEffect()
    {
        _lineRenderer.startWidth = 1f;
        _lineRenderer.endWidth = 1f;
        while (true)
        {
            Color color;
            color = Utility.StringToColor("#ff0000");
            color.a = 0.5f;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            yield return new WaitForSeconds(0.1f);

            color = Utility.StringToColor("#ff4040");
            color.a = 0.5f;
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreateParticle()
    {
        GameObject go = _ballEnd.gameObject.CreateClone();
        SpriteRenderer sr = go.Component<SpriteRenderer>();
        Transform transform = go.transform;
        transform.SetParent(null);

        sr.sprite = Manager.Resource.LoadResource<Sprite>(Sprites.CharacterSkill.Dino_BossDinoSkill_Red);
        transform.localScale = Vector3.one * 3f;
        sr.SetAlpha(0.5f);
        sr.SetBrightness(0f);

        Vector3 startPos = _ballStart.transform.position;
        Vector3 dir = Manager.Input.attackDirection;
        float dist = Utility.RandomNumber(1, 250);
        transform.position = startPos + dir * dist;


        List<Coroutine> coroutines = new();
        coroutines.Add(StartCoroutine(LoopEffect()));
        coroutines.Add(StartCoroutine(WaitAndDestroy()));

        void DestroyThisClone()
        {
            foreach (Coroutine coroutine in coroutines)
                StopCoroutine(coroutine);

            Destroy(go);
        }

        IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(0.1f);
            DestroyThisClone();
        }

        IEnumerator LoopEffect()
        {
            while (true)
            {
                foreach (int i in Count(10))
                {
                    sr?.AddBrightness(0.02f);
                    transform.localScale += Vector3.one * 0.3f;
                    yield return new WaitForFixedUpdate();
                }
                foreach (int i in Count(10))
                {
                    sr?.AddBrightness(-0.02f);
                    transform.localScale += Vector3.one * -0.3f;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
    private IEnumerator LoopCreateParticle()
    {
        while (true)
        {
            bool characterCheck = Manager.Game.currentCharacter == Characters.Dino;
            if (characterCheck && Manager.Game.isSpecialSkillInvoking)
                CreateParticle();

            yield return new WaitForFixedUpdate();
        }
    }

}
