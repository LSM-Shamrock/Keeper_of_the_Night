using System;
using System.Collections;
using UnityEngine;

public class CharacterSkill_MoonlightSword : BaseController
{
    private GameObject _child;

    protected override void Start()
    {
        Init();
    }

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        StartCoroutine(LoopSwinging());
    }

    private void CreateClone()
    {
        GameObject go = Instantiate(_child);
        go.name = _child.name;
        go.transform.position = _child.transform.position;
        go.transform.rotation = _child.transform.rotation;

        StartCoroutine(RoutineEffect(go));
        IEnumerator RoutineEffect(GameObject go)
        {
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.SetTransparency(0.75f);
            foreach (int i in Count(10))
            {
                sr.AddTransparency(0.025f);
                yield return new WaitForFixedUpdate();
            }
            Manager.Object.Despawn(go);
        }
    }

    private IEnumerator LoopSwinging()
    {
        while (true)
        {
        Return:
            yield return new WaitUntil(() => Manager.Game.currentCharacter == Characters.Sleepground);
            
            if (Manager.Game.isSpecialSkillInvoking) 
                goto Return;

            if (!Manager.Input.isDragAttack) 
                goto Return;

            _child.SetActive(true);

            int dir = -Math.Sign(Manager.Input.attackDirection.x);
            transform.rotation = Quaternion.Euler(0, 0, dir * 45);

            foreach (int i in Count(9))
            {
                transform.position = Manager.Object.Character.position;
                transform.Translate(Vector3.up * 50f);
                CreateClone();
                transform.Rotate(Vector3.forward * 10f * dir);
                yield return new WaitForFixedUpdate();
            }
            _child.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
