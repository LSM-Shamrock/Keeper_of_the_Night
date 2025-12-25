using System.Collections;
using UnityEngine;

public class CharacterSkill_Water : BaseController
{
    protected override void Start()
    {
        Init();
    }

    private GameObject _child;

    private void Init()
    {
        _child = transform.GetChild(0).gameObject;
        StartCoroutine(LoopThrow());
    }

    private IEnumerator LoopThrow()
    {
        while (true)
        {
            yield return new WaitUntil(() => Manager.Game.currentCharacter == Sprites.Characters.Rather);
            if (Manager.Input.isPressedAttack)
            {
                StartCoroutine(Throw());
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    private IEnumerator Throw()
    {
        GameObject go = _child.CreateClone();
        Transform transform = go.transform;
        Collider2D col = go.Component<Collider2D>();

        float size = 20f;
        transform.localScale = Vector3.one * size;

        transform.position = Manager.Game.Character.position;

        Vector3 direction = (Utility.MousePosition - transform.position).normalized;
        transform.rotation = Utility.Direction2Rotation(direction);

        go.SetActive(true);

        float fallAndSlowdown = 0.5f;

        while (!col.IsContact(PlaySceneObjects.Ground))
        {
            transform.position += direction * (4f / (fallAndSlowdown / 2f));
            transform.AddY(-fallAndSlowdown);
            fallAndSlowdown += 0.05f;

            if (col.IsContact(Prefabs.Scene_Play.Enemy))
            {
                if (size > 10f)
                {
                    size -= 0.5f;
                    transform.localScale = Vector3.one * size;
                }
                else
                {
                    Destroy(go);
                    yield break;
                }
            }

            yield return new WaitForFixedUpdate();
        }
        Destroy(go);
    }
}
