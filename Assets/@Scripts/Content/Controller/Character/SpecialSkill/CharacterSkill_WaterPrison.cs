using System.Collections;
using UnityEngine;

public class CharacterSkill_WaterPrison : CharacterSkillController
{
    protected override void Start()
    {
        Init();
    }

    private GameObject _child;
    private SpriteRenderer _sr;
    private Collider2D _col;

    private bool IsContactEnemy => _col.IsContact(Prefabs.Play.Enemy);

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        _sr = _child.Component<SpriteRenderer>();
        _col = _child.GetComponent<Collider2D>();

        Manager.Game.onDisarmSpecialSkill.Add(this, OnDisarmSpecialSkill);

        StartCoroutine(LoopInvoking());
        StartCoroutine(LoopBrightnessEffect());
        StartCoroutine(LoopSizeEffect());
    }

    private void OnDisarmSpecialSkill()
    {
        if (Manager.Game.selectedCharacter == Characters.Rather)
        {
            Manager.Game.isSpecialSkillInvoking = false;
            _child.SetActive(false);
        }
    }

    private IEnumerator LoopBrightnessEffect()
    {
        _sr.SetTransparency(0.5f);
        _sr.SetBrightness(0.5f);
        while (true)
        {
            foreach (int i in Count(50))
            {
                _sr.AddBrightness(0.005f);
                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(50))
            {
                _sr.AddBrightness(-0.005f);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator LoopSizeEffect()
    {
        while (true)
        {
            foreach (int i in Count(10))
            {
                _child.transform.localScale += Vector3.one * 0.02f;
                yield return new WaitForFixedUpdate();
            }
            foreach (int i in Count(10))
            {
                _child.transform.localScale += Vector3.one * -0.02f;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private IEnumerator LoopInvoking()
    {
        if (Manager.Game.currentCharacter == Characters.Rather)
            Manager.Game.skillCooltime = 15f;
        
        while (true)
        {
            yield return new WaitUntil(() => Manager.Game.currentCharacter == Characters.Rather);

            if (Manager.Game.skillCooltime <= 0 && Manager.Input.isOnSkill)
            {
                float size = 200;
                _child.transform.localScale = Vector3.one * size;
                _child.SetActive(true);

                float alpah = _sr.color.a;
                _sr.SetAlpha(0.2f);
                _col.enabled = false;
                while (Manager.Input.isOnSkill)
                {
                    Vector3 position = new Vector3();
                    if (Manager.Input.isMobileControl)
                    {
                        float cameraX = Manager.Object.MainCamera.transform.position.x;
                        float dragX = Manager.Input.skillJoystickVector.x;
                        position.x = cameraX + dragX * 240f;
                    }
                    else
                    {
                        position.x = Manager.Input.mousePosition.x;
                    }
                    position.y = -70;
                    transform.position = position;

                    yield return null;
                }
                _sr.SetAlpha(alpah);
                _col.enabled = true;


                Manager.Game.skillCooltime = 20f;
                Manager.Game.isSpecialSkillInvoking = true;
                foreach (int i in Count(10))
                {
                    size -= 10;
                    _child.transform.localScale = Vector3.one * size;
                    yield return new WaitForFixedUpdate();
                }
                Manager.Game.isSpecialSkillInvoking = false;

                foreach (int i in Count(10))
                {
                    if (IsContactEnemy)
                        yield return new WaitForSeconds(1f);
                    else
                        yield return null;
                }
                Manager.Game.onDisarmSpecialSkill.Call();
            }
        }
    }

}
