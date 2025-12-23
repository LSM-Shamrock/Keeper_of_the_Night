using System.Collections;
using UnityEngine;
using static Utility;

public class CharacterSkill_MoonlightSword : BaseController
{
    GameObject _child;

    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        StartCoroutine(Loop_Swinging());
    }

    void CreateClone()
    {
        GameObject go = Instantiate(_child);
        go.name = _child.name;
        go.transform.position = _child.transform.position;
        go.transform.rotation = _child.transform.rotation;

        StartCoroutine(RoutineEffect(go));
        IEnumerator RoutineEffect(GameObject go)
        {
            var sr = go.GetComponent<SpriteRenderer>();
            sr.SetTransparency(0.75f);
            foreach (int i in Count(10))
            {
                sr.AddTransparency(0.025f);
                yield return waitForFixedUpdate;
            }
            Destroy(go);
        }
    }

    IEnumerator Loop_Swinging()
    {
        while (true)
        {
            yield return WaitUntil(() => Manager.Game.currentCharacter == Sprites.Characters.Sleepground);
            
            if (Manager.Game.isSpecialSkillInvoking) 
                goto Return;

            if (!Manager.Input.IsMouseClicked) 
                goto Return;

            _child.SetActive(true);
            int rotateDirection = (MouseX > Manager.Game.Character.GetX()) ? -1 : 1;
            transform.rotation = Quaternion.Euler(0, 0, rotateDirection * 45);
            foreach (int i in Count(9))
            {
                transform.position = Manager.Game.Character.position;
                transform.Translate(Vector3.up * 50f);
                CreateClone();
                transform.Rotate(Vector3.forward * 10f * rotateDirection);
                yield return waitForFixedUpdate;
            }
            _child.SetActive(false);
            yield return WaitForSeconds(0.2f);


        Return:
            yield return waitForFixedUpdate;
        }
    }
}
