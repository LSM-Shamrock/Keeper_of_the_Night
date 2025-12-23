using System.Collections;
using UnityEngine;

public class CharacterSkill_WaterPrison : BaseController
{
    protected override void Start()
    {
        Init();
    }

    GameObject _child;
    SpriteRenderer _sr;
    Collider2D _col;

    bool IsContactEnemy => _col.IsContact(Prefabs.Scene_Play.Enemy);

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _col = _child.GetComponent<Collider2D>();

        Manager.Game.onDisarmSpecialSkill.Add(this, OnDisarmSpecialSkill);

        StartCoroutine(Loop_Invoking());
        StartCoroutine(Loop_BrightnessEffect());
        StartCoroutine(Loop_SizeEffect());
    }

    void OnDisarmSpecialSkill()
    {
        if (Manager.Game.selectedCharacter == Sprites.Characters.Rather)
        {
            Manager.Game.isSpecialSkillInvoking = false;
            _child.SetActive(false);
        }
    }

    IEnumerator Loop_BrightnessEffect()
    {
        _sr.SetTransparency(0.5f);
        _sr.SetBrightness(0.5f);
        while (true)
        {
            foreach (int i in Count(50))
            {
                _sr.AddBrightness(0.005f);
                yield return waitForFixedUpdate;
            }
            foreach (int i in Count(50))
            {
                _sr.AddBrightness(-0.005f);
                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_SizeEffect()
    {
        while (true)
        {
            foreach (int i in Count(10))
            {
                _child.transform.localScale += Vector3.one * 0.02f;
                yield return waitForFixedUpdate;
            }
            foreach (int i in Count(10))
            {
                _child.transform.localScale += Vector3.one * -0.02f;
                yield return waitForFixedUpdate;
            }

            yield return waitForFixedUpdate;
        }
    }

    IEnumerator Loop_Invoking()
    {
        if (Manager.Game.currentCharacter == Sprites.Characters.Rather)
            Manager.Game.specialSkillCooltime = 15f;
        
        while (true)
        {
            yield return WaitUntil(() => Manager.Game.currentCharacter == Sprites.Characters.Rather);

            if (Manager.Game.specialSkillCooltime <= 0 && Manager.Input.IsPressedS)
            {
                Manager.Game.specialSkillCooltime = 20f;
                Manager.Game.isSpecialSkillInvoking = true;

                transform.SetY(-70);
                transform.SetX(Utility.MouseX);

                float size = 200;
                _child.transform.localScale = Vector3.one * size;
                _child.SetActive(true);
                foreach (int i in Count(10))
                {
                    size -= 10;
                    _child.transform.localScale = Vector3.one * size;
                    yield return waitForFixedUpdate;
                }
                Manager.Game.isSpecialSkillInvoking = false;
                foreach (int i in Count(10))
                {
                    if (IsContactEnemy)
                        yield return WaitForSeconds(1f);

                    yield return waitForFixedUpdate;
                }
                Manager.Game.onDisarmSpecialSkill.Call();
            }

            yield return waitForFixedUpdate;
        }
    }

}
